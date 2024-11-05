using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThermalBathGUI
{
    public partial class AddTDAUForm : Form
    {
        public int TDAUValue { get; private set; }
        public int PortValue { get; private set; }

        public int[] usedPorts { get; private set; }

        public AddTDAUForm(int TDAUValue, int[] usedPorts)
        {
            InitializeComponent();

            // Store the TDAUValue in a local variable
            this.TDAUValue = TDAUValue;

            // Use TDAUValue (for example, set it in a label or textbox)
            tdauNumber.Text = ""+TDAUValue;

            this.usedPorts = usedPorts;
        }

        private void mouseClickPort(object sender, MouseEventArgs e)
        { 
            ((ComboBox)sender).Items.Clear();
            COMPorts ports = new COMPorts();
            int[] portsArr = ports.GetComPorts();

            foreach (int port in portsArr)
            {
                // Check if the current port is not in the usedPorts array
                if (Array.IndexOf(usedPorts, port) == -1) // -1 means the port is not in the usedPorts array
                {
                    ((ComboBox)sender).Items.Add(port);
                }
            }
            ((ComboBox)sender).MaxDropDownItems = portsArr.Length;
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            TDAUValue = int.Parse(tdauNumber.Text);
            PortValue = int.Parse(portComboBox.Text);

            // Close the form
            this.DialogResult = DialogResult.OK; // Sets the result to OK
            this.Close();
        }
    }
}
