using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


namespace ThermalBathGUI
{
    internal class COMPorts
    {
        public int[] GetComPorts()
        {
            // Get a string array of available COM ports
            string[] comPorts = SerialPort.GetPortNames();

            // Convert the string array to an int array
            int[] comPortNumbers = new int[comPorts.Length];

            for (int i = 0; i < comPorts.Length; i++)
            {
                try
                {
                    // Extract the COM port number from the string (e.g., "COM3" -> 3)
                    comPortNumbers[i] = int.Parse(comPorts[i].Substring(3));
                }
                catch (FormatException)
                {
                    // Handle parsing errors if any
                    comPortNumbers[i] = -1; // Indicates an error
                }
            }

            return comPortNumbers;
        }

    }
}
