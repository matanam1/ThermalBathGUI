using System;               //add for console print
using System.Windows.Forms; //add for console print

namespace ThermalBathGUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG                                                                            //add for console print
            // Redirect standard output to a console window in debug mode            //add for console print
            AllocConsole();                                                          //add for console print
            Console.WriteLine("Debug Console: Your debug output goes here.");        //add for console print
#endif                                                                               //add for console print
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]//add for console print
        private static extern bool AllocConsole(); //add for console print
    }
}