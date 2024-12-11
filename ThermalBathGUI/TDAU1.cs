

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace ThermalBathGUI
{

    internal class TDAU1
    {
        // Serial communication constants
        private const byte SLAVE = 0x01;
        private const char CR = '\r';
        private const char LF = '\n';

        // Command constants
        private const byte CMD_EXTC = 0x0D;     // Extended calibrate command
        private const byte CMD_SCO = 0x20;      // Single current offset calib
        private const byte CMD_RDMEM = 0x07;    // Read memory map command
        private const byte CMD_WRMEM = 0x08;    // Write memory command 
        private const byte CMD_VREQ = 0x03;     // Firmware version request
        private const byte CMD_RDSER = 0x04;    // Read serial number
        private const byte CMD_RDR0 = 0x34;     // Read absolute address page 0
        private const byte CMD_RDR1 = 0x35;     // Read absolute address page 1
        private const byte CMD_RDF0 = 0x36;     // Read flash page 0
        private const byte CMD_RDF1 = 0x37;     // Read flash page 1

        // Response codes
        private const byte R_COND = 0x80;      // Conditional reply
        private const byte R_MEM = 0x87;       // Memory contents
        private const byte R_SER = 0x84;       // Serial number response

        // Reply conditions
        private const byte C_PASS = 0x41;     // Pass
        private const byte C_INVC = 0x42;     // Invalid command
        private const byte C_INAC = 0x43;     // Inactive command
        private const byte C_BADCS = 0x44;    // Bad checksum
        private const byte C_BUSY = 0x45;     // Busy
        private const byte C_ERR = 0x46;      // Error
        private const byte C_RANGE = 0x47;    // Value out of range
        private const byte C_NODATA = 0x48;   // No more data
        private const byte C_OVERF = 0x49;    // Receiver overflow
        private const byte C_BOOT = 0x4A;     // Boot code not found

        private SerialPort serialPort;
        private byte[] txBuffer = new byte[39];
        private int txCount = 0;
        private bool commEnabled = false;
        private bool exceptionEnableConnect = false;

        public bool fnConnect(int comPort)
        {
            string portName = $"COM{comPort}";
            commEnabled = true;  // Must be set to run fnCheckCommunication

            Console.Write($"Connecting to Thermal Diode Acq Unit... ");
            try
            {
                serialPort = new SerialPort(portName, 38400, Parity.None, 8, StopBits.One);
                serialPort.Open();

                if (fnCheckCommunication())
                {
                    Console.WriteLine($"Connected on port {portName}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Unable to communicate on port {portName}");
                    if (exceptionEnableConnect)
                        throw new Exception($"Unable to connect to TDAU on port {portName}");
                    commEnabled = false;
                    return false;
                }
            }
            catch
            {
                Console.WriteLine($"Unable to open port {portName}");
                if (exceptionEnableConnect)
                    throw new Exception($"Unable to open COM{comPort}");
                serialPort = null;
                commEnabled = false;
                return false;
            }
        }

        public bool fnDisconnect()
        {
            if (!commEnabled)
                return false;

            serialPort.Close();
            commEnabled = false;
            return true;
        }

        public string fnRdMemory(int address, int type = 4, bool printMode = false)
        {
            if (!commEnabled)
                return null;

            txBuffer[1] = (byte)(address & 0xFF);
            txBuffer[2] = (byte)((address >> 8) & 0xFF);
            txBuffer[3] = 16;  // Quantity of bytes to read (type 4 only)

            var memMap = new Dictionary<int, (byte Cmd, int CountRx, int TxCount, string Region)>
        {
            { 0, (CMD_RDR0, 17, 3, "SRAM pg0:") },
            { 1, (CMD_RDR1, 17, 3, "SRAM pg1") },
            { 2, (CMD_RDF0, 17, 3, "FLASH p0:") },
            { 3, (CMD_RDF1, 17, 3, "FLASH p1:") },
            { 4, (CMD_RDMEM, 18, 4, "USER RAM:") }
        };

            var (txCmd, countRx, count, region) = memMap[type];
            txBuffer[0] = txCmd;
            txCount = count;
            fnWrBuffer();

            byte[] rxChars = new byte[18];
            int rxCount = 0;
            Thread.Sleep(250);

            while (rxCount < 18 && serialPort.BytesToRead > 0)
            {
                rxChars[rxCount] = (byte)serialPort.ReadByte();
                rxCount++;
            }

            if (rxCount < 3)
            {
                Console.WriteLine("Insufficient reply from TDAU");
                return null;
            }

            if (rxChars[0] == R_COND)
                return ShowError(rxChars[1], printMode);

            if (rxCount < countRx)
            {
                Console.WriteLine("Insufficient reply from TDAU");
                return null;
            }

            if (rxChars[0] == R_MEM)
            {
                StringBuilder result = new StringBuilder();
                for (int x = 0; x < 15; x++)
                {
                    result.Append(fnHex2Asc((rxChars[x + 1] >> 4) & 0x0F, true));
                    result.Append(fnHex2Asc(rxChars[x + 1] & 0x0F, true));
                    result.Append(" ");
                }

                result.Append(fnHex2Asc((rxChars[16] >> 4) & 0x0F, true));
                result.Append(fnHex2Asc(rxChars[16] & 0x0F, true));

                if (printMode)
                    Console.WriteLine($"{region} {result}");

                return result.ToString();
            }

            return null;
        }

        public string fnRdSerialNumber(bool printMode = false)
        {
            if (!commEnabled)
                return null;

            txBuffer[0] = CMD_RDSER;
            txCount = 1;
            fnWrBuffer();
            Thread.Sleep(250);

            byte[] rxChars = new byte[6];
            int rxCount = 0;

            while (rxCount < 6 && serialPort.BytesToRead > 0)
            {
                rxChars[rxCount] = (byte)serialPort.ReadByte();
                rxCount++;
            }

            if (rxCount < 6)
            {
                Console.WriteLine("Insufficient reply from TDAU");
                return null;
            }

            if (((rxChars[0] + rxChars[1] + rxChars[2] + rxChars[3] + rxChars[4]) & 0xFF) != rxChars[5])
            {
                Console.WriteLine("Checksum error");
                return null;
            }

            uint serialNumber = ((uint)rxChars[4] << 24) |
                              ((uint)rxChars[3] << 16) |
                              ((uint)rxChars[2] << 8) |
                              rxChars[1];

            if (printMode)
                Console.WriteLine($"Serial Number: {serialNumber}");

            return serialNumber.ToString();
        }

        public string fnWrMemory(int address, byte data, bool printMode = false)
        {
            if (!commEnabled)
                return null;

            byte checksum = (byte)(1 + data); // 1 is the quantity
            txBuffer[3] = 1;  // quantity

            byte tempL = (byte)(address & 0xFF);
            checksum += tempL;
            txBuffer[1] = tempL;

            byte tempH = (byte)((address >> 8) & 0xFF);
            checksum += tempH;
            txBuffer[2] = tempH;

            checksum += CMD_WRMEM;
            txBuffer[0] = CMD_WRMEM;
            txBuffer[4] = data;
            txBuffer[5] = checksum;
            txCount = 6;

            fnWrBuffer();
            Thread.Sleep(250);
            return fnRdReply(printMode);
        }

        private bool fnCheckCommunication()
        {
            serialPort.ReadTimeout = 5000;
            txBuffer[0] = CMD_VREQ;
            txCount = 1;
            fnWrBuffer();
            Thread.Sleep(1000);

            if (serialPort.BytesToRead == 0)
                return false;

            // Clear receive buffer
            for (int i = 0; i < 4; i++)
            {
                if (serialPort.BytesToRead == 0)
                    break;
                serialPort.ReadByte();
            }

            return true;
        }

        private bool fnWrBuffer()
        {
            if (!commEnabled)
                return false;

            byte[] writeData = new byte[txCount + 1];
            writeData[0] = SLAVE;
            Array.Copy(txBuffer, 0, writeData, 1, txCount);
            serialPort.Write(writeData, 0, writeData.Length);
            Thread.Sleep(50);
            return true;
        }

        private string ShowError(byte code, bool printMode)
        {
            var errorList = new Dictionary<byte, string>
        {
            { C_PASS, "PASS" },
            { C_INVC, "INVALID COMMAND" },
            { C_INAC, "INACTIVE COMMAND" },
            { C_BADCS, "BAD CHECKSUM" },
            { C_BUSY, "BUSY" },
            { C_ERR, "ERROR" },
            { C_RANGE, "RANGE" },
            { C_NODATA, "END OF DATA" },
            { C_OVERF, "Rx BUFFER FULL" },
            { C_BOOT, "NO BOOT LOADER" }
        };

            string error = errorList.ContainsKey(code) ? errorList[code] : "UNKNOWN";

            if (printMode)
                Console.WriteLine($"TDAU reply: {error}");

            return error;
        }

        private char fnHex2Asc(int value, bool upper = false)
        {
            if (value < 10)
                return (char)(value + '0');
            else
                return (char)(value + (upper ? 'A' - 10 : 'a' - 10));
        }

        private string fnRdReply(bool printMode = false)
        {
            if (!commEnabled)
                return null;

            byte[] rxChars = new byte[255];
            int count = 0;
            Thread.Sleep(250);

            while (count < 255 && serialPort.BytesToRead > 0)
            {
                rxChars[count] = (byte)serialPort.ReadByte();
                count++;
            }

            if (count < 3)
            {
                Console.WriteLine("Insufficient reply from TDAU");
                return null;
            }

            if (rxChars[0] == R_COND)
                return ShowError(rxChars[1], printMode);

            if (printMode)
                Console.WriteLine($"TDAU reply: {BitConverter.ToString(rxChars, 0, count)}");

            return BitConverter.ToString(rxChars, 0, count);
        }
        public float fnRdFloat(int address, bool printMode = false)
        {
            if (!commEnabled)
                return float.NaN;

            txBuffer[0] = CMD_RDMEM;
            txBuffer[1] = (byte)(address & 0xFF);
            txBuffer[2] = (byte)((address >> 8) & 0xFF);
            txBuffer[3] = 4;  // Quantity of bytes to read
            txCount = 4;
            fnWrBuffer();

            byte[] rxChars = new byte[18];
            int count = 0;
            Thread.Sleep(250);

            while (count < 18 && serialPort.BytesToRead > 0)
            {
                rxChars[count] = (byte)serialPort.ReadByte();
                count++;
            }

            if (count < 3)
            {
                Console.WriteLine("Insufficient reply from TDAU");
                return float.NaN;
            }

            if (rxChars[0] == R_COND)
            {
                ShowError(rxChars[1], printMode);
                return float.NaN;
            }

            if (rxChars[0] != R_MEM)
            {
                Console.WriteLine("Incorrect reply from TDAU");
                return float.NaN;
            }

            if (count < 6)
            {
                Console.WriteLine("Insufficient reply from TDAU");
                return float.NaN;
            }

            // Calculate checksum
            int calcCs = 0;
            for (int x = 0; x < 5; x++)
            {
                calcCs += rxChars[x];
            }

            if ((calcCs & 0xFF) != rxChars[5])
            {
                Console.WriteLine("Checksum error");
                return float.NaN;
            }

            // Get 4 bytes in correct order and build hex string
            string[] hexData = new string[4];
            for (int x = 0; x < 4; x++)
            {
                hexData[x] = rxChars[x + 1].ToString("X2");
            }

            // Combine hex values in correct order
            string hexValue = hexData[3] + hexData[2] + hexData[1] + hexData[0];

            // Convert hex string to integer then to float using bit pattern
            uint intValue = uint.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            float value = BitConverter.ToSingle(BitConverter.GetBytes(intValue), 0);

            if (printMode)
            {
                Console.WriteLine($"{value:G9}");
            }

            return value;
        }
        public string fnExtendedCalibration(bool printMode = false)
        {
            if (!commEnabled)
                return null;

            txBuffer[0] = CMD_EXTC;
            txCount = 1;
            fnWrBuffer();
            Thread.Sleep(250);
            return fnRdReply(printMode);
        }

        public string fnSCOCalibration(bool printMode = false)
        {
            if (!commEnabled)
                return null;

            txBuffer[0] = CMD_SCO;
            txCount = 1;
            fnWrBuffer();
            Thread.Sleep(250);
            return fnRdReply(printMode);
        }
    }
}
