using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using NationalInstruments.NI4882;
using NationalInstruments.VisaNS;
using Python.Runtime;

namespace ThermalBathGUI
{
    internal class BathController
    {
        dynamic bath_Module;
        dynamic setTempFunction;
        dynamic getTemperatureFunction;
        dynamic convertToFloatFunction;
        dynamic setTempWithoutSync;


        public BathController()
        {
            // Initialize the Python engine
            PythonEngine.Initialize();

            // Load the Python script
            bath_Module = PythonEngine.ModuleFromString("BATH", File.ReadAllText("C:\\Users\\lab_gigaev01\\source\\repos\\ThermalBathGUI\\ThermalBath.py"));

            // Access the Python functions from the module
            setTempFunction = bath_Module.set_temp;
            getTemperatureFunction = bath_Module.get_temperture;
            convertToFloatFunction = bath_Module.conver2Float;
            setTempWithoutSync = bath_Module.set_temp_without_sync;

        }

        public void setTemp(int temp=25)
        {
            // Call the set_temp function
            setTempFunction(temp, 60);  // Example values

            // Call the get_temperture function
            dynamic temperature = getTemperatureFunction();
            Console.WriteLine($"Temperature from Python: {temperature}");
        }

        public void setDefualtTemp()
        {
            setTempWithoutSync(25);
        }

        public void disconnectPy()
        {

            // Shut down the Python engine
            PythonEngine.Shutdown();
        }

    }
}
 
