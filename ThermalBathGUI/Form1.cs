using System.Data.SQLite;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Channels;
using System.Windows.Forms;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Net.Mime.MediaTypeNames;

namespace ThermalBathGUI
{
    public partial class Form1 : Form
    {
        public readonly string connectionString = "Data Source=ThermalBath.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        TestController test = new TestController();
        BathController bath = new BathController();


        private void projName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //bath.setTemp(26);
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevent the system sound
                String input = ((TextBox)sender).Text;
                if (!(input.Equals("")))
                {
                    test.setProjName(input);
                    ((TextBox)sender).BackColor = Color.PaleGreen;
                    Console.WriteLine(input);
                }
            }
            else
            {
                ((TextBox)sender).BackColor = Color.MistyRose;
            }
        }

        private void vccEnDis_CheckedChanged(object sender, EventArgs e)
        {
            vccGroup.Enabled = (((CheckBox)sender).Checked);
            if (vccGroup.Enabled)
            {
                vcc.Enabled = true;
                //vcc.BackColor = Color.LemonChiffon;
            }
            else
            {
                vcc.BackColor = SystemColors.Window;
            }
        }

        private void vcc_KeyPress(object sender, KeyPressEventArgs k)
        {
            if (k.KeyChar == (char)Keys.Enter)
            {
                k.Handled = true; // Prevent the system sound
                String input = ((TextBox)sender).Text;
                ((TextBox)sender).Clear();
                if (!(input.Equals("")))
                {
                    try
                    {
                        test.setVcc(double.Parse(input));
                        vccText.Text = "" + test.getVcc() + "V";
                        vccText.BackColor = Color.PaleGreen;
                        ((TextBox)sender).BackColor = SystemColors.Window;
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("The input is not a legal number.");
                        ShowTemporaryLabel("The input is not a legal number.", 3000);
                    }
                }

            }
        }

        private void ShowTemporaryLabel(string text, int durationMilliseconds)
        {
            labelTemporary.Text = text;
            labelTemporary.Visible = true;

            timerTemporary.Interval = durationMilliseconds;
            timerTemporary.Start();
        }

        private void timerTemporary_Tick(object sender, EventArgs e)
        {
            labelTemporary.Visible = false;
            timerTemporary.Stop();
        }


        private void Ie1_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleInput((TextBox)sender, Ie1List, value => test.addVarIe1List(value), e, "ie1");
        }

        private void Ie2_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleInput((TextBox)sender, Ie2List, value => test.addVarIe2List(value), e, "ie2");
        }

        private void Ie3_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleInput((TextBox)sender, Ie3List, value => test.addVarIe3List(value), e, "ie3");
        }

        private void HandleInput(TextBox inputField, ListBox resultList, Action<double> listAdder, KeyPressEventArgs e, string ieType)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevent the system sound
                string input = inputField.Text;
                if (!string.IsNullOrEmpty(input))
                {
                    try
                    {
                        listAdder(double.Parse(input) * 1e-6);
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine("The input is not a legal number.");
                    }
                }
                inputField.Clear();
                inputField.BackColor = Color.LemonChiffon;
                resultList.Items.Clear();
                resultList.BackColor = Color.PaleGreen;

                foreach (var item in test.getIeList(ieType))
                {
                    resultList.Items.Add($"{item:0.0e+0}");
                }
            }
            else
            {
                inputField.BackColor = Color.MistyRose;
            }
        }

        private void Ie1CleanBtn_Click(object sender, EventArgs e)
        {
            Ie1.Clear();
            Ie1.BackColor = Color.LemonChiffon;
            Ie1List.Items.Clear();
            Ie1List.BackColor = Color.LemonChiffon;
            test.clearIe1List();
        }

        private void Ie2CleanBtn_Click(object sender, EventArgs e)
        {
            Ie2.Clear();
            Ie2.BackColor = Color.LemonChiffon;
            Ie2List.Items.Clear();
            Ie2List.BackColor = Color.LemonChiffon;
            test.clearIe2List();
        }

        private void Ie3CleanBtn_Click(object sender, EventArgs e)
        {
            Ie3.Clear();
            Ie3.BackColor = Color.LemonChiffon;
            Ie3List.Items.Clear();
            Ie3List.BackColor = Color.LemonChiffon;
            test.clearIe3List();
        }



        private void lowTemp_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleTemperatureInput((TextBox)sender, lowTempText, test.setTempLow, e);
        }

        private void highTemp_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleTemperatureInput((TextBox)sender, highTempText, test.setTempHigh, e);
        }

        private void stepTemp_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleTemperatureInput((TextBox)sender, tempStepText, test.setTempStep, e);
        }

        private void HandleTemperatureInput(TextBox textArea, Label resultText, Action<double> setter, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevent the system sound
                string input = textArea.Text;
                double number = 0;

                if (!string.IsNullOrEmpty(input))
                {
                    try
                    {
                        number = double.Parse(input);
                        setter(number);
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine("The input is not a legal number.");
                    }
                }
                textArea.Clear();
                resultText.Text = number + "°C";
                resultText.BackColor = Color.PaleGreen;
                textArea.BackColor = Color.White;
            }
            else
            {
                textArea.BackColor = Color.MistyRose;
            }
        }

        private void email_KeyPress(object sender, KeyPressEventArgs k)
        {
            if (k.KeyChar == (char)Keys.Enter)
            {
                k.Handled = true; // Prevent the system sound
                String input = ((TextBox)sender).Text;
                if (!(input.Equals("")))
                {
                    try
                    {
                        test.setUser(input);
                        ((TextBox)sender).BackColor = Color.PaleGreen;
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("The input is not a legal number.");
                    }
                }
            }
            else
            {
                ((TextBox)sender).BackColor = Color.MistyRose;
            }
        }


        private void startTestBtn_Click(object sender, EventArgs e)
        {
            test.printTest();
            DBController db = new DBController();

            //db.updateIc();
            //db.updateRs_Idea();
            //db.createView();
            //return;


            db.CreateDatabase();
            db.InsertDataOfCurrentCombinationTable(test.getIe1List(), test.getIe2List(), test.getIe3List());
            db.InsertDataOfTDAUTable(test);
            db.InsertDataOfTestTable(test);

            runTest(test);


            db.updateIc();
            db.updateRs_Idea();
            db.createView();

            test.tdau1.disconnect();
            test.tdau2.disconnect();
            test.tdau1.disconnectPy();
            test.tdau2.disconnectPy();
            Console.WriteLine("test has been Done!!");
        }

        private void runTest(TestController test)
        {
            test.tdau1.writeCtrlWord(0);
            test.tdau2.writeCtrlWord(0);
            int oldTemperture = 200;
            object newTemperture;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * " +
                    "FROM Test " +
                    "JOIN Current_combination ON curr_combination_id = Current_combination.id " +
                    "JOIN TDAU ON unit_id = TDAU.id;";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Loop through all rows
                        while (writeEmiterCurrentTo4Channel(reader))
                        {
                            if (oldTemperture != Convert.ToInt32(reader["Temperature"]))
                            {
                                oldTemperture = Convert.ToInt32(reader["Temperature"]);
                                bath.setTemp(oldTemperture);
                            }
                            test.tdau1.calibrate();
                            test.tdau2.calibrate();

                            insertMesurment2DB(test, Convert.ToInt32(reader["TestId"]) - 3, connection);
                        }
                    }
                }
            }
            bath.setDefualtTemp();
        }

        private void insertMesurment2DB(TestController test, int testId, SQLiteConnection connection)
        {
            Console.WriteLine(testId);
            // Create your SQLite connection
            //using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                //connection.Open();

                // Define SQLite INSERT query
                string insertQuery = $" UPDATE Test SET " +
                    $" Vbe1=@val1, Ib1=@val2, Vbe2=@val3, Ib2=@val4, Vbe3=@val5, Ib3=@val6, " +
                    $" Ie1_measured=@val7, Ie2_measured=@val8, Ie3_measured=@val9, " +
                    $" Ie1_leak=@val10, Ie2_leak=@val11, Ie3_leak=@val12, " +
                    $" Ib1_leak=@val13, Ib2_leak=@val14, Ib3_leak=@val15 " +
                    $" WHERE TestId=@val;";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    for (int ch = 1; ch < 5; ch++)
                    {
                        // Set parameter values for the SQLite query
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@val1", test.tdau1.readMemorey(1090 + 26 * ch + 8 * 1));        //Vbe[n] = 1090+26*ch+8*I
                        command.Parameters.AddWithValue("@val2", test.tdau1.readMemorey(1094 + 26 * ch + 8 * 1));        //Ib[n] = 1094+26*G30+8*H30
                        command.Parameters.AddWithValue("@val3", test.tdau1.readMemorey(1090 + 26 * ch + 8 * 2));        //Vbe[n] = 1090+26*ch+8*I
                        command.Parameters.AddWithValue("@val4", test.tdau1.readMemorey(1094 + 26 * ch + 8 * 2));        //Ib[n] = 1094+26*ch+8*H30
                        command.Parameters.AddWithValue("@val5", test.tdau1.readMemorey(1090 + 26 * ch + 8 * 3));        //Vbe[n ]= 1090+26*ch+8*I
                        command.Parameters.AddWithValue("@val6", test.tdau1.readMemorey(1094 + 26 * ch + 8 * 3));        //Ib[n] = 1094+26*G30+8*H30
                        command.Parameters.AddWithValue("@val7", test.tdau1.readMemorey(1036 + 12 * ch + 4 * 1));        //Ie[n]_meas=1036+12*ch+4*I
                        command.Parameters.AddWithValue("@val8", test.tdau1.readMemorey(1036 + 12 * ch + 4 * 2));        //Ie[n]_meas=1036+12*ch+4*I
                        command.Parameters.AddWithValue("@val9", test.tdau1.readMemorey(1036 + 12 * ch + 4 * 3));        //Ie[n]_meas=1036+12*ch+4*I
                        command.Parameters.AddWithValue("@val10", test.tdau1.readMemorey(944 + 12 * ch + 4 * 1));        //Ie[n]_leak=944+12*ch+4*I
                        command.Parameters.AddWithValue("@val11", test.tdau1.readMemorey(944 + 12 * ch + 4 * 2));        //Ie[n]_leak=944+12*ch+4*I
                        command.Parameters.AddWithValue("@val12", test.tdau1.readMemorey(944 + 12 * ch + 4 * 3));        //Ie[n]_leak=944+12*ch+4*I
                        command.Parameters.AddWithValue("@val13", test.tdau1.readMemorey(896 + 12 * ch + 4 * 1));        //Ib[n]_leak=896+12*ch+4*I
                        command.Parameters.AddWithValue("@val14", test.tdau1.readMemorey(896 + 12 * ch + 4 * 2));        //Ib[n]_leak=896+12*ch+4*I
                        command.Parameters.AddWithValue("@val15", test.tdau1.readMemorey(896 + 12 * ch + 4 * 3));        //Ib[n]_leak=896+12*ch+4*I


                        command.Parameters.AddWithValue("@val", testId);
                        testId++;


                        // Execute the SQLite query to insert the combination into the table
                        command.ExecuteNonQuery();
                    }
                }
            }

        }


        private bool writeEmiterCurrentTo4Channel(SQLiteDataReader reader)
        {
            int ch;
            for (int i = 0; i < 4; i++)
            {
                if (!(reader.Read()))
                    return false;
                // Access the value in the specified column for each row
                object ie1 = reader["Ie1"];
                object ie2 = reader["Ie2"];
                object ie3 = reader["Ie3"];
                object channel = reader["channel"];
                int unit_id = Convert.ToInt32(reader["unit_id"]);
                ch = Convert.ToInt32(channel);

                if (ie1 != DBNull.Value && ie2 != DBNull.Value && ie3 != DBNull.Value)
                {
                    Console.WriteLine(Convert.ToInt32(reader["TestId"]) + " ," + Convert.ToSingle(ie1) + ", " + Convert.ToSingle(ie2) + ", " + Convert.ToSingle(ie3));
                    if (unit_id <= 4)
                    {
                        test.tdau1.writeMemorey(68, ch, 1, Convert.ToSingle(ie1));  //=68+12*ch+4*I
                        test.tdau1.writeMemorey(68, ch, 2, Convert.ToSingle(ie2));
                        test.tdau1.writeMemorey(68, ch, 3, Convert.ToSingle(ie3));
                    }
                    else
                    {
                        test.tdau2.writeMemorey(68, ch, 1, Convert.ToSingle(ie1));  //=68+12*ch+4*I
                        test.tdau2.writeMemorey(68, ch, 2, Convert.ToSingle(ie2));
                        test.tdau2.writeMemorey(68, ch, 3, Convert.ToSingle(ie3));
                    }
                }
            }
            return true; // Indicates that there is another row to read
        }

        private void TDAU1_MouseClick(object sender, MouseEventArgs e)
        {
            ((ComboBox)sender).Items.Clear();
            COMPorts ports = new COMPorts();
            int[] portsArr = ports.GetComPorts();
            String temp;
            foreach (int port in portsArr)
            {
                temp = TDAU2.Text;
                if (!(String.IsNullOrEmpty(TDAU2.Text)))
                {
                    if (!(port == int.Parse(TDAU2.Text)))
                    {
                        ((ComboBox)sender).Items.Add(port);
                    }
                }
                else
                {
                    ((ComboBox)sender).Items.Add(port);
                }
            }
            ((ComboBox)sender).BackColor = Color.MistyRose;
        }

        private void TDAU2_MouseClick(object sender, MouseEventArgs e)
        {
            ((ComboBox)sender).Items.Clear();
            COMPorts ports = new COMPorts();
            int[] portsArr = ports.GetComPorts();
            foreach (int port in portsArr)
            {
                if (!(String.IsNullOrEmpty(TDAU1.Text)))
                    if (!(port == int.Parse(TDAU1.Text)))
                    {
                        ((ComboBox)sender).Items.Add(port);
                    }
            }
            ((ComboBox)sender).BackColor = Color.MistyRose;
        }

        private void TDAU1ConnectBtn_Click(object sender, EventArgs e)
        {
            COMPorts ports = new COMPorts();
            int[] portsArr = ports.GetComPorts();

            if (String.IsNullOrEmpty(TDAU1.Text))
            {
                Console.WriteLine("Please select COM device first!");
                TDAU1.BackColor = Color.MistyRose;
            }
            else
            {
                TDAU1.BackColor = Color.PaleGreen;
                test.setCom1(int.Parse(TDAU1.Text));
                test.tdau1.connect();
                if ((portsArr.Length > 1))
                {
                    TDAU2.Enabled = true;
                    TDAU2ConnectBtn.Enabled = true;
                }
            }
        }

        private void TDAU2ConnectBtn_Click(object sender, EventArgs e)
        {
            COMPorts ports = new COMPorts();
            int[] portsArr = ports.GetComPorts();

            if (String.IsNullOrEmpty(TDAU2.Text))
            {
                Console.WriteLine("Please select COM device first!");
                TDAU2.BackColor = Color.MistyRose;

            }
            else
            {
                TDAU2.BackColor = Color.PaleGreen;
                test.setCom2(int.Parse(TDAU2.Text));
                test.tdau2.connect();
            }
        }

        private void vccGroup_Enter(object sender, EventArgs e)
        {

        }



        private void vcc_Enter(object sender, EventArgs e)
        {
            vcc.Text = "";
        }


        private void vcc_Leave(object sender, EventArgs e)
        {
            vcc.Text = "Vcc";

        }

        private void Ie1_Enter(object sender, EventArgs e)
        {
            Ie1.Text = "";
        }

        private void Ie1_Leave(object sender, EventArgs e)
        {
            Ie1.Text = "Ie1(uA)";
        }

        private void Ie2_Enter(object sender, EventArgs e)
        {
            Ie2.Text = "";
        }

        private void Ie2_Leave(object sender, EventArgs e)
        {
            Ie2.Text = "Ie2(uA)";
        }

        private void Ie3_Enter(object sender, EventArgs e)
        {
            Ie3.Text = "";
        }

        private void Ie3_Leave(object sender, EventArgs e)
        {
            Ie3.Text = "Ie3(uA)";
        }

        private void lowTemp_Enter(object sender, EventArgs e)
        {
            lowTemp.Text = "";
        }

        private void lowTemp_Leave(object sender, EventArgs e)
        {
            lowTemp.Text = "°C";
        }

        private void highTemp_Enter(object sender, EventArgs e)
        {
            highTemp.Text = "";
        }

        private void highTemp_Leave(object sender, EventArgs e)
        {
            highTemp.Text = "°C";
        }
        //°
    }

}