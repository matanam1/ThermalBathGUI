using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThermalBathGUI
{
    internal class TDAU1Controller
    {
        //  Word1   Word2   Word3   Word4
        //  11 11   00 38   01 00   33 33 // 3-Curr No Equ / 3-Curr ifact
        //  99 99   00 38   01 00   BB BB // 3-Cur, No Equ, Lkg / 3-Cur Ifact, Lkg 
        //  99 99   F0 38   01 00   BB BB // 3-Cur, No Equ, Lkg / 3-Cur Ifact, Lkg / Ib leakage Comp

        bool connectStatus = false;
        bool available = false;
        int com;
        int serialNumber;
        int Dut;
        TDAU1 tdau1;

        readonly byte[,] CTRL_WORD = new byte[,] {
            {0x11, 0x11, 0x00, 0x38, 0x01, 0x00, 0x33, 0x33},
            {0x99, 0x99, 0x00, 0x38, 0x01, 0x00, 0xBB, 0xBB},
            {0x99, 0x99, 0xF0, 0x38, 0x01, 0x00, 0xBB, 0xBB}};

        public TDAU1Controller()
        {
            tdau1 = new TDAU1();
        }

        public TDAU1Controller(int Dut)
        {
            this.Dut = Dut;
            tdau1 = new TDAU1();
        }

        public bool getCnnectStatus() { return connectStatus; }
        public int getDut() { return Dut; }
        public void setDut(int Dut) {  this.Dut = Dut; }
        public bool isAvailable() { return available; }
        public int getCom() { return com; }
        public void setCom(int com)
        {
            this.com = com;
            this.available = true;
        }
        public int getSerialNumber() { return serialNumber; }
        public void connect()
        {
            Console.WriteLine(tdau1.fnConnect(com));

            String input = tdau1.fnRdMemory(0x180, 4);
            this.serialNumber = Int32.Parse(tdau1.fnRdSerialNumber());
            connectStatus = true;
        }

        //get the config of the contrll word to write to TDAU
        public void writeCtrlWord(int config)
        {
            for (int i = 0; i < CTRL_WORD.GetLength(1); i++)
            {
                tdau1.fnWrMemory(i, CTRL_WORD[config, i]); // Write RAM
            }
            Console.WriteLine(tdau1.fnRdMemory(00, 4));
        }

        public static float HexToFloat(string hexValue)
        {
            if (hexValue.Length != 8)
            {
                throw new ArgumentException("Hex value must be 8 characters long.");
            }

            uint intValue = Convert.ToUInt32(hexValue, 16);
            byte[] bytes = BitConverter.GetBytes(intValue);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            float floatValue = BitConverter.ToSingle(bytes, 0);

            return floatValue;
        }

        public static string FloatToHex(float floatValue)
        {
            byte[] bytes = BitConverter.GetBytes(floatValue);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            uint intValue = BitConverter.ToUInt32(bytes, 0);
            string hexValue = intValue.ToString("X8");

            return hexValue;
        }

        public void writeMemorey(int baseAddress, int channel, int curr, float data)
        {
            String hexString = FloatToHex(data);
            // Check if the length is even
            if (hexString.Length % 2 == 0)
            {
                byte[] byteArray = new byte[hexString.Length / 2];

                // Convert each pair of characters to a byte
                for (int i = 0; i < byteArray.Length; i++)
                {
                    string hexPair = hexString.Substring(i * 2, 2);
                    byteArray[i] = Convert.ToByte(hexPair, 16);
                    int address = baseAddress + (12 * channel) + (4 * curr) + i;
                    tdau1.fnWrMemory(address, byteArray[i]);
                }
            }
        }

        public float readMemorey(int address)
        {
            //int address = baseAddress + (12 * channel) + (4 * curr);
            float val = tdau1.fnRdFloat(address);
            return val;
        }

        public void calibrate()
        {
            tdau1.fnExtendedCalibration();
            tdau1.fnSCOCalibration();
        }

        public void disconnect()
        {
            Console.WriteLine(tdau1.fnDisconnect());
        }





    }
}
