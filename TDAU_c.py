# ---------- TDAU Device Personality Module
# Copyright 2011-2019 Intel Corporation
# This module uses PySerial to communicate via RS232 (physically USB)
# Author: George McLam -- george.e.mclam@intel.com
#         I also wrote TDAU firmware through version 1.15/128.24
#
# 2019-04-04 Misc update for v3 compatibility
# 2019-05-15 Create version for Python 3
# 2019-07-09 Added fields to fnSaveToFile
# 2019-11-05 Corrected math for VADC offset negative
# 2019-11-05 Added float of uC A-D Int Vref
# 2019-11-06 Changed how float converted to string

# REVISION: 2019-11-07
#
# Standard functions in this module:
#           fnAskRawString()                     Write query, return reply
#           fnConnect()                          Connect via RS-232 (USB)
#           fnDict()                             Get module dictionary
#           fnDisconnect()                       Disconnect
#           fnDoSequenceList()                   Perform sequence list
#           fnEng()                              Return string of float in engineer notation
#           fnRdRawString()                      Read Raw String
#           fnVersion()                          Get module version info
#           fnWrRawString()                      Write Raw String

# TDAU specific functions:
#           fnCalibration()                      Initiate Auto Calibration
#           fnCalRTD()                           Initiate RTD Calibration
#           fnExtendedCalibration()              Initiate Extended Calibration
#           fnFactoryCalibration()               Factory Calibration
#           fnFlush()                            Flush log
#           fnLock()                             Lock memory access
#           fnRdExtendedError()                  Read Extended Error
#           fnRdFloat()                          Return float of value in memory
#           fnRdFWVersion()                      Request Firmware Version
#           fnRdLog()                            Send block of logged data
#           fnRdMemory()                         Read Memory
#           fnRdReply()                          Read Reply from TDAU
#           fnRdSerialNumber()                   Read Unit Serial Number
#           fnRdTemperature()                    Read Temperature
#           fnResend()                           Resend prior response
#           fnSaveMemory()                       Save RAM to EEPROM
#           fnSaveToFile()                       Save User memory to file (was fnWriteFile)
#           fnSCOCalibration()                   Initiate Single Current Offset Calibration
#           fnShowConfiguration()                Display TDAU's user configuration
#           fnShowDynamic()                      Display TDAU's dynamic readings
#           fnShowProtected()                    Display TDAU's factory configuration
#           fnShowTemperatures()                 Display TDAU's temperature memory
#           fnStartConversion()                  Start Temperature Conversion
#           fnStopConversion()                   Stop Temperature Conversion
#           fnUnlock()                           Unlock memory access
#           fnWrFWUpdate()                       Update Firmware from HEX file
#           fnWrMemory()                         Write Memory

import re
import sys
import string
import time
import serial
import struct                            # Used by unpack
import math                              # Used by powerise10, floor, log10
from ctypes import *                     # Used by cnvfloat

CR = chr(13)
LF = chr(10)
SLAVE = 0x01                             # Slave address

# ----- Command format
#  SEND: <slave> <command> [data] [checksum]
# REPLY: <rep code> [data] [checksum]

# ----- Serial commands
CMD_BLOCK  = 0x0F                        # Read block of data from log
CMD_CAL    = 0x0C                        # Calibrate command
CMD_EXTC   = 0x0D                        # Extended calibrate command
CMD_FACC   = 0x0E                        # Factory calibrate
CMD_FLLOG  = 0x06                        # Flush log command
CMD_LEAK   = 0x0F                        # Perform Leakage tests
CMD_LOCK   = 0x0A                        # Lock command
CMD_PGM1   = 0x2D                        # Program Flash cmd byte 1
CMD_PGM2   = 0xD2                        # Program Flash cmd byte 2
CMD_RDF0   = 0x36                        # Read flash page 0
CMD_RDF1   = 0x37                        # Read flash page 1
CMD_RDR0   = 0x34                        # Read absolute address page 0
CMD_RDR1   = 0x35                        # Read absolute address page 1
CMD_RDERR  = 0x02                        # Read extended error
CMD_RDLOG  = 0x05                        # Read next log record
CMD_RDMEM  = 0x07                        # Read memory map command
CMD_RDSER  = 0x04                        # Read serial number
CMD_RSEND  = 0x21                        # Resend prior response
CMD_RTD    = 0x22                        # Calibrate RTD
CMD_SAVEM  = 0x09                        # Save to EEPROM
CMD_SCO    = 0x20                        # Single current offset calib
CMD_START  = 0x01                        # Start conversion command
CMD_STAT1  = 0x11                        # Request channel 1
CMD_STAT2  = 0x12                        # Request channel 2
CMD_STAT3  = 0x14                        # Request channel 3
CMD_STAT4  = 0x18                        # Request channel 4
CMD_STATA  = 0x1F                        # Request all channels
CMD_STOP   = 0x10                        # Stop conversion
CMD_TEST1  = 0x33                        # Test command byte 1
CMD_TEST2  = 0xCC                        # Test command byte 2
CMD_UNLK   = 0x0B                        # Unlock command
CMD_VREQ   = 0x03                        # Firmware version request
CMD_WRMEM  = 0x08                        # Write memory command

# ----- Response codes
R_COND     = 0x80                        # Conditional reply
R_ERR      = 0x82                        # Error message
R_FWVER    = 0x83                        # Firmware version
R_SER      = 0x84                        # Serial version
R_LOG      = 0x85                        # Log record
R_MEM      = 0x87                        # Memory contents
R_STAT1    = 0x91                        # 1 channel in reply
R_STAT2    = 0x92                        # 2 channels in reply
R_STAT3    = 0x93                        # 3 channels in reply
R_STAT4    = 0x94                        # 4 channels in reply

# ----- Reply conditions
C_PASS     = 0x41                        # Pass
C_INVC     = 0x42                        # Invalid command
C_INAC     = 0x43                        # Inactive command
C_BADCS    = 0x44                        # Bad checksum
C_BUSY     = 0x45                        # Busy
C_ERR      = 0x46                        # Error
C_RANGE    = 0x47                        # Value out of range
C_NODATA   = 0x48                        # No more data
C_OVERF    = 0x49                        # Receiver overflow
C_BOOT     = 0x4A                        # Boot code not found


class TDAU():
    def __init__(self):
        self.Module_Name = "TDAU"
        self.Module_Version_Miles = 3
        self.Module_Version_Major = 0
        self.Module_Version_Minor = 5
        self.Module_Version_Month = 11
        self.Module_Version_Date = 7
        self.Module_Version_Year = 2019
        self.Module_Version_Time = "08:00"
        self.bCommEnabled = False
        self.hTDAU = None                        # Handle to device
        self.TxBuffer = [None] * 39              # Tx Buffer
        self.TxCount = 0                         # Number of bytes to transmit
        self.SerialTimeout = 5                   # Seconds to wait before serial timeout
        self.bExceptionEnableConnect = False     # Exception error if fail to connect?
        self.bExceptionEnableComError = False    # Enable exception error for general communication error?
        return

# ---------- Simulate ASK Command with Raw String to TDAU ----------
    def fnAskRawString(self,fDelay,sCommand):
        """
        Write raw string to TDAU
        Parameters: string/int/float: time to wait after write before read
                    string: raw string to send
        Returns:    string: string from TDAU
                     OR bool: False upon error
        """
        if not self.bCommEnabled:                                # Port not open
            return False
        if type(fDelay) == str:
            try:
                fDelay = eval(fDelay)
            except:
                fDelay = 0
        bResult = self.fnWrRawString(sCommand)
        if fDelay != 0:
            time.sleep(fDelay)
        if bResult:
            return self.fnRdRawString()
        return False

# ---------- Connect to TDAU ----------
    def fnConnect(self,COMPort):
        """
        Connect to TDAU
        Parameters: int: Port number
        Returns:    bool: True if successful
                          False if unsuccessful
        """
        sCOMPort = "COM{}".format(COMPort)
        self.bCommEnabled = True                 # Must be set to run fnCheckCommunication
        print("Connecting to Thermal Diode Acq Unit... ",end="")
        try:
            self.hTDAU = serial.Serial(sCOMPort,38400,serial.EIGHTBITS,serial.PARITY_NONE,serial.STOPBITS_ONE)
            if self.fnCheckCommunication():
                print("Connected on port {}".format(sCOMPort))
                return True
            else:
                print("Unable to communicate on port {}".format(sCOMPort))
                if self.bExceptionEnableConnect:
                    sMessage = "Unable to connect to TDAU on port {}".format(sCOMPort)
                    raise Exception(sMessage)
                self.bCommEnabled = False
                return False
        except:
            print("Unable to open port {}".format(sCOMPort))
            if self.bExceptionEnableConnect:
                sMessage = "Unable to open COM{:d}".format(COMPort)
                raise Exception(sMessage)
            self.hTDAU = None
            self.bCommEnabled = False
            return False
        return True

# ---------- Return dictionary ----------
    def fnDict(self,bPrint=False):
        """
        Return dictionary
        Parameters: bool: Display (default False)
        Returns:    dict: dictionary from module
        """
        dDict = globals()
        if bPrint:
            LsDict = sorted(dDict.keys())
            iLen = len(LsDict)
            for x in range(iLen):
                print(LsDict[x])
        return dDict

# ---------- Disconnect TDAU ----------
    def fnDisconnect(self,):
        """
        Disconnect TDAU
        Parameters: None
        Returns:    bool: True if successful
                          False if not connected
        """
        if not self.bCommEnabled:                                # Port not open
            return False
        self.hTDAU.close()
        #print("TDAU Communication Port Closed")
        self.bCommEnabled = False
        return True

# ---------- Process list of commands in Sequence tuple ----------
    def fnDoSequenceList(self,ListName):
        """
        Process list of commands in Sequence tuple
        Parameters: string: Name of list to process
        Returns:    bool: True if list processed
                          False if no action taken
        List Format:
                    Element 1 is the function to perform or string to write
                    Element 2 is the amount of time to delay in seconds after the write
                    Element 3.0 = 1 means string is a function, else a raw string
                    Element 3.1 = 1 means write to module, else write to hardware  NOT USED HERE
                    Element 3.2 = 1 means don't add "Result = " to function
                    Element 3.4 = 1 means set Debug = True
                    Element 3.5 = 1 means use try/except, display error, continue
                    Element 3.6 = 1 means ignore return values (no warnings or errors)
                    Element 3.7 = 1 means trap exception errors, otherwise only display them
        """
        if not self.bCommEnabled:                                # Port not open
            return False
        if len(ListName) == 0:
            return False                                         # Sequence list is empty
        bDebug = False
        NumWrites = len(ListName) // 3                           # Three entries per write
        print("Sending {:d} messages to TDAU...".format(NumWrites))
        LIndex = 0
        for ii in range(0,NumWrites,1):                          # Do all entries in sequence
            sString = ListName[LIndex]
            if len(sString) != 0:
                if (ListName[LIndex+2] & 0x10) == 0x10:          # Debug?
                    bDebug = True
                else:
                    bDebug = False
                if (ListName[LIndex+2] & 0x01) == 0x01:          # It's a function
                    if (ListName[LIndex+2] & 0x04) == 0x04:      # Supress Result =
                        sFunction = "self.{}".format(sString)
                        Result = True
                    else:
                        sFunction = "Result = self.{}".format(sString)
                    if bDebug:
                        print(sFunction)
                    if (ListName[LIndex+2] & 0x20) != 0:         # Use try/except
                        try:
                            exec(sFunction)
                        except:
                            print("Function {} did not work".format(sFunction))
                    else:
                        exec(sFunction)
                    if (ListName[LIndex+2] & 0x40) == 0:         # Don't ignore return
                        if not Result:
                            print("Error writing to TDAU: {}".format(sFunction))
                            if (ListName[LIndex+2] & 0x80) == 0x80:  # Trap error?
                                raise Exception("Error writing to TDAU")
                else:                                            # Otherwise raw string to write
                    self.fnWrRawString(sString)
                    if bDebug:
                        print(sString)
                    self.fnRdRawString()                         # Read and toss reply from write
            if ListName[LIndex+1] != 0:                          # Optional delay after each write
                time.sleep(ListName[LIndex+1])
            LIndex += 3
        return True

# ---------- Return a string representing fNum in an engineer friendly notation ----------
    def fnEng(self,fNum):
        """
        Return a string representing fNum in an engineer friendly notation
        Parameters: string/float: number to convert
        Returns:    string
        """
        if type(fNum) == str:
            fNum = float(fNum)
        a,b = self.powerise10(fNum)
        if (-3 < b < 3):
            return "{:.9g}".format(fNum)
        a = a * 10**(b%3)
        b = b - b%3
        return "{:.9g}E{}".format(a,b)

# ---------- Returns fNum as a * 10 ^ b with 0<= a < 10 ----------
    def powerise10(self,fNum):
        """
        Returns fNum as a * 10 ^ b with 0<= a < 10
        Parameters: float: number to convert
        Returns:    two values
        """
        if fNum == 0:
            return 0,0
        Neg = fNum < 0
        if Neg:
            fNum = -fNum
        a = 1.0 * fNum / 10**(math.floor(math.log10(fNum)))
        b = int(math.floor(math.log10(fNum)))
        if Neg:
            a = -a
        return a,b

# ---------- Read Raw String from TDAU ----------
    def fnRdRawString(self):
        """
        Read Raw String from TDAU
        Parameters: None
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        sReceivedData = ""                           # Clear receive buffer
        time.sleep(0.25)                             # Wait for data
        for i in range(0,255,1):                     # Expect 255 characters MAX
            if self.hTDAU.inWaiting() == 0:
                break                                # No characters waiting
            Rx = self.hTDAU.read()                   # Get character
            try:
                RxChar = ord(Rx.decode())
            except:
                RxChar = ord(Rx)
            sReceivedData += str(self.fnHex2Asc((RxChar >> 4) & 0x0F))
            sReceivedData += str(self.fnHex2Asc(RxChar & 0x0F))
            sReceivedData += " "
        return sReceivedData

# ---------- Return Module Version Information ----------
    def fnVersion(self,imode=None):
        """
        Return Module Version Information
        Parameters: None: returns string of Name,Miles,Major.Minor,Year-Month-Date,Time
                     OR string/int of selection:
                        0 = string of module name only
                        1 = string of miles.major.minor only
                        2 = string of year-month-date only
                        3 = string of time only
                        4 = int of miles version only
                        5 = int of major version only
                        6 = int of minor version only
        Returns:    As per above
        """
        if type(imode) == str:
            try:
                smode = eval(imode)
            except:
                smode = None
            imode = smode
        if imode == 0:
            return self.Module_Name
        elif imode == 1:
            sMessage = "{:01d}.{:01d}.{:01d}".format(self.Module_Version_Miles,self.Module_Version_Major,self.Module_Version_Minor)
        elif imode == 2:
            sMessage = "{:04d}-{:02d}-{:02d}".format(self.Module_Version_Year,self.Module_Version_Month,self.Module_Version_Date)
        elif imode == 3:
            return self.Module_Version_Time
        elif imode == 4:
            return self.Module_Version_Miles
        elif imode == 5:
            return self.Module_Version_Major
        elif imode == 6:
            return self.Module_Version_Minor
        else:
            sMessage = ("{},{:01d}.{:01d}".format(self.Module_Name,self.Module_Version_Miles,self.Module_Version_Major)
            + ".{:01d},{:04d}".format(self.Module_Version_Minor,self.Module_Version_Year)
            + "-{:02d}-{:02d},{}".format(self.Module_Version_Month,self.Module_Version_Date,self.Module_Version_Time))
        return sMessage

# ---------- Write Raw String to TDAU ----------
    def fnWrRawString(self,sString):
        """
        Write Raw String to TDAU
        Parameters: string: raw string to send
        Returns:    bool: True if successful
                          False if unsuccessful
        """
        bDebug = False
        if self.bCommEnabled:                        # Port open
            self.fnWrSerialPort(sString)             # Send command to controller
            if bDebug:
                print(sString)
            return True
        return False                                 # Port not open


# ========== TDAU SPECIFIC FUNCTIONS =========================================

# ---------- Auto Calibration ----------
    def fnCalibration(self,PrintMode=False):
        """
        Auto Calibration
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_CAL                   # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- RTD Calibration ----------
    def fnCalRTD(self,PrintMode=False):
        """
        RTD Calibration
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_RTD                   # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Extended Calibration ----------
    def fnExtendedCalibration(self,PrintMode=False):
        """
        Extended Calibration
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_EXTC                  # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Factory Calibration ----------
    def fnFactoryCalibration(self,Mode,Value,PrintMode=False):
        """
        Factory Calibration
        Parameters: string/int: mode (0-9)
                    string/float: value
                      voltage for modes 1-8
                      DAC value for mode 9
                    bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        if type(Mode) == str:
            try:
                iMode = eval(Mode)
            except:
                return False
        elif type(Mode) == int:
            iMode = Mode
        else:
            return False
        self.TxBuffer[0] = CMD_FACC
        self.TxBuffer[1] = iMode
        if iMode == 9:
            if type(Value) == str:
                try:
                    iValue = eval(Value)
                except:
                    return False
            elif type(Value) == int:
                iValue = Value
            else:
                return False
            self.TxBuffer[2] = iValue & 0xFF
            self.TxBuffer[3] = (iValue >> 8) & 0xFF
            self.TxBuffer[4] = 0
            self.TxBuffer[5] = 0
        else:
            if type(Value) == str:
                fValue = float(Value)
            elif type(Value) == float:
                fValue = Value
            else:
                return False
            iValue = self.float_to_hex(fValue)
            self.TxBuffer[2] = iValue & 0xFF
            self.TxBuffer[3] = (iValue >> 8) & 0xFF
            self.TxBuffer[4] = (iValue >> 16) & 0xFF
            self.TxBuffer[5] = (iValue >> 24) & 0xFF
        self.TxCount = 6
        self.fnWrBuffer()
        time.sleep(0.25)
        return self.fnRdReply(PrintMode)

# ---------- Flush Log ----------
    def fnFlush(self,PrintMode=False):
        """
        Flush Log
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_FLLOG                 # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Lock ----------
    def fnLock(self,PrintMode=False):
        """
        Lock
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_LOCK                  # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Read Extended Error ----------
    def fnRdExtendedError(self,PrintMode=False):
        """
        Read Extended Error
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: 00 20 00 02
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_RDERR                 # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Read Float from memory ----------
    def fnRdFloat(self,iAddress,PrintMode=False):
        """
        Read Float from memory
        Parameters: 16 bit int: memory address
                    bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    float: value
                     OR bool: False if not connected
                     OR string of error
        Example: 150.0
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_RDMEM                 # Command to send
        self.TxBuffer[1] = (iAddress & 0xFF)         # Address
        self.TxBuffer[2] = ((iAddress >> 8) & 0xFF)
        self.TxBuffer[3] = 4                         # Quantity of bytes to read
        self.TxCount = 4                             # Number of chars to send
        self.fnWrBuffer()
        sReceivedData = ""                           # Clear receive buffer
        RxChars = [0] * 18
        Count = 0                                    # Number of characters received
        time.sleep(0.25)                             # Wait for reply
        for i in range(0,18,1):                      # Expect 18 characters MAX
            if self.hTDAU.inWaiting() == 0:
                break                                # No characters waiting
            Rx = self.hTDAU.read()                   # Get character
            try:
                RxChar = ord(Rx.decode())
            except:
                RxChar = ord(Rx)
            RxChars[Count] = RxChar
            Count += 1                               # Count it
            sReceivedData += str(self.fnHex2Asc((RxChar >> 4) & 0x0F))
            sReceivedData += str(self.fnHex2Asc(RxChar & 0x0F))
            sReceivedData += " "
        if Count < 3:
            print("Insufficient reply from TDAU {}".format(sReceivedData))
            return "INSUFFICIENT REPLY"
        if RxChars[0] == R_COND:                     # Error response
            return self.ShowError(RxChars[1],PrintMode)
        if RxChars[0] != R_MEM:
            print("Incorrect reply from TDAU")
            return "INCORRECT REPLY"
        if Count < 6:                                # Number of chars received
            print("Insufficient reply from TDAU {}".format(sReceivedData))
            return "INSUFFICIENT REPLY"
        CalcCs = 0                                   # Calculate checksum
        for x in range(0,5,1):
            CalcCs += RxChars[x]
        if (CalcCs & 0xFF) != RxChars[5]:
            print("Checksum error: {}".format(sReceivedData))
            return "BAD CHECKSUM"
        sData = [None] * 4
        for x in range(0,4,1):
            char1 = self.fnHex2Asc(((RxChars[(x+1)] >> 4) & 0x0F),True)
            char2 = self.fnHex2Asc((RxChars[(x+1)] & 0x0F),True)
            sData[x] = str(char1) + str(char2)
        sValue = "{}{}{}{}".format(sData[3],sData[2],sData[1],sData[0])
        iValue = bytes.fromhex(sValue)
        fValue = struct.unpack(">f",iValue)[0]
        #print(sValue)
        if PrintMode:
            #print(self.cnvfloat(sValue))
            print(self.fnEng(fValue))
        #return self.cnvfloat(sValue)
        return fValue

# ---------- Convert string of hex chars to float ----------
    def cnvfloat(self,sValue):
        i = int(sValue,16)                           # Convert from hex str to Python int
        cp = pointer(c_int(i))                       # Make into C int
        fp = cast(cp, POINTER(c_float))              # Cast int pointer to float pointer
        return fp.contents.value

# ---------- Firmware Version Request ----------
    def fnRdFWVersion(self,PrintMode=False):
        """
        Firmware Version Request
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: 128.24
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        bDebug = False
        self.TxBuffer[0] = CMD_VREQ                  # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Read Block of data from log ----------
    def fnRdLog(self,PrintMode=False):
        """
        Read Block of data from log
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: END OF DATA
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_BLOCK                 # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Read Memory ----------
    def fnRdMemory(self,iAddress,iType=4,PrintMode=False):
        """
        Read Memory
        Parameters: 16 bit int: memory address
                    int: memory type/map
                        0 = Absolute RAM page 0
                        1 = Absolute RAM page 1
                        2 = Flash Page 0
                        3 = Flash Page 1
                        4 = RAM
                    bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: F2 FF 00 30 00 00 00 00 00 00 0F 00 01 00 01 00
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[1] = (iAddress & 0xFF)         # Address
        self.TxBuffer[2] = ((iAddress >> 8) & 0xFF)
        self.TxBuffer[3] = 16                        # Quantity of bytes to read (type 4 only)
        dMemMap = {0:(CMD_RDR0,17,3,"SRAM pg0:"),
                   1:(CMD_RDR1,17,3,"SRAM pg1"),
                   2:(CMD_RDF0,17,3,"FLASH p0:"),
                   3:(CMD_RDF1,17,3,"FLASH p1:"),
                   4:(CMD_RDMEM,18,4,"USER RAM:")}
        TxCmd,CountRx,self.TxCount,sRegion = dMemMap[iType]
        self.TxBuffer[0] = TxCmd                     # Command to send
        self.fnWrBuffer()
        sReceivedData = ""                           # Clear receive buffer
        RxChars = [0] * 18
        Count = 0                                    # Number of characters received
        time.sleep(0.25)                             # Wait for reply
        for i in range(0,18,1):                      # Expect 18 characters MAX
            if self.hTDAU.inWaiting() == 0:
                break                                # No characters waiting
            Rx = self.hTDAU.read()                   # Get character
            try:
                RxChar = ord(Rx.decode())
            except:
                RxChar = ord(Rx)
            RxChars[Count] = RxChar
            Count += 1                               # Count it
            sReceivedData += str(self.fnHex2Asc((RxChar >> 4) & 0x0F))
            sReceivedData += str(self.fnHex2Asc(RxChar & 0x0F))
            sReceivedData += " "
        if Count < 3:
            print("Insufficient reply from TDAU {}".format(sReceivedData))
            return sReceivedData
        if RxChars[0] == R_COND:                     # Error response
            return self.ShowError(RxChars[1],PrintMode)
        if Count < CountRx:
            print("Insufficient reply from TDAU {}".format(sReceivedData))
            return sReceivedData
        if iType == 4:                               # Reading memory map
            CalcCs = 0                               # Calculate checksum
            for x in range(0,17,1):
                CalcCs += RxChars[x]
            if (CalcCs & 0xFF) != RxChars[17]:
                print("Checksum error: {}".format(sReceivedData))
                return sReceivedData
        if RxChars[0] == R_MEM:
            sString = ""
            for x in range(0,15,1):# """*********************************************************************************"""
            #for x in range(0,1,1):
                char1 = self.fnHex2Asc(((RxChars[(x+1)] >> 4) & 0x0F),True)
                char2 = self.fnHex2Asc((RxChars[(x+1)] & 0x0F),True)
                sString += str(char1) + str(char2) + " "
            char1 = self.fnHex2Asc(((RxChars[16] >> 4) & 0x0F),True)
            char2 = self.fnHex2Asc((RxChars[16] & 0x0F),True)
            sString += str(char1) + str(char2)
            if PrintMode:
                print("{} {}".format(sRegion,sString))
            return sString
        print("TDAU reply: {}".format(sReceivedData))# Unexpected response
        return sReceivedData

# ---------- Read Reply from TDAU ----------
    def fnRdReply(self,PrintMode=False):
        """
        Read Reply from TDAU
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        dDebug = False
        sReceivedData = ""                           # Clear receive buffer
        RxChars = [0] * 255                          # Maximum number of bytes to Rx
        Count = 0                                    # Number of characters received
        time.sleep(0.25)                             # Wait for reply
        for i in range(0,255,1):                     # Expect 255 characters MAX
            if self.hTDAU.inWaiting() == 0:
                break                                # No characters waiting
            Rx = self.hTDAU.read()                   # Get character
            try:
                RxChar = ord(Rx.decode())
            except:
                RxChar = ord(Rx)
            RxChars[Count] = RxChar
            Count += 1                               # Count it
            sReceivedData += str(self.fnHex2Asc((RxChar >> 4) & 0x0F))
            sReceivedData += str(self.fnHex2Asc(RxChar & 0x0F))
            sReceivedData += " "
        if dDebug:
            for x in range(Count):
                print(hex(RxChars[x]),end="")
            print
        if Count < 3:                                # All transmissions are 3 bytes min
            print("Insufficient reply from TDAU {}".format(sReceivedData))
            return sReceivedData
# Condition response
        if RxChars[0] == R_COND:
            return self.ShowError(RxChars[1],PrintMode)
# Status/Temperature response
        if ((RxChars[0] == R_STAT1)
         or (RxChars[0] == R_STAT2)
         or (RxChars[0] == R_STAT3)
         or (RxChars[0] == R_STAT4)):
            CalcCS = 0
            for i in range(0,(Count-1),1):
                CalcCS += RxChars[i]
            if (CalcCS & 0xFF) != RxChars[(Count-1)]:
                print("Checksum error: {}".format(sReceivedData))
                return False                             # Checksum error
            sString = ""
            LoopCount = (Count-2) // 2
            for i in range(0,(LoopCount),1):
                sTemperature = ("0x" + str(self.fnHex2Asc((RxChars[((i*2)+2)] >> 4) & 0x0F))
                                     + str(self.fnHex2Asc(RxChars[((i*2)+2)] & 0x0F)))
                iTemp = eval(sTemperature)
                iDecimal = eval(str(self.fnHex2Asc(RxChars[((i*2)+1)] & 0x0F)))
                if iDecimal > 9:
                    iDecimal = 9
                fTemperature = float(iTemp) * 10
                fTemperature = (fTemperature + iDecimal) / 10
                if (RxChars[((i*2)+1)] & 0x10) != 0:
                    fTemperature = 0 - fTemperature      # Negative number
                if len(sString) != 0:                    # If appending another channel
                    sString += ","
                sString += str(fTemperature) + "," + str(self.fnHex2Asc((RxChars[((i*2)+1)] >> 4) & 0x0E))
            if PrintMode:
                print("Temperatures: {}".format(sString))
            return sString
# Extended Error response
        if RxChars[0] == R_ERR:
            if Count < 6:
                print("Insufficient reply from TDAU {}".format(sReceivedData))
                return sReceivedData
            if ((RxChars[0] + RxChars[1] + RxChars[2] + RxChars[3] + RxChars[4]) & 0xFF) != RxChars[5]:
                print("Checksum error: {}".format(sReceivedData))
                return sReceivedData
            sString = (str(self.fnHex2Asc((RxChars[1] >> 4) & 0x0F))
                     + str(self.fnHex2Asc(RxChars[1] & 0x0F)) + " "
                     + str(self.fnHex2Asc((RxChars[2] >> 4) & 0x0F))
                     + str(self.fnHex2Asc(RxChars[2] & 0x0F)) + " "
                     + str(self.fnHex2Asc((RxChars[3] >> 4) & 0x0F))
                     + str(self.fnHex2Asc(RxChars[3] & 0x0F)) + " "
                     + str(self.fnHex2Asc((RxChars[4] >> 4) & 0x0F))
                     + str(self.fnHex2Asc(RxChars[4] & 0x0F)) + " ")
            if PrintMode:
                print("Extended Error: {}".format(sString))
                dError1 = {0:"External SRAM test fail",
                           1:"Thermal variation detected",
                           2:"Final temperature out of range",
                           3:"Error from ADC during conversion",
                           4:"Current measurement outside of limits",
                           5:"Internal ADC voltage error detected",
                           6:"SRAM not loaded from EEPROM",
                           7:"Power on CPU reset detected",
                           8:"External CPU reset detected",
                           9:"CPU reset from Brown-out detected",
                          10:"CPU reset from WDT detected",
                          11:"CPU reset from JTAG detected",
                          12:"-2.5V power supply failure",
                          13:"+2.5V power supply failure",
                          14:"+5VA power supply failure",
                          15:"+5VD power supply failure"}
                dError2 = {0:"System busy",
                           1:"Factory calibration required",
                           2:"Boot loader not detected",
                           3:"Too hot condition detected",
                           4:"Catastrophic hot condition detected",
                           5:"Unknown1",
                           6:"Unknown2",
                           7:"Unknown3"}
                dError3 = {0x41:"PASS",
                           0x42:"Invalid command",
                           0x43:"Inactive command",
                           0x44:"Bad checksum",
                           0x45:"Busy",
                           0x46:"Hardware error",
                           0x47:"Out of range value",
                           0x48:"No more data",
                           0x49:"Buffer full",
                           0x4A:"No Boot code"}
                iError1 = (RxChars[1]  << 8) | RxChars[2]
                if iError1 != 0:
                    for x in range(16):
                        if (iError1 & 0x01) != 0:
                            print(dError1[x])
                        iError1 = iError1 >> 1
                iError2 = RxChars[4]
                if iError2 != 0:
                    for x in range(8):
                        if (iError2 & 0x01) != 0:
                            print(dError2[x])
                        iError2 = iError2 >> 1
                iError3 = RxChars[3]
                if iError3 in dError3.keys():
                    print(dError3[iError3])
            return sString
# FirmWare Version response
        if RxChars[0] == R_FWVER:
            if Count < 4:
                print("Insufficient reply from TDAU {}".format(sReceivedData))
                return sReceivedData
            if ((RxChars[0] + RxChars[1] + RxChars[2]) & 0xFF) != RxChars[3]:
                print("Checksum error: {}".format(sReceivedData))
                return sReceivedData
            sChar1 = "0x" + (str(self.fnHex2Asc((RxChars[1] >> 4) & 0x0F))
                           + str(self.fnHex2Asc(RxChars[1] & 0x0F)))
            sChar2 = "0x" + (str(self.fnHex2Asc((RxChars[2] >> 4) & 0x0F))
                           + str(self.fnHex2Asc(RxChars[2] & 0x0F)))
            sString = (str(eval(sChar1)) + "."
                     + str(eval(sChar2)))
            if PrintMode:
                print("FW Version: {}".format(sString))
            return sString                           # Return major.minor
# Serial Number response
        if RxChars[0] == R_SER:
            if Count < 6:
                print("Insufficient reply from TDAU {}".format(sReceivedData))
                return sReceivedData
            if ((RxChars[0] + RxChars[1] + RxChars[2] + RxChars[3] + RxChars[4]) & 0xFF) != RxChars[5]:
                print("Checksum error: {}".format(sReceivedData))
                return sReceivedData
            sSerialNo = eval("0x"
                        + str(self.fnHex2Asc((RxChars[4] >> 4) & 0x0F))
                        + str(self.fnHex2Asc(RxChars[4] & 0x0F))
                        + str(self.fnHex2Asc((RxChars[3] >> 4) & 0x0F))
                        + str(self.fnHex2Asc(RxChars[3] & 0x0F))
                        + str(self.fnHex2Asc((RxChars[2] >> 4) & 0x0F))
                        + str(self.fnHex2Asc(RxChars[2] & 0x0F))
                        + str(self.fnHex2Asc((RxChars[1] >> 4) & 0x0F))
                        + str(self.fnHex2Asc(RxChars[1] & 0x0F)))
            if PrintMode:
                print("Serial Number: {}".format(sSerialNo))
            return sSerialNo
# RdLog response
        if RxChars[0] == R_LOG:
            if Count < 4:
                print("Insufficient reply from TDAU {}".format(sReceivedData))
                return sReceivedData
            #print(sReceivedData)                        # Debug
            iReplySize = RxChars[1] * RxChars[2]         # Quan * record size
            iTotalSize = iReplySize + 3                  # Plus REP Quan Size
            CalcCs = 0                                   # Calculate checksum
            for x in range(0,iTotalSize,1):
                CalcCs += RxChars[x]
            if (CalcCs & 0xFF) != RxChars[iTotalSize]:
                print("Checksum error: {}".format(sReceivedData))
                return sReceivedData
            sString = ""
            for x in range(3,iTotalSize,1):
                char1 = self.fnHex2Asc(((RxChars[x] >> 4) & 0x0F),True)
                char2 = self.fnHex2Asc((RxChars[x] & 0x0F),True)
                sString += str(char1) + str(char2) + " "
            char1 = self.fnHex2Asc(((RxChars[x] >> 4) & 0x0F),True)
            char2 = self.fnHex2Asc((RxChars[x] & 0x0F),True)
            sString += str(char1) + str(char2)
            if PrintMode:
                print(sString)
            return sString
# All other conditions
        if PrintMode:
            print("TDAU reply: {}".format(sReceivedData))
        return sReceivedData


    def ShowError(self,code,PrintMode=False):
        ErrorList = {C_PASS:"PASS",C_INVC:"INVALID COMMAND",C_INAC:"INACTIVE COMMAND",
                     C_BADCS:"BAD CHECKSUM",C_BUSY:"BUSY",C_ERR:"ERROR",C_RANGE:"RANGE",
                     C_NODATA:"END OF DATA",C_OVERF:"Rx BUFFER FULL",C_BOOT:"NO BOOT LOADER"}
        if code in ErrorList.keys():
            sError = ErrorList[code]
        else:
            sError = "UNKNOWN"
        if PrintMode:
            print("TDAU reply: {}".format(sError))
        return sError

# ---------- Read Serial Number ----------
    def fnRdSerialNumber(self,PrintMode=False):
        """
        Read Serial Number
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: 0
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_RDSER                 # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Read Temperature ----------
    def fnRdTemperature(self,ChannelMap,PrintMode=False):
        """
        Read Temperature
        Parameters: byte: ChannelMap (any or all channels to be read)
                      0000 0001 = channel 1
                      0000 0010 = channel 2
                      0000 0100 = channel 3
                      0000 1000 = channel 4
                    bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: -81.0,8,0.0,0,0.0,0,0.0,0
                   0 = No errors
                   1 = Conversion error
                   2 = New conversion
                   4 = System error
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        bDebug = False
        if type(ChannelMap) == str:
            try:
                ChannelMap = eval(ChannelMap)
            except:
                ChannelMap = 15                      # Show all
        if (ChannelMap < 1) or (ChannelMap > 15):
            return False                             # Must specify at least 1 channel
        iCommand = 0x10 | ChannelMap                 # Create command
        self.TxBuffer[0] = iCommand
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Resend prior response ----------
    def fnResend(self):
        """
        Resend prior response
        Parameters: None
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_RSEND                 # Command to send
        self.TxCount = 1                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdRawString()

# ---------- Save Memory ----------
    def fnSaveMemory(self,iAddress,iQuan,PrintMode=False):
        """
        Save Memory to EEPROM
        Parameters: 16 bit int: memory address
                    8 bit int: quantity of bytes to save
                    bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Note:        0         1       2       3      4
                    <command> <addrL> <addrH> <quan> <CS>
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        if iQuan == 0:
            return False                             # No data to save
        iCS = iQuan
        self.TxBuffer[3] = iQuan
        iTemp = (iAddress & 0xFF)                    # Low half of address
        iCS += iTemp
        self.TxBuffer[1] = iTemp                     # Address L
        iTemp = ((iAddress >> 8) & 0xFF)             # High half of address
        iCS += iTemp
        self.TxBuffer[2] = iTemp                     # Address H
        iCS += CMD_SAVEM
        self.TxBuffer[0] = CMD_SAVEM                 # Command
        self.TxBuffer[4] = (iCS & 0xFF)              # CS
        self.TxCount = 5                             # Number of chars to send
        self.fnWrBuffer()
        time.sleep(0.25)                             # Wait for reply
        return self.fnRdReply(PrintMode)

# ---------- Save User memory to file ----------
    def fnSaveToFile(self,sFile="abc"):
        """
        Copy User Memory to CSV file
        Parameters: string: File name
        Returns:    bool: True if successful
                          False if unsuccessful
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        if sFile == None:
            sFile = input("File name: ")
            if len(sFile) == 0:
                return False
        sTest = "abc"	
        sTest = sFile.upper()
        if re.search(".CSV",sTest) != None:
            FileName = sFile
        else:
            FileName = "{}.csv".format(sFile)
        try:
            hFile1 = open(FileName,"wb")
        except:
            print("{} did NOT open".format(FileName))
            return False
        # Type:
        # 0 = No int, show float in raw & float
        # 1 = 1 byte int, float optional
        #     FFF = no float
        #     FF0 = output decimal
        # 2 = 2 byte int, float optional
        #     FFF = no float
        #     FFC = Vref
        #     FFA = temp.status
        #     FF8 = +/- offset calculation
        #     FF4 = divide by 100
        #     FF0 = output decimal
        # 3 = 2 byte int, 2 byte int
        # 4 = 4 byte ADC, float separate
        #     FFF = no float
        #     FF8 = +/- offset calculation
        #     FF6 = Ext ADC temperature
        #     FF2 = Ext ADC voltage
        # 5 = 4 byte int, no float
        # 6 = 2 byte int, 2 byte voltage (x1000.0)
        dMemory = {0:("Control Word 1",                   0x00,2,0xFFF),
                   1:("Control Word 2",                   0x02,2,0xFFF),
                   2:("Control Word 3",                   0x04,2,0xFFF),
                   3:("Control Word 4",                   0x06,2,0xFFF),
                   4:("Trigger Delay",                    0x08,2,0xFF0),
                   5:("Sampling Interval",                0x0A,2,0xFF0),
                   6:("Number of samples to acquire",     0x0C,2,0xFF0),
                   7:("Measurement Averaging",            0x0E,1,0xFF0),
                   8:("Temperature Averaging",            0x10,1,0xFF0),
                   9:("Base Offset DAC Default",          0x12,2,0xFF0),
                  10:("Single I Offset Sampling Interval",0x14,2,0xFF0),
                  11:("Single I Offset Number of samples",0x16,1,0xFF0),
                  12:("Temperature DAC Offset",           0x18,2,0xFF0),
                  13:("Temperature DAC Slope",            0x1A,2,0xFF0),
                  14:("Too Hot Threshold",                0x174,2,0xFF4),
                  15:("Catastrophic Threshold",           0x176,2,0xFF4),
                  16:("System Temperature Delta Hi-Limit",0x1A6,2,0xFF4),
                  17:("Ch1 Ideality Factor",              0,0,0x1C),
                  18:("Ch2 Ideality Factor",              0,0,0x20),
                  19:("Ch3 Ideality Factor",              0,0,0x24),
                  20:("Ch4 Ideality Factor",              0,0,0x28),
                  21:("Ch1 Early Voltage",                0,0,0x2C),
                  22:("Ch2 Early Voltage",                0,0,0x30),
                  23:("Ch3 Early Voltage",                0,0,0x34),
                  24:("Ch4 Early Voltage",                0,0,0x38),
                  25:("Ch1 Single I Slope",               0x3C,2,0xFF8),
                  26:("Ch2 Single I Slope",               0x3E,2,0xFF8),
                  27:("Ch3 Single I Slope",               0x40,2,0xFF8),
                  28:("Ch4 Single I Slope",               0x42,2,0xFF8),
                  29:("Ch1 Temperature Offset",           0,0,0x44),
                  30:("Ch2 Temperature Offset",           0,0,0x48),
                  31:("Ch3 Temperature Offset",           0,0,0x4C),
                  32:("Ch4 Temperature Offset",           0,0,0x50),
                  33:("Ch1 Force Current 1 (Ie1)",        0,0,0x54),
                  34:("Ch1 Force Current 2 (Ie2)",        0,0,0x58),
                  35:("Ch1 Force Current 3 (Ie3)",        0,0,0x5C),
                  36:("Ch2 Force Current 1 (Ie1)",        0,0,0x60),
                  37:("Ch2 Force Current 2 (Ie2)",        0,0,0x64),
                  38:("Ch2 Force Current 3 (Ie3)",        0,0,0x68),
                  39:("Ch3 Force Current 1 (Ie1)",        0,0,0x6C),
                  40:("Ch3 Force Current 2 (Ie2)",        0,0,0x70),
                  41:("Ch3 Force Current 3 (Ie3)",        0,0,0x74),
                  42:("Ch4 Force Current 1 (Ie1)",        0,0,0x78),
                  43:("Ch4 Force Current 2 (Ie2)",        0,0,0x7C),
                  44:("Ch4 Force Current 3 (Ie3)",        0,0,0x80),
                  45:("Ch1 BJT Temp Lo-Limit",            0,0,0x84),
                  46:("Ch1 BJT Temp Hi-Limit",            0,0,0x88),
                  47:("Ch2 BJT Temp Lo-Limit",            0,0,0x8C),
                  48:("Ch2 BJT Temp Hi-Limit",            0,0,0x90),
                  49:("Ch3 BJT Temp Lo-Limit",            0,0,0x94),
                  50:("Ch3 BJT Temp Hi-Limit",            0,0,0x98),
                  51:("Ch4 BJT Temp Lo-Limit",            0,0,0x9C),
                  52:("Ch4 BJT Temp Hi-Limit",            0,0,0xA0),
                  53:("Ch1 Force ie1 Lo-Limit",           0,0,0xA4),
                  54:("Ch1 Force ie1 Hi-Limit",           0,0,0xA8),
                  55:("Ch1 Force ie2 Lo-Limit",           0,0,0xAC),
                  56:("Ch1 Force ie2 Hi-Limit",           0,0,0xB0),
                  57:("Ch1 Force ie3 Lo-Limit",           0,0,0xB4),
                  58:("Ch1 Force ie3 Hi-Limit",           0,0,0xB8),
                  59:("Ch2 Force ie1 Lo-Limit",           0,0,0xBC),
                  60:("Ch2 Force ie1 Hi-Limit",           0,0,0xC0),
                  61:("Ch2 Force ie2 Lo-Limit",           0,0,0xC4),
                  62:("Ch2 Force ie2 Hi-Limit",           0,0,0xC8),
                  63:("Ch2 Force ie3 Lo-Limit",           0,0,0xCC),
                  64:("Ch2 Force ie3 Hi-Limit",           0,0,0xD0),
                  65:("Ch3 Force ie1 Lo-Limit",           0,0,0xD4),
                  66:("Ch3 Force ie1 Hi-Limit",           0,0,0xD8),
                  67:("Ch3 Force ie2 Lo-Limit",           0,0,0xDC),
                  68:("Ch3 Force ie2 Hi-Limit",           0,0,0xE0),
                  69:("Ch3 Force ie3 Lo-Limit",           0,0,0xE4),
                  70:("Ch3 Force ie3 Hi-Limit",           0,0,0xE8),
                  71:("Ch4 Force ie1 Lo-Limit",           0,0,0xEC),
                  72:("Ch4 Force ie1 Hi-Limit",           0,0,0xF0),
                  73:("Ch4 Force ie2 Lo-Limit",           0,0,0xF4),
                  74:("Ch4 Force ie2 Hi-Limit",           0,0,0xF8),
                  75:("Ch4 Force ie3 Lo-Limit",           0,0,0xFC),
                  76:("Ch4 Force ie3 Hi-Limit",           0,0,0x100),
                  77:("Ch1 Current ib1 Lo-Limit",         0,0,0x104),
                  78:("Ch1 Current ib1 Hi-Limit",         0,0,0x108),
                  79:("Ch1 Current ib2 Lo-Limit",         0,0,0x10C),
                  80:("Ch1 Current ib2 Hi-Limit",         0,0,0x110),
                  81:("Ch1 Current ib3 Lo-Limit",         0,0,0x114),
                  82:("Ch1 Current ib3 Hi-Limit",         0,0,0x118),
                  83:("Ch2 Current ib1 Lo-Limit",         0,0,0x11C),
                  84:("Ch2 Current ib1 Hi-Limit",         0,0,0x120),
                  85:("Ch2 Current ib2 Lo-Limit",         0,0,0x124),
                  86:("Ch2 Current ib2 Hi-Limit",         0,0,0x128),
                  87:("Ch2 Current ib3 Lo-Limit",         0,0,0x12C),
                  88:("Ch2 Current ib3 Hi-Limit",         0,0,0x130),
                  89:("Ch3 Current ib1 Lo-Limit",         0,0,0x134),
                  90:("Ch3 Current ib1 Hi-Limit",         0,0,0x138),
                  91:("Ch3 Current ib2 Lo-Limit",         0,0,0x13C),
                  92:("Ch3 Current ib2 Hi-Limit",         0,0,0x140),
                  93:("Ch3 Current ib3 Lo-Limit",         0,0,0x144),
                  94:("Ch3 Current ib3 Hi-Limit",         0,0,0x148),
                  95:("Ch4 Current ib1 Lo-Limit",         0,0,0x14C),
                  96:("Ch4 Current ib1 Hi-Limit",         0,0,0x150),
                  97:("Ch4 Current ib2 Lo-Limit",         0,0,0x154),
                  98:("Ch4 Current ib2 Hi-Limit",         0,0,0x158),
                  99:("Ch4 Current ib3 Lo-Limit",         0,0,0x15C),
                 100:("Ch4 Current ib3 Hi-Limit",         0,0,0x160),
                 101:("Ch1 Leakage Hi-Limit",             0,0,0x164),
                 102:("Ch2 Leakage Hi-Limit",             0,0,0x168),
                 103:("Ch3 Leakage Hi-Limit",             0,0,0x16C),
                 104:("Ch4 Leakage Hi-Limit",             0,0,0x170),
                 105:("Voltage A-D VREF",                 0,0,0x184),
                 106:("Voltage A-D FS Calibration",       0,0,0x188),
                 107:("Current A-D VREF",                 0,0,0x18C),
                 108:("Current A-D FS Calibration",       0,0,0x190),
                 109:("Current DAC VREF",                 0,0,0x194),
                 110:("Current DAC Offset",               0x198,2,0xFF8),
                 111:("Current DAC Scale",                0,0,0x19A),
                 112:("CPU ADC VRef",                     0x19E,2,0xFFF),
                 113:("Base DAC Offset",                  0x1A0,2,0xFF8),
                 114:("Base DAC Scale",                   0,0,0x1A2),
                 115:("Voltage A-D In 1 Offset",          0x1A8,2,0xFF8),
                 116:("Voltage A-D In 2 Offset",          0x1AA,2,0xFF8),
                 117:("Voltage A-D In 3 Offset",          0x1AC,2,0xFF8),
                 118:("Voltage A-D In 4 Offset",          0x1AE,2,0xFF8),
                 119:("Voltage A-D In 5 Offset",          0x1B0,2,0xFF8),
                 120:("Voltage A-D In 6 Offset",          0x1B2,2,0xFF8),
                 121:("Voltage A-D In 7 Offset",          0x1B4,2,0xFF8),
                 122:("Voltage A-D In 8 Offset",          0x1B6,2,0xFF8),
                 123:("Voltage A-D In 1 Scale",           0,0,0x1B8),
                 124:("Voltage A-D In 2 Scale",           0,0,0x1BC),
                 125:("Voltage A-D In 3 Scale",           0,0,0x1C0),
                 126:("Voltage A-D In 4 Scale",           0,0,0x1C4),
                 127:("Voltage A-D In 5 Scale",           0,0,0x1C8),
                 128:("Voltage A-D In 6 Scale",           0,0,0x1CC),
                 129:("Voltage A-D In 7 Scale",           0,0,0x1D0),
                 130:("Voltage A-D In 8 Scale",           0,0,0x1D4),
                 131:("Current A-D In 1 Offset",          0x1D8,2,0xFF8),
                 132:("Current A-D In 2 Offset",          0x1DA,2,0xFF8),
                 133:("Current A-D In 3 Offset",          0x1DC,2,0xFF8),
                 134:("Current A-D In 4 Offset",          0x1DE,2,0xFF8),
                 135:("Current A-D In 5 Offset",          0x1E0,2,0xFF8),
                 136:("Current A-D In 6 Offset",          0x1E2,2,0xFF8),
                 137:("Current A-D In 7 Offset",          0x1E4,2,0xFF8),
                 138:("Current A-D In 8 Offset",          0x1E6,2,0xFF8),
                 139:("Current A-D In 1 Scale",           0,0,0x1E8),
                 140:("Current A-D In 2 Scale",           0,0,0x1EC),
                 141:("Current A-D In 3 Scale",           0,0,0x1F0),
                 142:("Current A-D In 4 Scale",           0,0,0x1F4),
                 143:("Current A-D In 5 Scale",           0,0,0x1F8),
                 144:("Current A-D In 6 Scale",           0,0,0x1FC),
                 145:("Current A-D In 7 Scale",           0,0,0x200),
                 146:("Current A-D In 8 Scale",           0,0,0x204),
                 147:("+2.5v PS Voltage Lo-Limit",        0x208,2,0xFF0),
                 148:("+2.5v PS Voltage Hi-Limit",        0x20A,2,0xFF0),
                 149:("-2.5v PS Voltage Lo-Limit",        0x20C,2,0xFF0),
                 150:("-2.5v PS Voltage Hi-Limit",        0x20E,2,0xFF0),
                 151:("+5v Analog PS Voltage Lo-Limit",   0x210,2,0xFF0),
                 152:("+5v Analog PS Voltage Hi-Limit",   0x212,2,0xFF0),
                 153:("+5v Digital PS Voltage Lo-Limit",  0x214,2,0xFF0),
                 154:("+5v Digital PS Voltage Hi-Limit",  0x216,2,0xFF0),
                 155:("Internal ADC VRef Lo-Limit",       0x218,2,0xFF0),
                 156:("Internal ADC VRef Hi-Limit",       0x21A,2,0xFF0),
                 157:("10ua Test Resistance Lo Limit",    0,0,0x21C),
                 158:("10ua Test Resistance Hi Limit",    0,0,0x220),
                 159:("175ua Test Resistance Lo Limit",   0,0,0x224),
                 160:("175ua Test Resistance Lo Limit",   0,0,0x228),
                 161:("Voltage A-D In6 10ua Test",        0x338,4,0x3F0),
                 162:("Voltage A-D In6 175ua Test",       0x33C,4,0x3F4),
                 163:("Current A-D In6 10ua Test",        0x340,4,0x3F8),
                 164:("Current A-D In6 175ua Test",       0x344,4,0x3FC),
                 165:("Temperature DAC Calibration Offset",0x22C,2,0xFF0),
                 166:("Raw DAC Leakage",                  0,0,0x334),
                 167:("Temperature DAC Calibration Slope",0,0,0x22E),
                 168:("Reserved1",                        0x232,3,0xFFF),
                 169:("Reserved2",                        0x234,3,0xFFF),
                 170:("Reserved3",                        0x236,3,0xFFF),
                 171:("Reserved4",                        0x238,3,0xFFF),
                 172:("Reserved5",                        0x178,3,0xFFF),
                 173:("Reserved6",                        0x17A,3,0xFFF),
                 174:("Reserved7",                        0x17C,3,0xFFF),
                 175:("Reserved8",                        0x17E,3,0xFFF),
                 176:("Month",                            0x23A,1,0xFF0),
                 177:("Date",                             0x23B,1,0xFF0),
                 178:("Year",                             0x23C,2,0xFF0),
                 179:("Voltage A-D In1 Ie1",              0x240,4,0x464),
                 180:("Voltage A-D In1 Ie2",              0x244,4,0x46C),
                 181:("Voltage A-D In1 Ie3",              0x248,4,0x474),
                 182:("Voltage A-D In2 Ie1",              0x24C,4,0x47E),
                 183:("Voltage A-D In2 Ie2",              0x250,4,0x486),
                 184:("Voltage A-D In2 Ie3",              0x254,4,0x48E),
                 185:("Voltage A-D In3 Ie1",              0x258,4,0x498),
                 186:("Voltage A-D In3 Ie2",              0x25C,4,0x4A0),
                 187:("Voltage A-D In3 Ie3",              0x260,4,0x4A8),
                 188:("Voltage A-D In4 Ie1",              0x264,4,0x4B2),
                 189:("Voltage A-D In4 Ie2",              0x268,4,0x4BA),
                 190:("Voltage A-D In4 Ie3",              0x26C,4,0x4C2),
                 191:("Voltage A-D In5 VbOs",             0x270,4,0x368),
                 192:("Voltage A-D In6 ie1",              0x274,4,0x36C),
                 193:("Voltage A-D In6 ie2",              0x278,4,0x370),
                 194:("Voltage A-D In6 ie3",              0x27C,4,0x374),
                 195:("Voltage A-D In7 FullScale",        0x280,4,0x378),
                 196:("Voltage A-D In8 Offset",           0x284,4,0xFF8),
                 197:("Voltage A-D Internal Offset",      0x288,4,0xFF8),
                 198:("Voltage A-D Internal Supply",      0x28C,4,0x380),
                 199:("Voltage A-D Int Temperature",      0x290,4,0x384),
                 200:("Voltage A-D Internal Gain",        0x294,4,0x388),
                 201:("Voltage A-D External Ref",         0x298,4,0x38C),
                 202:("Voltage A-D Factory",              0x29C,4,0xFF8),
                 203:("Current A-D In1 Ib1",              0x2A0,4,0x468),
                 204:("Current A-D In1 Ib2",              0x2A4,4,0x470),
                 205:("Current A-D In1 Ib3",              0x2A8,4,0x478),
                 206:("Current A-D In2 Ib1",              0x2AC,4,0x482),
                 207:("Current A-D In2 Ib2",              0x2B0,4,0x48A),
                 208:("Current A-D In2 Ib3",              0x2B4,4,0x492),
                 209:("Current A-D In3 Ib1",              0x2B8,4,0x49C),
                 210:("Current A-D In3 Ib2",              0x2BC,4,0x4A4),
                 211:("Current A-D In3 Ib3",              0x2C0,4,0x4AC),
                 212:("Current A-D In4 Ib1",              0x2C4,4,0x4B6),
                 213:("Current A-D In4 Ib2",              0x2C8,4,0x4BE),
                 214:("Current A-D In4 Ib3",              0x2CC,4,0x4C6),
                 215:("Current A-D In6 Ch1 ie1",          0x2D0,4,0x41C),
                 216:("Current A-D In6 Ch1 ie2",          0x2D4,4,0x420),
                 217:("Current A-D In6 Ch1 ie3",          0x2D8,4,0x424),
                 218:("Current A-D In6 Ch2 ie1",          0x2DC,4,0x428),
                 219:("Current A-D In6 Ch2 ie2",          0x2E0,4,0x42C),
                 220:("Current A-D In6 Ch2 ie3",          0x2E4,4,0x430),
                 221:("Current A-D In6 Ch3 ie1",          0x2E8,4,0x434),
                 222:("Current A-D In6 Ch3 ie2",          0x2EC,4,0x438),
                 223:("Current A-D In6 Ch3 ie3",          0x2F0,4,0x43C),
                 224:("Current A-D In6 Ch4 ie1",          0x2F4,4,0x440),
                 225:("Current A-D In6 Ch4 ie2",          0x2F8,4,0x444),
                 226:("Current A-D In6 Ch4 ie3",          0x2FC,4,0x448),
                 227:("Current A-D In7 FullScale",        0x300,4,0x37C),
                 228:("Current A-D In8 Offset",           0x304,4,0xFF8),
                 229:("Current A-D Internal Offset",      0x308,4,0xFF8),
                 230:("Current A-D Internal Supply",      0x30C,4,0xFF2),
                 231:("Current A-D Int Temperature",      0x310,4,0xFF6),
                 232:("Current A-D Internal Gain",        0x314,4,0x35C),
                 233:("Current A-D External Ref",         0x318,4,0xFF2),
                 234:("Current A-D Factory",              0x31C,4,0xFF8),
                 235:("uC A-D In1 +2.5V",                 0x320,6,0x360),
                 236:("uC A-D In2 -2.5V",                 0x322,6,0x362),
                 237:("uC A-D In3 +5V Analog",            0x324,6,0x364),
                 238:("uC A-D In4 +5V Digital",           0x326,6,0x366),
                 239:("uC A-D Internal Vref",             0x330,2,0xFFC),
                 240:("uC A-D Internal Offset",           0x332,2,0xFF8),
                 241:("Ch1 Single I Offset",              0,0,0x348),
                 242:("Ch2 Single I Offset",              0,0,0x34C),
                 243:("Ch3 Single I Offset",              0,0,0x350),
                 244:("Ch4 Single I Offset",              0,0,0x354),
                 245:("Ch1 Temperature",                  0x47C,2,0xFFA),
                 246:("Ch1 Minimum",                      0x404,2,0xFFA),
                 247:("Ch1 Maximum",                      0x406,2,0xFFA),
                 248:("Ch1 Average",                      0x408,2,0xFFA),
                 249:("Ch1 Logged Minimum",               0x44C,2,0xFFA),
                 250:("Ch1 Logged Maximum",               0x44E,2,0xFFA),
                 251:("Ch1 Logged Average",               0x450,2,0xFFA),
                 252:("Ch2 Temperature",                  0x496,2,0xFFA),
                 253:("Ch2 Minimum",                      0x40A,2,0xFFA),
                 254:("Ch2 Maximum",                      0x40C,2,0xFFA),
                 255:("Ch2 Average",                      0x40E,2,0xFFA),
                 256:("Ch2 Logged Minimum",               0x452,2,0xFFA),
                 257:("Ch2 Logged Maximum",               0x454,2,0xFFA),
                 258:("Ch2 Logged Average",               0x456,2,0xFFA),
                 259:("Ch3 Temperature",                  0x4B0,2,0xFFA),
                 260:("Ch3 Minimum",                      0x410,2,0xFFA),
                 261:("Ch3 Maximum",                      0x412,2,0xFFA),
                 262:("Ch3 Average",                      0x414,2,0xFFA),
                 263:("Ch3 Logged Minimum",               0x458,2,0xFFA),
                 264:("Ch3 Logged Maximum",               0x45A,2,0xFFA),
                 265:("Ch3 Logged Average",               0x45C,2,0xFFA),
                 266:("Ch4 Temperature",                  0x4CA,2,0xFFA),
                 267:("Ch4 Minimum",                      0x416,2,0xFFA),
                 268:("Ch4 Maximum",                      0x418,2,0xFFA),
                 269:("Ch4 Average",                      0x41A,2,0xFFA),
                 270:("Ch4 Logged Minimum",               0x45E,2,0xFFA),
                 271:("Ch4 Logged Maximum",               0x460,2,0xFFA),
                 272:("Ch4 Logged Average",               0x462,2,0xFFA),
                 273:("Serial Number",                    0x180,5,0xFFF)}
        dMargin = {35:0,83:0,131:0,179:0,227:0,275:0,}
        sWrite = "NAME,ADDRESS,RAW,ADDRESS,FLOAT" + chr(13) + chr(10)
        hFile1.write(sWrite.encode('utf-8'))
        print("Please wait ",end="")
        for x in range(len(dMemory)):
            sName,iAddress,iType,iFloat = dMemory[x]
            if iAddress == 0x4CA:                    # Trap to avoid end of memory range error
                sReply = self.fnRdMemory(0x4C0)
                WordList = sReply.split(" ")
                sVal1 = "0x{}".format(WordList[1])
                iVal1 = eval(sVal1)
                sVal2 = "0x{}".format(WordList[0])
                iVal2 = eval(sVal2)
                if (iVal2 & 0x10) == 0x10:
                    fVal = iVal1 * -1.0
                sWrite = "{},x{:04X},x{}{},,{}".format(sName,iAddress,WordList[11],WordList[10],fVal)
                sWrite += chr(13) + chr(10)
                hFile1.write(sWrite.encode('utf-8'))
            elif iType == 0:
                fVal1 = self.fnRdFloat(iFloat)
                sVal1 = self.fnEng(fVal1)
                sReply = self.fnRdMemory(iFloat)
                WordList = sReply.split(" ")
                sWrite = "{},x{:04X},x{}{}{}{},x{:04X},{}".format(sName,iFloat,WordList[3],WordList[2],WordList[1],WordList[0],iFloat,sVal1)
                sWrite += chr(13) + chr(10)
                hFile1.write(sWrite.encode('utf-8'))
            elif iType == 1:
                sReply = self.fnRdMemory(iAddress)
                WordList = sReply.split(" ")
                if iFloat == 0xFFF:
                    sWrite = "{},x{:04X},x{}".format(sName,iAddress,WordList[0])
                else:
                    sVal = "0x{}".format(WordList[0])
                    iVal = eval(sVal)
                    sWrite = "{},x{:04X},x{}{},,{:d}".format(sName,iAddress,WordList[1],WordList[0],iVal)
                sWrite += chr(13) + chr(10)
                hFile1.write(sWrite.encode('utf-8'))
            elif iType == 2:
                sReply = self.fnRdMemory(iAddress)
                WordList = sReply.split(" ")
                if iFloat == 0xFFF:                      # No float
                    sWrite = "{},x{:04X},x{}{}".format(sName,iAddress,WordList[1],WordList[0])
                elif iFloat == 0xFFC:                    # Vref
                    sVal = "0x{}{}".format(WordList[1],WordList[0])
                    iVal = eval(sVal)
                    fVal = (iVal * 25) / 10000
                    sWrite = "{},x{:04X},x{}{},,{}".format(sName,iAddress,WordList[1],WordList[0],fVal)
                elif iFloat == 0xFFA:                    # Temp.status
                    sVal1 = "0x{}".format(WordList[1])
                    iVal1 = eval(sVal1)
                    sVal2 = "0x{}".format(WordList[0])
                    iVal2 = eval(sVal2)
                    if (iVal2 & 0x10) == 0x10:
                        fVal = iVal1 * -1.0
                    else:
                        fVal = iVal1 * 1.0
                    fVal += ((iVal2 & 0x0F) / 10.0)
                    sWrite = "{},x{:04X},x{}{},,{}".format(sName,iAddress,WordList[1],WordList[0],fVal)
                elif iFloat == 0xFF8:                    # +/- offset
                    sVal = "0x{}{}".format(WordList[1],WordList[0])
                    iVal = eval(sVal)
                    if iVal > 0x7FFF:
                        iVal = (0x10000 - iVal) * -1
                    sWrite = "{},x{:04X},x{}{},,{}".format(sName,iAddress,WordList[1],WordList[0],iVal)
                elif iFloat == 0xFF4:                    # Divide by 100
                    sVal = "0x{}{}".format(WordList[1],WordList[0])
                    iVal = eval(sVal)
                    fVal = iVal / 100.0
                    sWrite = "{},x{:04X},x{}{},,{}".format(sName,iAddress,WordList[1],WordList[0],fVal)
                elif iFloat == 0xFF0:                    # Decimal
                    sVal = "0x{}{}".format(WordList[1],WordList[0])
                    iVal = eval(sVal)
                    sWrite = "{},x{:04X},x{}{},,{:d}".format(sName,iAddress,WordList[1],WordList[0],iVal)
                else:
                    fVal1 = self.fnRdFloat(iFloat)
                    sVal1 = self.fnEng(fVal1)
                    sWrite = "{},x{:04X},x{}{},x{:04X},{}".format(sName,iAddress,WordList[1],WordList[0],iFloat,sVal1)
                sWrite += chr(13) + chr(10)
                hFile1.write(sWrite.encode('utf-8'))
            elif iType == 3:
                sReply = self.fnRdMemory(iAddress)
                WordList1 = sReply.split(" ")
                sWrite = "{},x{:04X},x{}{}".format(sName,iAddress,WordList1[1],WordList1[0])
                if iFloat != 0xFFF:
                    sReply = self.fnRdMemory(iFloat)
                    WordList2 = sReply.split(" ")
                    sWrite += ",x{:04X},x{}".format(iFloat,WordList2[1],WordList2[0])
                sWrite += chr(13) + chr(10)
                hFile1.write(sWrite.encode('utf-8'))
            elif iType == 4:
                sReply = self.fnRdMemory(iAddress)
                WordList = sReply.split(" ")
                if iFloat == 0xFFF:
                    sWrite = "{},x{:04X},x{}{}{}{}".format(sName,iAddress,WordList[0],WordList[1],WordList[2],WordList[3])
                elif iFloat == 0xFF8:                    # +/- offset
                    sVal = "0x{}{}{}".format(WordList[1],WordList[2],WordList[3])
                    iVal = eval(sVal)
                    if iVal > 0x7FFFFF:
                        iVal = (0x1000000 - iVal) * -1
                    sWrite = "{},x{:04X},x{}{}{}{},,{}".format(sName,iAddress,WordList[0],WordList[1],WordList[2],WordList[3],iVal)
                elif iFloat == 0xFF6:                    # Ext ADC temperature
                    sVal = "0x{}{}{}".format(WordList[1],WordList[2],WordList[3])
                    iVal = eval(sVal)
                    if iVal > 0x7FFFFF:
                        iVal = (0x1000000 - iVal) * -1
                    fVal = iVal * 6.357829E-7
                    fVal -= 168E-3
                    fVal = fVal / 394E-6
                    fVal += 25
                    sWrite = "{},x{:04X},x{}{}{}{},,{}".format(sName,iAddress,WordList[0],WordList[1],WordList[2],WordList[3],fVal)
                elif iFloat == 0xFF2:                    # Ext ADC voltage
                    sVal = "0x{}{}{}".format(WordList[1],WordList[2],WordList[3])
                    iVal = eval(sVal)
                    if iVal > 0x7FFFFF:
                        iVal = (0x1000000 - iVal) * -1
                    fVal = iVal / 7.86432E5
                    sWrite = "{},x{:04X},x{}{}{}{},,{}".format(sName,iAddress,WordList[0],WordList[1],WordList[2],WordList[3],fVal)
                else:
                    fVal1 = self.fnRdFloat(iFloat)
                    sVal1 = self.fnEng(fVal1)
                    sWrite = "{},x{:04X},x{}{}{}{},x{:04X},{}".format(sName,iAddress,WordList[0],WordList[1],WordList[2],WordList[3],iFloat,sVal1)
                sWrite += chr(13) + chr(10)
                hFile1.write(sWrite.encode('utf-8'))
            elif iType == 5:
                sReply = self.fnRdMemory(iAddress)
                WordList = sReply.split(" ")
                iVal = "0x{}{}{}{}".format(WordList[3],WordList[2],WordList[1],WordList[0])
                fVal = eval(iVal)
                sWrite = "{},x{:04X},x{}{}{}{},,{:d}".format(sName,iAddress,WordList[3],WordList[2],WordList[1],WordList[0],fVal)
                sWrite += chr(13) + chr(10)
                hFile1.write(sWrite.encode('utf-8'))
            elif iType == 6:
                sReply = self.fnRdMemory(iAddress)
                WordList1 = sReply.split(" ")
                sWrite = "{},x{:04X},x{}{}".format(sName,iAddress,WordList1[1],WordList1[0])
                if iFloat != 0xFFF:
                    sReply = self.fnRdMemory(iFloat)
                    WordList2 = sReply.split(" ")
                    sVal1 = "0x" + WordList2[1] + WordList2[0]
                    iVal1 = eval(sVal1)
                    fVal1 = iVal1 / 10000.0
                    sWrite += ",x{:04X},{:f}".format(iFloat,fVal1)
                sWrite += chr(13) + chr(10)
                hFile1.write(sWrite.encode('utf-8'))
            print(".",end="")
            if x in dMargin.keys():
                print("")
        hFile1.close()
        print
        return True

# ---------- Single Current Offset Calibration ----------
    def fnSCOCalibration(self,PrintMode=False):
        """
        Single Current Offset Calibration
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_SCO
        self.TxCount = 1
        self.fnWrBuffer()
        time.sleep(0.25)
        return self.fnRdReply(PrintMode)

# ---------- Display User Configuration ----------
    def fnShowConfiguration(self,iLevel=0):
        """
        Show TDAU User configuration
        Parameters: string/int: level to show (optional)
                         0 = all
                         1 = no limits
        Returns:    bool: True if successful
                          False if unsuccessful
        """
        dLimit =  {0:("Ch1 BJT Lo Limit",0x084,"Ch1 BJT Hi Limit",0x088),
                   1:("Ch2 BJT Lo Limit",0x08C,"Ch2 BJT Hi Limit",0x090),
                   2:("Ch3 BJT Lo Limit",0x094,"Ch3 BJT Hi Limit",0x098),
                   3:("Ch4 BJT Lo Limit",0x09C,"Ch4 BJT Hi Limit",0x0A0),
                   4:("Ch1 Ie1 Lo Limit",0x0A4,"Ch1 Ie1 Hi Limit",0x0A8),
                   5:("Ch1 Ie2 Lo Limit",0x0AC,"Ch1 Ie2 Hi Limit",0x0B0),
                   6:("Ch1 Ie3 Lo Limit",0x0B4,"Ch1 Ie3 Hi Limit",0x0B8),
                   7:("Ch2 Ie1 Lo Limit",0x0BC,"Ch2 Ie1 Hi Limit",0x0C0),
                   8:("Ch2 Ie2 Lo Limit",0x0C4,"Ch2 Ie2 Hi Limit",0x0C8),
                   9:("Ch2 Ie3 Lo Limit",0x0CC,"Ch2 Ie3 Hi Limit",0x0D0),
                  10:("Ch3 Ie1 Lo Limit",0x0D4,"Ch3 Ie1 Hi Limit",0x0D8),
                  11:("Ch3 Ie2 Lo Limit",0x0DC,"Ch3 Ie2 Hi Limit",0x0E0),
                  12:("Ch3 Ie3 Lo Limit",0x0E4,"Ch3 Ie3 Hi Limit",0x0E8),
                  13:("Ch4 Ie1 Lo Limit",0x0EC,"Ch4 Ie1 Hi Limit",0x0F0),
                  14:("Ch4 Ie2 Lo Limit",0x0F4,"Ch4 Ie2 Hi Limit",0x0F8),
                  15:("Ch4 Ie3 Lo Limit",0x0FC,"Ch4 Ie3 Hi Limit",0x100),
                  16:("Ch1 Ib1 Lo Limit",0x104,"Ch1 Ib1 Hi Limit",0x108),
                  17:("Ch1 Ib2 Lo Limit",0x10C,"Ch1 Ib2 Hi Limit",0x110),
                  18:("Ch1 Ib3 Lo Limit",0x114,"Ch1 Ib3 Hi Limit",0x118),
                  19:("Ch2 Ib1 Lo Limit",0x11C,"Ch2 Ib1 Hi Limit",0x120),
                  20:("Ch2 Ib2 Lo Limit",0x124,"Ch2 Ib2 Hi Limit",0x128),
                  21:("Ch2 Ib3 Lo Limit",0x12C,"Ch2 Ib3 Hi Limit",0x130),
                  22:("Ch3 Ib1 Lo Limit",0x134,"Ch3 Ib1 Hi Limit",0x138),
                  23:("Ch3 Ib2 Lo Limit",0x13C,"Ch3 Ib2 Hi Limit",0x140),
                  24:("Ch3 Ib3 Lo Limit",0x144,"Ch3 Ib3 Hi Limit",0x148),
                  25:("Ch4 Ib1 Lo Limit",0x14C,"Ch4 Ib1 Hi Limit",0x150),
                  26:("Ch4 Ib2 Lo Limit",0x154,"Ch4 Ib2 Hi Limit",0x158),
                  27:("Ch4 Ib3 Lo Limit",0x15C,"Ch4 Ib3 Hi Limit",0x160),
                  28:("Ch1 Leak H Limit",0x164,"Ch2 Leak H Limit",0x168),
                  29:("Ch3 Leak H Limit",0x16C,"Ch4 Leak H Limit",0x170)}
        dConfig = {0:("Ch1 Force Ie1   ",0x54,"Ch1 Force Ie2    ",0x58),
                   1:("Ch1 Force Ie3   ",0x5C,"Ch1 Temp Offset  ",0x44),
                   2:("Ch2 Force Ie1   ",0x60,"Ch2 Force Ie2    ",0x64),
                   3:("Ch2 Force Ie3   ",0x68,"Ch2 Temp Offset  ",0x48),
                   4:("Ch3 Force Ie1   ",0x6C,"Ch3 Force Ie2    ",0x70),
                   5:("Ch3 Force Ie3   ",0x74,"Ch3 Temp Offset  ",0x4C),
                   6:("Ch4 Force Ie1   ",0x78,"Ch4 Force Ie2    ",0x7C),
                   7:("Ch4 Force Ie3   ",0x80,"Ch4 Temp Offset  ",0x50),
                   8:("Ch1 Ideality    ",0x1C,"Ch1 Early Voltage",0x2C),
                   9:("Ch2 Ideality    ",0x20,"Ch2 Early Voltage",0x30),
                  10:("Ch3 Ideality    ",0x24,"Ch3 Early Voltage",0x34),
                  11:("Ch4 Ideality    ",0x28,"Ch4 Early Voltage",0x38)}
        if not self.bCommEnabled:                    # Port not open
            return False
        if type(iLevel) == str:
            sX = iLevel
            try:
                iLevel = eval(sX)
            except:
                iLevel = 0
        dSelect = {0:"2 Current None",
                   1:"3 Current None",
                   2:"2 Current Ideality",
                   3:"3 Current Ideality",
                   4:"2 Current Early",
                   5:"3 Current Early",
                   6:"2 Current NPN DUT",
                   7:"RTD",
                   8:"2 Current None/Leak",
                   9:"3 Current None/Leak",
                  10:"2 Current Ideality/Leak",
                  11:"3 Current Ideality/Leak",
                  12:"2 Current Early/Leak",
                  13:"3 Current Early/Leak",
                  14:"Single Current",
                  15:"Disabled"}
        sReply = self.fnRdMemory(0)
        WordList = sReply.split(" ")
        sVal1 = "0x{}{}".format(WordList[1],WordList[0])
        iVal1 = eval(sVal1)
        sVal2 = "0x{}{}".format(WordList[3],WordList[2])
        iVal2 = eval(sVal2)
        sVal3 = "0x{}{}".format(WordList[5],WordList[4])
        iVal3 = eval(sVal3)
        sVal4 = "0x{}{}".format(WordList[7],WordList[6])
        iVal4 = eval(sVal4)
        sVal5 = "0x{}{}".format(WordList[9],WordList[8])
        iVal5 = eval(sVal5)
        sVal6 = "0x{}{}".format(WordList[11],WordList[10])
        iVal6 = eval(sVal6)
        sVal7 = "0x{}{}".format(WordList[13],WordList[12])
        iVal7 = eval(sVal7)
        sVal8 = "0x{}".format(WordList[15])
        iVal8 = eval(sVal8)
        Ch1 = iVal1 & 0x0F
        Ch2 = (iVal1 >> 4) & 0x0F
        Ch3 = (iVal1 >> 8) & 0x0F
        Ch4 = (iVal1 >> 12) & 0x0F
        Ch1s = iVal4 & 0x0F
        Ch2s = (iVal4 >> 4) & 0x0F
        Ch3s = (iVal4 >> 8) & 0x0F
        Ch4s = (iVal4 >> 12) & 0x0F
        print("Control Word 1: {}{}".format(WordList[1],WordList[0]))
        print("                Ch1: {}".format(dSelect[Ch1]))
        print("                Ch2: {}".format(dSelect[Ch2]))
        print("                Ch3: {}".format(dSelect[Ch3]))
        print("                Ch4: {}".format(dSelect[Ch4]))
        print("Control Word 2: {}{}".format(WordList[3],WordList[2]))
        if (iVal2 & 0x8000) != 0:
            print("                Run auto calibration on falling edge")
        if (iVal2 & 0x4000) != 0:
            print("                Trigger delay active")
        if (iVal2 & 0x2000) != 0:
            print("                Hardware trigger enabled")
        if (iVal2 & 0x1000) != 0:
            print("                Software trigger enabled")
        if (iVal2 & 0x0800) != 0:
            print("                Continuous read mode")
        if (iVal2 & 0x0100) != 0:
            print("                Data logging enabled")
            if (iVal2 & 0x0400) != 0:
                print("                Log averaged temperature reads")
            if (iVal2 & 0x0200) != 0:
                print("                Log parametric data")
        else:
            print("                Data logging disabled")
        if (iVal2 & 0x0007) != 0:
            print("                Common cathode mode enabled")
        if (iVal2 & 0x0010) != 0:
            print("                Ch1 Base Leakage enabled")
        if (iVal2 & 0x0020) != 0:
            print("                Ch2 Base Leakage enabled")
        if (iVal2 & 0x0040) != 0:
            print("                Ch3 Base Leakage enabled")
        if (iVal2 & 0x0080) != 0:
            print("                Ch4 Base Leakage enabled")
        print("Control Word 3: {}{}".format(WordList[5],WordList[4]))
        if (iVal3 & 0x0200) != 0:
            print("                Temperature DAC digital mode enabled")
        else:
            if (iVal3 & 0x0080) != 0:
                print("                HWTRIG is busy output")
        if (iVal3 & 0x0100) != 0:
            print("                Temperature DAC = hottest channel")
        if (iVal3 & 0x0040) != 0:
            print("                Snapshot logging mode enabled")
        if (iVal3 & 0x0020) != 0:
            print("                Start conversion after single I offset")
        if (iVal3 & 0x0010) != 0:
            print("                Leakage current range checking enabled")
        if (iVal3 & 0x0008) != 0:
            print("                Base current range checking enabled")
        if (iVal3 & 0x0004) != 0:
            print("                BJT range checking enabled")
        if (iVal3 & 0x0300) == 0:
            print("                Temperature DAC: Ch{:d}".format((iVal3 & 0x03)+1))
        print("Control Word 4: {}{}".format(WordList[7],WordList[6]))
        if iVal4 != 0:
            print("                Single I Ch1: {}".format(dSelect[Ch1s]))
            print("                Single I Ch2: {}".format(dSelect[Ch2s]))
            print("                Single I Ch3: {}".format(dSelect[Ch3s]))
            print("                Single I Ch4: {}".format(dSelect[Ch4s]))
        print("Trigger delay (seconds)  : {:d}".format(iVal5))
        print("Sample interval (seconds): {:d}".format(iVal6))
        print("Number of Samples to acq : {:d}".format(iVal7))
        print("Measurement avg count    : {:d}".format(iVal8))
        sReply = self.fnRdMemory(0x10)
        WordList = sReply.split(" ")
        sVal1 = "0x{}".format(WordList[0])
        iVal1 = eval(sVal1)
        sVal2 = "0x{}{}".format(WordList[3],WordList[2])
        iVal2 = eval(sVal2)
        sVal3 = "0x{}{}".format(WordList[5],WordList[4])
        iVal3 = eval(sVal3)
        sVal4 = "0x{}".format(WordList[6])
        iVal4 = eval(sVal4)
        sVal5 = "0x{}{}".format(WordList[9],WordList[8])
        iVal5 = eval(sVal5)
        sVal6 = "0x{}{}".format(WordList[11],WordList[10])
        iVal6 = eval(sVal6)
        print("Temperature avg count    : {:d}".format(iVal1))
        print("Base offset DAC default  : {:d}".format(iVal2))
        print("Single I offset interval : {:d}".format(iVal3))
        print("Single I offset samples  : {:d}".format(iVal4))
        print("Temperature DAC offset   : {:d}".format(iVal5))
        print("Temperature DAC slope    : {:d}".format(iVal6))
        sReply = self.fnRdMemory(0x174)
        WordList = sReply.split(" ")
        sVal1 = "0x{}{}".format(WordList[1],WordList[0])
        iVal1 = (eval(sVal1))
        fVal1 = iVal1 / 100.0
        sVal2 = "0x{}{}".format(WordList[3],WordList[2])
        iVal2 = (eval(sVal2))
        fVal2 = iVal2 / 100.0
        print("Too Hot Threshhold       : {}  \tCat Hot Threshhold: {}".format(fVal1,fVal2))
        sReply = self.fnRdMemory(0x3C)
        WordList = sReply.split(" ")
        sVal1 = "0x{}{}".format(WordList[1],WordList[0])
        iVal1 = (eval(sVal1))
        fVal1 = iVal1 / 10.0
        sVal2 = "0x{}{}".format(WordList[3],WordList[2])
        iVal2 = (eval(sVal2))
        fVal2 = iVal2 / 10.0
        sVal3 = "0x{}{}".format(WordList[5],WordList[4])
        iVal3 = (eval(sVal3))
        fVal3 = iVal3 / 10.0
        sVal4 = "0x{}{}".format(WordList[7],WordList[8])
        iVal4 = (eval(sVal4))
        fVal4 = iVal4 / 10.0
        print("Ch1 1 I slope   : {}".format(fVal1))
        print("Ch2 1 I slope   : {}".format(fVal2))
        print("Ch3 1 I slope   : {}".format(fVal3))
        print("Ch4 1 I slope   : {}".format(fVal4))
        for x in range(len(dConfig)):
            sName1,iAddress1,sName2,iAddress2 = dConfig[x]
            fVal1 = self.fnRdFloat(iAddress1)
            sVal1 = self.fnEng(fVal1)
            fVal2 = self.fnRdFloat(iAddress2)
            sVal2 = self.fnEng(fVal2)
            print("{}: {}  \t{}: {}".format(sName1,sVal1,sName2,sVal2))
        if iLevel < 1:
            for x in range(len(dLimit)):
                sName1,iAddress1,sName2,iAddress2 = dLimit[x]
                fVal1 = self.fnRdFloat(iAddress1)
                sVal1 = self.fnEng(fVal1)
                fVal2 = self.fnRdFloat(iAddress2)
                sVal2 = self.fnEng(fVal2)
                print("{}: {}  \t{}: {}".format(sName1,sVal1,sName2,sVal2))
        sReply = self.fnRdMemory(0x230)
        WordList = sReply.split(" ")
        sMonth = "0x{}".format(WordList[10])
        iMonth = eval(sMonth)
        sDay = "0x{}".format(WordList[11])
        iDay = eval(sDay)
        sYear = "0x{}{}".format(WordList[13],WordList[12])
        iYear = eval(sYear)
        print("Manufacture Date: {:d}-{:d}-{:04d}".format(iMonth,iDay,iYear))
        return True

# ---------- Display Dynamic Readings ----------
    def fnShowDynamic(self,iLevel=0):
        """
        Show TDAU Dynamic Readings
        Parameters: string/int: level to show (optional)
                         0 = all
                         1 = no limits
        Returns:    bool: True if successful
                          False if unsuccessful
        """
        dDynamic =  {0:("Voltage A-D in1 (Ch1 Vbe1) ",0x240,0x464),
                     1:("Voltage A-D in1 (Ch1 Vbe2) ",0x244,0x46C),
                     2:("Voltage A-D in1 (Ch1 Vbe3) ",0x248,0x474),
                     3:("Voltage A-D in2 (Ch2 Vbe1) ",0x24C,0x47E),
                     4:("Voltage A-D in2 (Ch2 Vbe2) ",0x250,0x486),
                     5:("Voltage A-D in2 (Ch2 Vbe3) ",0x254,0x48E),
                     6:("Voltage A-D in3 (Ch3 Vbe1) ",0x258,0x498),
                     7:("Voltage A-D in3 (Ch3 Vbe2) ",0x25C,0x4A0),
                     8:("Voltage A-D in3 (Ch3 Vbe3) ",0x260,0x4A8),
                     9:("Voltage A-D in4 (Ch4 Vbe1) ",0x264,0x4B2),
                    10:("Voltage A-D in4 (Ch4 Vbe2) ",0x268,0x4BA),
                    11:("Voltage A-D in4 (Ch4 Vbe3) ",0x26C,0x4C2),
                    12:("Voltage A-D in5 (VbOs)     ",0x270,0x368),
                    13:("Voltage A-D in6 (V@ie1)    ",0x274,0x36C),
                    14:("Voltage A-D in6 (V@ie2)    ",0x278,0x370),
                    15:("Voltage A-D in6 (V@ie3)    ",0x27C,0x374),
                    16:("Voltage A-D in7 (FullScale)",0x280,0x378),
                    17:("Voltage A-D in8 (V-Offset) ",0x284,0),
                    18:("Voltage A-D (Int Offset)   ",0x288,0),
                    19:("Voltage A-D (Int Supply)   ",0x28C,0x380),
                    20:("Voltage A-D (Temperature)  ",0x290,0x384),
                    21:("Voltage A-D (Internal Gain)",0x294,0x388),
                    22:("Voltage A-D (External Ref) ",0x298,0x38C),
                    23:("Voltage A-D (Factory Calib)",0x29C,0),
                    24:("Current A-D in1 (Ch1 Ib1)  ",0x2A0,0x468),
                    25:("Current A-D in1 (Ch1 Ib2)  ",0x2A4,0x470),
                    26:("Current A-D in1 (Ch1 Ib3)  ",0x2A8,0x478),
                    27:("Current A-D in2 (Ch2 Ib1)  ",0x2AC,0x482),
                    28:("Current A-D in2 (Ch2 Ib2)  ",0x2B0,0x48A),
                    29:("Current A-D in2 (Ch2 Ib3)  ",0x2B4,0x492),
                    30:("Current A-D in3 (Ch3 Ib1)  ",0x2B8,0x49C),
                    31:("Current A-D in3 (Ch3 Ib2)  ",0x2BC,0x4A4),
                    32:("Current A-D in3 (Ch3 Ib3)  ",0x2C0,0x4AC),
                    33:("Current A-D in4 (Ch4 Ib1)  ",0x2C4,0x4B6),
                    34:("Current A-D in4 (Ch4 Ib2)  ",0x2C8,0x4BE),
                    35:("Current A-D in4 (Ch4 Ib3)  ",0x2CC,0x4C6),
                    36:("Current A-D in6 (Ch1@Ie1)  ",0x2D0,0x41C),
                    37:("Current A-D in6 (Ch1@Ie2)  ",0x2D4,0x420),
                    38:("Current A-D in6 (Ch1@Ie3)  ",0x2D8,0x424),
                    39:("Current A-D in6 (Ch2@Ie1)  ",0x2DC,0x428),
                    40:("Current A-D in6 (Ch2@Ie2)  ",0x2E0,0x42C),
                    41:("Current A-D in6 (Ch2@Ie3)  ",0x2E4,0x430),
                    42:("Current A-D in6 (Ch3@Ie1)  ",0x2E8,0x434),
                    43:("Current A-D in6 (Ch3@Ie2)  ",0x2EC,0x438),
                    44:("Current A-D in6 (Ch3@Ie3)  ",0x2F0,0x43C),
                    45:("Current A-D in6 (Ch4@Ie1)  ",0x2F4,0x440),
                    46:("Current A-D in6 (Ch4@Ie2)  ",0x2F8,0x444),
                    47:("Current A-D in6 (Ch4@Ie3)  ",0x2FC,0x448),
                    48:("Current A-D in7 (FullScale)",0x300,0x37C),
                    49:("Current A-D in8 (I-Offset) ",0x304,0),
                    50:("Current A-D (Int Offset)   ",0x308,0),
                    51:("Current A-D (Int Supply)   ",0x30C,0),
                    52:("Current A-D (Temperature)  ",0x310,0),
                    53:("Current A-D (Internal Gain)",0x314,0x35C),
                    54:("Current A-D (External Ref) ",0x318,0),
                    55:("Current A-D (Factory Calib)",0x31C,0),
                    56:("Raw DAC Leakage            ",0x334,0),
                    57:("Voltage A-D in6 (10uA test)",0x338,0x3F0),
                    58:("Voltage A-D in6 (175uA tst)",0x33C,0x3F4),
                    59:("Current A-D in6 (10uA test)",0x340,0x3F8),
                    60:("Current A-D in6 (175uA tst)",0x344,0x3FC),
                    61:("Ch 1 Single I Offset       ",0,0x348),
                    62:("Ch 2 Single I Offset       ",0,0x34C),
                    63:("Ch 3 Single I Offset       ",0,0x350),
                    64:("Ch 4 Single I Offset       ",0,0x354)}
        dInternal = {0:("          uC A-D in1 +2P5A ",0x320,0x360),
                     1:("          uC A-D in2 -2P5A ",0x322,0x362),
                     2:("          uC A-D in3 +5A   ",0x324,0x364),
                     3:("          uC A-D in4 +5D   ",0x326,0x366),
                     4:("          uC A-D Int Vref  ",0x330,1),
                     5:("          uC A-D Int offset",0x332,0)}
        if not self.bCommEnabled:                    # Port not open
            return False
        for x in range(len(dDynamic)):
            sName,iADC,iFloat = dDynamic[x]
            if iADC != 0:
                sReply = self.fnRdMemory(iADC)
                WordList = sReply.split(" ")
            else:
                WordList = ["  ","  ","  ","  "]
            if iFloat != 0:
                fFloat1 = self.fnRdFloat(iFloat)
                sFloat1 = self.fnEng(fFloat1)
            else:
                sFloat1 = ""
            print("{}: {} {}{}{}    {}".format(sName,WordList[0],WordList[1],WordList[2],WordList[3],sFloat1))
        for x in range(len(dInternal)):
            sName,iADC,iResult = dInternal[x]
            sReply = self.fnRdMemory(iADC)
            WordList1 = sReply.split(" ")
            if iResult == 0:
                print("{}: {}{}".format(sName,WordList1[1],WordList1[0]))
            elif iResult == 1:
                WordList2 = sReply.split(" ")
                sVal1 = "0x{}{}".format(WordList2[1],WordList2[0])
                iVal1 = eval(sVal1)
                fVal1 = (iVal1 * 25.0) / 10000.0
                print("{}: {}{}    {:f}".format(sName,WordList1[1],WordList1[0],fVal1))
            else:
                sReply = self.fnRdMemory(iResult)
                WordList2 = sReply.split(" ")
                sVal1 = "0x{}{}".format(WordList2[1],WordList2[0])
                iVal1 = eval(sVal1)
                fVal1 = iVal1 / 10000.0
                print("{}: {}{}    {:f}".format(sName,WordList1[1],WordList1[0],fVal1))
        return True

# ---------- Display Factory configuration ----------
    def fnShowProtected(self,iLevel=0):
        """
        Show TDAU Factory configuration
        Parameters: string/int: level to show (optional)
                         0 = all
                         1 = no limits
        Returns:    bool: True if successful
                          False if unsuccessful
        """
        dOffScale  = {0:("Voltage A-D in 1 Offset ",0x1A8,"Voltage A-D in 1 Scale",0x1B8),
                      1:("Voltage A-D in 2 Offset ",0x1AA,"Voltage A-D in 2 Scale",0x1BC),
                      2:("Voltage A-D in 3 Offset ",0x1AC,"Voltage A-D in 3 Scale",0x1C0),
                      3:("Voltage A-D in 4 Offset ",0x1AE,"Voltage A-D in 4 Scale",0x1C4),
                      4:("Voltage A-D in 5 Offset ",0x1B0,"Voltage A-D in 5 Scale",0x1C8),
                      5:("Voltage A-D in 6 Offset ",0x1B2,"Voltage A-D in 6 Scale",0x1CC),
                      6:("Voltage A-D in 7 Offset ",0x1B4,"Voltage A-D in 7 Scale",0x1D0),
                      7:("Voltage A-D in 8 Offset ",0x1B6,"Voltage A-D in 8 Scale",0x1D4),
                      8:("Current A-D in 1 Offset ",0x1D8,"Current A-D in 1 Scale",0x1E8),
                      9:("Current A-D in 2 Offset ",0x1DA,"Current A-D in 2 Scale",0x1EC),
                     10:("Current A-D in 3 Offset ",0x1DC,"Current A-D in 3 Scale",0x1F0),
                     11:("Current A-D in 4 Offset ",0x1DE,"Current A-D in 4 Scale",0x1F4),
                     12:("Current A-D in 5 Offset ",0x1E0,"Current A-D in 5 Scale",0x1F8),
                     13:("Current A-D in 6 Offset ",0x1E2,"Current A-D in 6 Scale",0x1FC),
                     14:("Current A-D in 7 Offset ",0x1E4,"Current A-D in 7 Scale",0x200),
                     15:("Current A-D in 8 Offset ",0x1E6,"Current A-D in 8 Scale",0x204),
                     16:("Current DAC Offset      ",0x198,"Current DAC Scale     ",0x19A),
                     17:("Base DAC Offset         ",0x1A0,"Base DAC Scale        ",0x1A2),
                     18:("Temperature DAC Offset  ",0x22C,"Temperature DAC Scale ",0x22E),
                     19:("uC ADC internal Vref    ",0x19E,"",0)}
        dProtect =   {0:("Voltage A-D Vref        ",0x184,"Voltage A-D FS Calib     ",0x188),
                      1:("Current A-D Vref        ",0x18C,"Current A-D FS Calib     ",0x190)}
        if not self.bCommEnabled:                    # Port not open
            return False
        if type(iLevel) == str:
            sX = iLevel
            try:
                iLevel = eval(sX)
            except:
                iLevel = 0
        for x in range(len(dOffScale)):
            sName1,iAddress1,sName2,iAddress2 = dOffScale[x]
            sReply = self.fnRdMemory(iAddress1)
            WordList = sReply.split(" ")
            sVal1 = "0x{}{}".format(WordList[1],WordList[0])
            iVal1 = eval(sVal1)
            if (iVal1 & 0x8000) != 0:
                iVal1 = (0x10000 - iVal1) * -1
            if iAddress2 != 0:
                fVal1 = self.fnRdFloat(iAddress2)
                sVal1 = self.fnEng(fVal1)
                print("{}: {} \t{}: {}".format(sName1,iVal1,sName2,sVal1))
            else:
                print("{}: {}".format(sName1,iVal1))
        sReply = self.fnRdMemory(0x1A6)
        WordList = sReply.split(" ")
        sVal1s = "0x{}".format(WordList[0])
        iVal1s = eval(sVal1s)
        sVal1t = "0x{}".format(WordList[1])
        iVal1t = eval(sVal1t)
        if (iVal1s & 0x10) != 0:
            iVal1t *= -1
        iVal1s &= 0x0F
        if (iVal1s & 0x10) != 0:
            print("System Temperature Delta:-{}.{}".format(iVal1t,iVal1s))
        else:
            print("System Temperature Delta: {}.{}".format(iVal1t,iVal1s))
        for x in range(len(dProtect)):
            sName1,iAddress1,sName2,iAddress2 = dProtect[x]
            fVal1 = self.fnRdFloat(iAddress1)
            sVal1 = self.fnEng(fVal1)
            fVal2 = self.fnRdFloat(iAddress2)
            sVal2 = self.fnEng(fVal2)
            print("{}: {} \t{}: {}".format(sName1,sVal1,sName2,sVal2))
        if iLevel < 1:
            sReply = self.fnRdMemory(0x208)
            WordList = sReply.split(" ")
            sVal1 = "0x{}{}".format(WordList[1],WordList[0])
            iVal1 = eval(sVal1)
            sVal2 = "0x{}{}".format(WordList[3],WordList[2])
            iVal2 = eval(sVal2)
            sVal3 = "0x{}{}".format(WordList[5],WordList[4])
            iVal3 = eval(sVal3)
            sVal4 = "0x{}{}".format(WordList[7],WordList[6])
            iVal4 = eval(sVal4)
            sVal5 = "0x{}{}".format(WordList[9],WordList[8])
            iVal5 = eval(sVal5)
            sVal6 = "0x{}{}".format(WordList[11],WordList[10])
            iVal6 = eval(sVal6)
            sVal7 = "0x{}{}".format(WordList[13],WordList[12])
            iVal7 = eval(sVal7)
            sVal8 = "0x{}{}".format(WordList[15],WordList[14])
            iVal8 = eval(sVal8)
            print("+2.5 PS Voltage Lo limit:  {:d}  \t\t+2.5 PS Voltage Hi limit :  {:d}".format(iVal1,iVal2))
            print("-2.5 PS Voltage Lo limit:  {:d}  \t\t-2.5 PS Voltage Hi limit :  {:d}".format(iVal3,iVal4))
            print("+5 A PS Voltage Lo limit:  {:d}  \t\t+5 A PS Voltage Hi limit :  {:d}".format(iVal5,iVal6))
            print("+5 D PS Voltage Lo limit:  {:d}  \t\t+5 D PS Voltage Hi limit :  {:d}".format(iVal7,iVal8))
            sReply = self.fnRdMemory(0x218)
            WordList = sReply.split(" ")
            sVal1 = "0x{}{}".format(WordList[1],WordList[0])
            iVal1 = eval(sVal1)
            sVal2 = "0x{}{}".format(WordList[3],WordList[2])
            iVal2 = eval(sVal2)
            print("Internal ADC Vref Lo Lim:  {:3d}  \t\tInternal ADC Vref H Limit: {:d}".format(iVal1,iVal2))
            fVal1 = self.fnRdFloat(0x21C)
            sVal1 = self.fnEng(fVal1)
            fVal2 = self.fnRdFloat(0x220)
            sVal2 = self.fnEng(fVal2)
            print("10uA Resistance Lo Limit: {} \t 10uA Resistance Hi Limit:  {}".format(sVal1,sVal2))
            fVal1 = self.fnRdFloat(0x224)
            sVal1 = self.fnEng(fVal1)
            fVal2 = self.fnRdFloat(0x228)
            sVal2 = self.fnEng(fVal2)
            print("175uA Resistance L Limit: {} \t175uA Resistance Hi Limit:  {}".format(sVal1,sVal2))
        sReply = self.fnRdMemory(0x230)
        WordList = sReply.split(" ")
        sMonth = "0x{}".format(WordList[10])
        iMonth = eval(sMonth)
        sDay = "0x{}".format(WordList[11])
        iDay = eval(sDay)
        sYear = "0x{}{}".format(WordList[13],WordList[12])
        iYear = eval(sYear)
        print("Manufacture Date: {:d}-{:d}-{:04d}".format(iMonth,iDay,iYear))
        return True

# ---------- Display TDAU's temperature memory ----------
    def fnShowTemperatures(self):
        """
        Show TDAU Temperature memory
        Parameters: None
        Returns:    bool: True if successful
                          False if unsuccessful
        """
        dTemps = {0:("Ch1 Cur  Min  Max  Avg",0x404),
                  1:("Ch2 Cur  Min  Max  Avg",0x40A),
                  2:("Ch3 Cur  Min  Max  Avg",0x410),
                  3:("Ch4 Cur  Min  Max  Avg",0x416),
                  4:("Ch1 Log  Min  Max  Avg",0x44C),
                  5:("Ch2 Log  Min  Max  Avg",0x452),
                  6:("Ch3 Log  Min  Max  Avg",0x458),
                  7:("Ch4 Log  Min  Max  Avg",0x45E)}
        if not self.bCommEnabled:                    # Port not open
            return False
        for x in range(len(dTemps)):
            sName,iAddress = dTemps[x]
            sReply = self.fnRdMemory(iAddress)
            WordList = sReply.split(" ")
            sVal1s = "0x{}".format(WordList[0])
            iVal1s = eval(sVal1s)
            sVal1t = "0x{}".format(WordList[1])
            iVal1t = eval(sVal1t)
            sVal2s = "0x{}".format(WordList[2])
            iVal2s = eval(sVal2s)
            sVal2t = "0x{}".format(WordList[3])
            iVal2t = eval(sVal2t)
            sVal3s = "0x{}".format(WordList[4])
            iVal3s = eval(sVal3s)
            sVal3t = "0x{}".format(WordList[5])
            iVal3t = eval(sVal3t)
            if (iVal1s & 0x10) != 0:
                iVal1t *= -1
            iVal1s &= 0x0F
            if (iVal2s & 0x10) != 0:
                iVal2t *= -1
            iVal2s &= 0x0F
            if (iVal3s & 0x10) != 0:
                iVal3t *= -1
            iVal3s &= 0x0F
            print("{}: {}.{}  {}.{}  {}.{}".format(sName,iVal1t,iVal1s,iVal2t,iVal2s,iVal3t,iVal3s))
        return True

# ---------- Start Conversion ----------
    def fnStartConversion(self,PrintMode=False):
        """
        Start Conversion
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_START
        self.TxCount = 1
        self.fnWrBuffer()
        time.sleep(0.25)
        return self.fnRdReply(PrintMode)

# ---------- Stop Conversion ----------
    def fnStopConversion(self,PrintMode=False):
        """
        Stop Conversion
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_STOP
        self.TxCount = 1
        self.fnWrBuffer()
        time.sleep(0.25)
        return self.fnRdReply(PrintMode)

# ---------- Unlock ----------
    def fnUnlock(self,PrintMode=False):
        """
        Unlock
        Parameters: bool:  (optional)
                        True = display messages
                        False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        self.TxBuffer[0] = CMD_UNLK
        self.TxCount = 1
        self.fnWrBuffer()
        time.sleep(0.25)
        return self.fnRdReply(PrintMode)

# ---------- Write FirmWare to TDAU ----------
    def fnWrFWUpdate(self,sFileName=None,iMode=1):
        """
        Write FirmWare to TDAU
        Parameters: string: string of [path\]HEX file
                    int: Mode (optional) default = 1
                        0 = pass raw lines from file to TDAU, display progress
                        1 = pass raw lines from file to TDAU, display HEX
                        2 = parse lines, display each character as sent
        Returns:    bool:
                        True if successful - SEE NOTE
                        False upon error
        Note:       After a successful FW update, TDAU will reboot
        """
        if iMode > 2:
            return False
        TDAUTimeOut = 5                                  # Max time to wait for reply
        if not self.bCommEnabled:                        # Port not open
            return False
        if (sFileName == None) or (type(sFileName) != str):
            print("You must specify the path\\name of the Intel HEX file to program")
            return False
        FileTime = 5                                     # Time to wait for file (seconds)
        FileTimeout = 0                                  # Timeout counter
        while FileTimeout != FileTime:
            try:
                hFileH = open(sFileName, "r")
                break                                    # File opened
            except:
                time.sleep(1)                            # Else wait one second
                FileTimeout += 1                         # Bump counter
        if FileTimeout == FileTime:
            print("{} did not open".format(sFileName))
            return False
        print("THIS FUNCTION UPDATES FIRMWARE IN THE TDAU.")
        print("SOME ERRORS MAY RENDER THE TDAU UNUSABLE!!!")
        print("This process CANNOT be undone or reversed!!")
        sTest = input("Type 123 enter to continue: ")
        if sTest != "123":
            hFileH.close()
            print("Programming ABORTED")
            return False
        tSTART = time.time()
        self.TxBuffer[0] = CMD_PGM1
        self.TxBuffer[1] = CMD_PGM2
        self.TxCount = 2
        self.fnWrBuffer()
        time.sleep(0.250)
        sStatus = self.fnRdReply()
        if sStatus != "PASS":
            print("TDAU Error")
            return False
        self.hTDAU.apply_settings({'write_timeout':30})  # Change timeouts to 30 seconds
        self.hTDAU.apply_settings({'timeout':30})
        linecount = 0;
        while True:
            sInput = hFileH.readline()                   # Read line from file
            iLen = len(sInput)
            if iLen == 0:
                break                                    # EOF
            linecount += 1;
            if iMode == 0:
                print(".",end="")
                if (linecount % 40) == 0:
                    print
                self.fnWrSerialPort(sInput)              # Send raw line to TDAU
            elif iMode == 1:
                print(sInput,end="")
                self.fnWrSerialPort(sInput)              # Send raw line to TDAU
            elif iMode == 2:
                sWrite = ""                              # Clear string to write
                for x in range(0,iLen,1):
                    iFile = sInput[x:(x+1)]
                    if iFile == 0x0D:                    # CR found (ignore)
                        print("CR",end="")
                        continue
                    elif (iFile == 0x0A) or (iFile == '\n'):
                        sWrite += str(chr(0x0A))         # Append LF to string
                        self.fnWrSerialPort(sWrite)      # Write the string
                        print("LF")
                        break
                    else:                                # Not CR or LF
                        sWrite += str(iFile)
                        if (iFile < '0') or (iFile > 'F'):
                            print(iFile,end="")
                        elif (iFile > '9') and (iFile < 'A'):
                            print(iFile,end="")
                        else:
                            iByte = self.Asc2Hex(iFile)
                            print("{:01X}".format(iByte),end="")
                        continue
#...............End of for
            time.sleep(0.100)
            tStart = time.time()                         # Start time
            while True:
                iTest = self.hTDAU.inWaiting()           # Characters waiting?
                if iTest != 0:
                    break
                tNow = time.time()                       # Time now
                if abs(tNow - tStart) >= TDAUTimeOut:
                    self.fnWrSerialPort(str(chr(0x1B)))  # ESCAPE
                    print("TDAU timeout")
                    return False
            Rx = self.hTDAU.read()                       # Get character
            RxChar = ord(Rx.decode())
            if RxChar == C_NODATA:
                print
                print("Programming completed")
                break
            if RxChar != C_PASS:
                dERR = {C_INVC:"ERROR: Invalid command",
                        C_INAC:"ERROR: Inactive command",
                        C_BADCS:"ERROR: Bad checksum",
                        C_BUSY:"ERROR: Busy",
                        C_ERR:"ERROR: General error",
                        C_RANGE:"ERROR: Value out of range",
                        C_NODATA:"No more data",
                        C_OVERF:"ERROR: Receiver overflow",
                        C_BOOT:"ERROR: Boot code not found",
                        C_PASS:"No error"}
                print
                if RxChar not in dERR.keys():
                    print("ERROR: Unknown")
                else:
                    print(dERR[RxChar])
                if self.bExceptionEnableComError:
                    raise Exception("TDAU ERROR")
                return False
#.......End of while
        hFileH.close()
        print("")
        sTime = self.fnCalcTime(time.time() - tSTART)
        print("Elapsed time: {}".format(sTime))
        return True

# ---------- Write Memory ----------
    def fnWrMemory(self,iAddress,tData,PrintMode=False):
        """
        Write Memory
        Parameters: 16 bit int: memory address
                    tuple: int for each byte of data
                    bool:  (optional)
                       True = display messages
                       False = don't display messages DEFAULT
        Returns:    string: string from TDAU
                     OR bool: False if not connected
        Note:        0         1       2       3      4
                    <command> <addrL> <addrH> <quan> <data> <CS>
        Example: PASS
        """
        if not self.bCommEnabled:                    # Port not open
            return False
        #iQuan = len(tData)
        iQuan = 1
        if iQuan == 0:
            return False                             # No data to write
        if (iQuan+6) > len(self.TxBuffer):
            print("Writing too much data (max = 32 bytes)")
            return False
        iCS = iQuan
        self.TxBuffer[3] = iQuan
        iTemp = (iAddress & 0xFF)
        iCS += iTemp
        self.TxBuffer[1] = iTemp                     # Address
        iTemp = ((iAddress >> 8) & 0xFF)
        iCS += iTemp
        self.TxBuffer[2] = iTemp
        iCS += CMD_WRMEM
        self.TxBuffer[0] = CMD_WRMEM                 # Command
        for x in range(0,iQuan,1):
            self.TxBuffer[(x+4)] = tData #[x]
            iCS += tData #[x]
        self.TxBuffer[(4+iQuan)] = (iCS & 0xFF)
        self.TxCount = iQuan+5
        self.fnWrBuffer()
        time.sleep(0.25)
        return self.fnRdReply(PrintMode)


# =================== SUBROUTINES ===================

# ---------- Convert single ASCII character to hex ----------
    def Asc2Hex(self,Ascii):
        """
        INTERNAL USE ONLY: Convert single ASCII character to hex
        Parameters: byte: ASCII character
        Returns:    byte: Hex value (4 bits)
        """
        Value = ord(Ascii)
        if (Value < 48) or (Value > 102):            # Out of range value
            return 0
        if Value >= 97:                              # lower case a to f
            return (Value - 87)
        if Value > 70:                               # Out of range
            return 0
        if Value < 58:
            return (Value - 48)                      # Number 0 to 9
        return (Value - 55)                          # A to F

# ---------- Convert float of time into d h:mm:ss ----------
    def fnCalcTime(self,fTime,bForce=False):
        """
        Convert float of time into d h:mm:ss
        Parameters: float: time in seconds
                    bool: True to force verbose reply (optional)
        Returns:    string: time in h:mm:ss
        """
        fDay = int(fTime / 86400)
        fRem0 = int(fTime % 86400)
        fHr = int(fRem0 / 3600)
        fRem1 = (fRem0 % 3600)
        fMin = int(fRem1 / 60)
        fRem2 = (fRem1 % 60)
        fSec = int(fRem2)
        sTime = ""
        if (fDay != 0) or bForce:
            sTime += "{:d} days ".format(fDay)
        if (fHr != 0) or bForce:
            sTime += "{:d}:".format(fHr)
        sTime += "{:02d}:{:02d}".format(fMin,fSec)
        return sTime

# ---------- Check for serial communication with TDAU ----------
    def fnCheckCommunication(self):
        """
        INTERNAL USE ONLY: Check for serial communication with TDAU
        Parameters: None
        Returns:    bool:
                        True if connected
                        False if not connected
        """
        bDebug = False
        self.hTDAU.timeout = self.SerialTimeout      # Set the timeout
        self.TxBuffer[0] = CMD_VREQ                  # Version request
        self.TxCount = 1
        self.fnWrBuffer()
        time.sleep(1.000)                            # Wait 1000ms
        if self.hTDAU.inWaiting() == 0:              # No characters waiting
            return False
        for i in range(0,4,1):                       # Expect 4 characters MAX
            try:                                     # Empty the receive buffer to speed things up later
                Rx = self.hTDAU.read()               # Get character
                try:
                    RxChar = ord(Rx.decode())
                except:
                    RxChar = ord(Rx)
            except:
                break                                # Nothing in the buffer
            if bDebug:
                sString = str(self.fnHex2Asc((RxChar >> 4) & 0x0F))
                sString += str(self.fnHex2Asc(RxChar & 0x0F))
                sString += " "
                print(sString,end="")
            if self.hTDAU.inWaiting() == 0:
                break
        if bDebug:
            print
        return True  # !!!

# ---------- Convert hex nibble to ASCII character ----------
    def fnHex2Asc(self,Byte,Upper=False):
        """
        INTERNAL USE ONLY: Convert hex nibble to ASCII character
        Parameters: byte: Hex value (4 bits)
        Returns:    byte: ASCII character
        """
        if type(Byte) == int:
            if Byte < 10:                            # Numeral
                xx = chr(Byte + 0x30)                # Convert to ASCII
            else:
                if Upper:
                    xx = chr(Byte + 55)              # Convert A to F
                else:
                    xx = chr(Byte + 87)              # Convert a to f
        elif type(Byte) == str:
            zz = int(Byte)
            if zz < 10:
                xx = chr(zz + 0x30)
            else:
                if Upper:
                    xx = chr(xx + 55)
                else:
                    xx = chr(xx + 87)
        else:
            if ord(Byte) < 10:                       # Numeral
                xx = ord(Byte) + 0x30                # Convert to ASCII
            else:
                if Upper:
                    xx = ord(Byte) + 55              # Convert A to F
                else:
                    xx = ord(Byte) + 87              # Convert a to f
        return xx

# ---------- Write Buffer to TDAU ----------
    def fnWrBuffer(self):
        """
        INTERNAL USE ONLY: Write Buffer to TDAU
        Parameters: None
        Returns:    bool: True
        """
        bDebug = False
        sWrite = str(chr(SLAVE))
        for x in range(0,self.TxCount,1):
            TxChar = chr(self.TxBuffer[x])
            sWrite += str(TxChar)
        self.fnWrSerialPort(sWrite)
        if bDebug:
            print(sWrite)
        return True

# ---------- Write to Serial Port and delay ----------
    def fnWrSerialPort(self,sCommand):
        """
        Write to Serial Port and delay 50mS
        Parameters: string: raw string to send
        Returns:    bool: True if successful
                          False if unsuccessful
        """
        if type(sCommand) != str:
            return False
        iLen = len(sCommand)
        if iLen == 0:
            return True
        LsCommand = []
        for x in range(iLen):
            LsCommand.append(ord(sCommand[x:x+1]))
        self.hTDAU.write(LsCommand)
        time.sleep(0.050)
        return True

# ---------- Convert Float to Hex ----------
    def float_to_hex(self,f):
        """
        Convert Float to Hex
        Parameters: string/float: value to convert
        Returns:    int: Hex value
        """
        sValue = hex(struct.unpack('<I', struct.pack('<f', f))[0])
        return eval(sValue)

