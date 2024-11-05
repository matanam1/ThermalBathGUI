using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ThermalBathGUI
{
    internal class Bath
    {
        private SerialPort serialPort;

        // Constructor to initialize the serial port with port name and baud rate
        public Bath(string portName, int baudRate)
        {
            serialPort = new SerialPort
            {
                PortName = portName,
                BaudRate = baudRate,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReadTimeout = 2000,   // 2 seconds timeout for read operation
                WriteTimeout = 2000   // 2 seconds timeout for write operation
            };
        }

        // Method to open the serial port connection
        public void OpenConnection()
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                    Console.WriteLine("Connection to bath opened.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening connection: " + ex.Message);
            }
        }

        // Method to close the serial port connection
        public void CloseConnection()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine("Connection to bath closed.");
            }
        }

        // Method to send a command to set the temperature on the bath
        public void setTemp(double temperature)
        {
            if (serialPort.IsOpen)
            {
                string command = $"s={temperature}\r"; // Adjust the command format as per the device manual
                serialPort.Write(command);
                Console.WriteLine($"Temperature set to: {temperature}°C");
                while (Math.Abs(temperature - getTemp()) > 0.2) {
                    Thread.Sleep(10000);
                }
            }
            else
            {
                Console.WriteLine("Serial port is not open.");
            }
        }

        // Method to get the current temperature from the bath (if needed)
        public float getTemp()
        {
            float temp = 0;
            if (serialPort.IsOpen)
            {
                string command = "t\r"; // Example command to get temperature, adjust as needed
                serialPort.Write(command);

                string response = serialPort.ReadLine();
                temp = tofloat(response);
                Console.WriteLine("Current temperature: " + temp);
                return temp;
            }
            else
            {
                Console.WriteLine("Connection is not open.");
                return -255;
            }
        }

        public void setDefualtTemp()
        {
            if (serialPort.IsOpen)
            {
                string command = $"s=25\r"; // Adjust the command format as per the device manual
                serialPort.Write(command);
                Console.WriteLine($"Temperature set to: 25°C");
            }
        }

        private float tofloat(string temp)
        {
            string pattern = @"[-+]?\d*\.\d+"; // Regular expression to match a float number
            float temperature = 0;

            // Extract the float number using Regex
            Match match = Regex.Match(temp, pattern);

            if (match.Success)
            {
                temperature = float.Parse(match.Value);
                Console.WriteLine("Extracted temperature: " + temperature);
            }
            return temperature;
        }

        // Method to check if the connection is open
        public bool IsConnected()
        {
            return serialPort.IsOpen;
        }

    }
}