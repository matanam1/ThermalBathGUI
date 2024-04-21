using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;

namespace ThermalBathGUI
{
    //  Word1   Word2   Word3   Word4
    //  11 11   00 38   01 00   33 33 // 3-Curr No Equ / 3-Curr ifact
    //  99 99   00 38   01 00   BB BB // 3-Cur, No Equ, Lkg / 3-Cur Ifact, Lkg 
    //  99 99   F0 38   01 00   BB BB // 3-Cur, No Equ, Lkg / 3-Cur Ifact, Lkg / Ib leakage Comp


internal class TDAU
    {

        bool connectStatus = false;
        bool available = false;
        int com;
        dynamic TDAU_Module;
        dynamic TDAU_class;
        dynamic python;
        dynamic result;


        readonly int[,] CTRL_WORD = new int[,]
            {
            {0x11, 0x11, 0x00, 0x38, 0x01, 0x00, 0x33, 0x33},
            {0x99, 0x99, 0x00, 0x38, 0x01, 0x00, 0xBB, 0xBB},
            {0x99, 0x99, 0xF0, 0x38, 0x01, 0x00, 0xBB, 0xBB}
            };

        public TDAU() {
            // Initialize the Python engine
            PythonEngine.Initialize();

            // Load the Python script
            TDAU_Module = PythonEngine.ModuleFromString("TDAU", File.ReadAllText("C:\\Users\\lab_gigaev01\\source\\repos\\ThermalBathGUI\\TDAU_c.py"));

            // Access the TDAU class from the Python module
            TDAU_class = TDAU_Module.TDAU();

        }
        
        public bool getCnnectStatus() { return connectStatus; }

        public bool isAvailable() { return available; }
        public int getCom() { return com; }
        public void setCom(int com) {  
            this.com = com;
            this.available = true;
        }



        public void connect()
        {
            // Call the fnConnect method with the appropriate argument
            Console.WriteLine(TDAU_class.fnConnect(com));
            //TDAU_class.fnSaveToFile();
            Console.WriteLine(TDAU_class.fnRdMemory(0x54, 4));

            String input = TDAU_class.fnRdMemory(0x54, 4);
            string result = new string(input.Replace(" ", "").Take(8).ToArray());
            float num = HexToFloat(result);

            Console.WriteLine(input + "\n" + result);
            Console.WriteLine(num);
            Console.WriteLine(FloatToHex(num));

            connectStatus = true;
        }

        //get the config of the contrll word to write to TDAU
        public void writeCtrlWord(int config)
        {
            for (int i = 0; i < CTRL_WORD.GetLength(1); i++)
            {
                TDAU_class.fnWrMemory(i, CTRL_WORD[config, i]); // Write RAM
            }
            Console.WriteLine(TDAU_class.fnRdMemory(00, 4));
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
                    int address = baseAddress + (12 * channel) + (4 * curr) +i;
                    TDAU_class.fnWrMemory(address, byteArray[i]);
                }
            }
        }

        public float readMemorey(int address)
        {
            //int address = baseAddress + (12 * channel) + (4 * curr);
            float val = TDAU_class.fnRdFloat(address);
            return val;
        }

        public void calibrate()
        {
            TDAU_class.fnExtendedCalibration();
            TDAU_class.fnSCOCalibration();
        }

        public void disconnect()
        {
            Console.WriteLine(TDAU_class.fnDisconnect());
            connectStatus = false;
        }

        public void disconnectPy()
        {
            PythonEngine.Shutdown();
        }
    }
}
