using Python.Runtime;
using System.Data.SQLite;
using System.Drawing.Text;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Channels;
using System.Threading;
using System.Windows.Forms;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace ThermalBathGUI
{
    public partial class Form1 : Form
    {
        public readonly string connectionString = "Data Source=ThermalBathNew.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        TestController test = new TestController();



        private void projName_KeyPress(object sender, KeyPressEventArgs e)
        {
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

        private void projStep_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevent the system sound
                String input = ((TextBox)sender).Text;
                if (!(input.Equals("")))
                {
                    test.setProjStep(input);
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
                vccGroup.Enabled = true;
                vcc.Enabled = false;
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
                        Console.WriteLine("The input is not a legal number:" + e);
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
                        Console.WriteLine("The input is not a legal number: " + ex);
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
                        Console.WriteLine("The input is not a legal number: " + ex);
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
                        Console.WriteLine("The input is not a legal number: " + e);
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
            DBController db = new DBController();
            test.setProjId(db.getNextProjId()-1);
            /*test.printTest();
            db.CreateDatabase();
            db.InsertDataOfProjectInfoTable(test.getProjId(), test.getProjName(), test.getProjStep(), test.getUser(), test.getEmail());
            db.InsertDataOfCurrentCombinationTable(test.getProjId(), test.getIe1List(), test.getIe2List(), test.getIe3List());
            db.InsertDataOfTDAUTable(test);
            db.InsertDataOfTestTable(test);

            runTest(test);

            db.updateIc(test.getProjId());
            db.updateRs_Idea(test.getProjId());*/
            db.createView(test.getProjId(),test.getProjName());

            test.disconnect();
            Console.WriteLine("test has been Done!!");
            return;

        }

        private void runTest(TestController test)
        {
            Bath bath = new Bath("COM3", 2400);
            bath.OpenConnection();
            Console.WriteLine(bath.getTemp());

            foreach (var tdau in test.tdauList)
            {
                tdau.writeCtrlWord(0);
            }



            int oldTemperture = 200;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query =   "SELECT Test_Id,Proj_id,Diode_Id,com_port,channel,Ie1,Ie2,Ie3 ,Temperature "
                               + "FROM Test "
                               + "NATURAL JOIN Current_combination "
                               + "NATURAL JOIN TDAU "
                               + "WHERE Test.Proj_Id = @Proj_Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Proj_Id", test.getProjId());

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Loop through all rows
                        while (writeEmiterCurrent(reader))
                        {
                            if (oldTemperture != Convert.ToInt32(reader["Temperature"]))
                            {
                                oldTemperture = Convert.ToInt32(reader["Temperature"]);
                                bath.setTemp(oldTemperture);
                                Thread.Sleep(300*1000);
                            }
                            insertMesurment2DB(test.getTdauByCom(Convert.ToInt32(reader["com_port"])), Convert.ToInt32(reader["Test_Id"]), test.getProjId(), Convert.ToInt32(reader["channel"]), connection);
                        }
                    }
                }
            }
            bath.setDefualtTemp();
            bath.CloseConnection();
        }


        private bool writeEmiterCurrent(SQLiteDataReader reader)
        {
            //int ch;
            if (!(reader.Read()))
                return false;

            // Access the value in the specified column for each row
            object ie1 = reader["Ie1"];
            object ie2 = reader["Ie2"];
            object ie3 = reader["Ie3"];
            object channel = reader["channel"];
            object com = reader["com_port"];
            object temp = reader["Temperature"];

            if (Convert.ToInt32(channel)==4) { return true;} ///need to be delete with row 388 only for unit with 3 didoe

            TDAU tdau;
            int unit_id = Convert.ToInt32(reader["Diode_Id"]);

            if (ie1 != DBNull.Value && ie2 != DBNull.Value && ie3 != DBNull.Value)
            {
                Console.WriteLine(Convert.ToInt32(reader["Test_Id"]) + " ," + Convert.ToSingle(ie1) + ", " + Convert.ToSingle(ie2) + ", " + Convert.ToSingle(ie3), ", " + Convert.ToSingle(temp));
                tdau = test.getTdauByCom(Convert.ToInt32(com));
                if (tdau != null)
                {
                    tdau.writeMemorey(68, Convert.ToInt32(channel), 1, Convert.ToSingle(ie1));
                    tdau.writeMemorey(68, Convert.ToInt32(channel), 2, Convert.ToSingle(ie2));
                    tdau.writeMemorey(68, Convert.ToInt32(channel), 3, Convert.ToSingle(ie3));
                }
            }
            return true; // Indicates that there is another row to read
        }

        private void insertMesurment2DB(TDAU tdau, int testId,int Proj_Id, int ch, SQLiteConnection connection)
        {
            if (ch == 4) { return; }        ///need to be delete with row 367 only for unit with 3 didoe
            //Console.WriteLine(testId);
            // Create your SQLite connection
            //using (SQLiteConnection connection = new SQLiteConnection(connectionString))

            //connection.Open();
            tdau.calibrate();


            // Define SQLite INSERT query
            string insertQuery = $" UPDATE Test SET " +
                    $" Vbe1=@val1, Ib1=@val2, Vbe2=@val3, Ib2=@val4, Vbe3=@val5, Ib3=@val6, " +
                    $" Ie1_measured=@val7, Ie2_measured=@val8, Ie3_measured=@val9, " +
                    $" Ie1_leak=@val10, Ie2_leak=@val11, Ie3_leak=@val12, " +
                    $" Ib1_leak=@val13, Ib2_leak=@val14, Ib3_leak=@val15 " +
                    $" WHERE Test_Id=@val AND Test.Proj_Id = @Proj_Id;";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    // Set parameter values for the SQLite query
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@val1", tdau.readMemorey(1090 + 26 * ch + 8 * 1));        //Vbe[n] = 1090+26*ch+8*I
                    command.Parameters.AddWithValue("@val5", tdau.readMemorey(1090 + 26 * ch + 8 * 3));        //Vbe[n]= 1090+26*ch+8*I
                    command.Parameters.AddWithValue("@val3", tdau.readMemorey(1090 + 26 * ch + 8 * 2));        //Vbe[n] = 1090+26*ch+8*I
                    command.Parameters.AddWithValue("@val2", tdau.readMemorey(1094 + 26 * ch + 8 * 1));        //Ib[n] = 1094+26*G30+8*H30
                    command.Parameters.AddWithValue("@val4", tdau.readMemorey(1094 + 26 * ch + 8 * 2));        //Ib[n] = 1094+26*ch+8*H30
                    command.Parameters.AddWithValue("@val6", tdau.readMemorey(1094 + 26 * ch + 8 * 3));        //Ib[n] = 1094+26*G30+8*H30
                    command.Parameters.AddWithValue("@val7", tdau.readMemorey(1036 + 12 * ch + 4 * 1));        //Ie[n]_meas=1036+12*ch+4*I
                    command.Parameters.AddWithValue("@val8", tdau.readMemorey(1036 + 12 * ch + 4 * 2));        //Ie[n]_meas=1036+12*ch+4*I
                    command.Parameters.AddWithValue("@val9", tdau.readMemorey(1036 + 12 * ch + 4 * 3));        //Ie[n]_meas=1036+12*ch+4*I
                    command.Parameters.AddWithValue("@val10", tdau.readMemorey(944 + 12 * ch + 4 * 1));        //Ie[n]_leak=944+12*ch+4*I
                    command.Parameters.AddWithValue("@val11", tdau.readMemorey(944 + 12 * ch + 4 * 2));        //Ie[n]_leak=944+12*ch+4*I
                    command.Parameters.AddWithValue("@val12", tdau.readMemorey(944 + 12 * ch + 4 * 3));        //Ie[n]_leak=944+12*ch+4*I
                    command.Parameters.AddWithValue("@val13", tdau.readMemorey(896 + 12 * ch + 4 * 1));        //Ib[n]_leak=896+12*ch+4*I
                    command.Parameters.AddWithValue("@val14", tdau.readMemorey(896 + 12 * ch + 4 * 2));        //Ib[n]_leak=896+12*ch+4*I
                    command.Parameters.AddWithValue("@val15", tdau.readMemorey(896 + 12 * ch + 4 * 3));        //Ib[n]_leak=896+12*ch+4*I
                    command.Parameters.AddWithValue("@val", testId);
                    command.Parameters.AddWithValue("@Proj_Id", Proj_Id);
                    // Execute the SQLite query to insert the combination into the table
                    command.ExecuteNonQuery();
                }
        }


        private void TDAU1ConnectBtn_Click(object sender, EventArgs e)
        {
            int[] usedPorts = new int[test.tdauList.Count()]; // Create array for used ports
            TDAU tdau;

            int index = 0; // Index to fill usedPorts array
            foreach (TDAU tdauTmp in test.tdauList)
            {
                usedPorts[index] = tdauTmp.getCom(); // Get the COM port from the TDAU object
                index++;
            }

            using (var addForm = new AddTDAUForm(test.tdauList.Count() + 1,usedPorts))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    // Get the values from the add form
                    int tdauValue = addForm.TDAUValue;
                    int portValue = addForm.PortValue;

                    try
                    {
                        tdau = new TDAU();
                        tdau.setCom(portValue);
                        tdau.connect();
                        test.addTdau(tdau);
                        // Add the values to the ListView
                        ListViewItem item = new ListViewItem(new[] { "" + tdauValue, "" + portValue, "" + tdau.getSerialNumber() });
                        listView1.Items.Add(item); // Add the new row to the ListView
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                }
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

        private void projName_Enter(object sender, EventArgs e)
        {
            projName.Text = "";
        }

        private void projName_Leave(object sender, EventArgs e)
        {
            if (projName.Text.Equals(""))
                if (test.getProjName() == String.Empty)
                    projName.Text = "Name";
                else projName.Text = test.getProjName();
            else projName.Text = "Name";
        }

        private void projStep_Enter(object sender, EventArgs e)
        {
            projStep.Text = "";
        }

        private void projStep_Leave(object sender, EventArgs e)
        {
            if (projStep.Text.Equals(""))
                if (test.getProjStep() == String.Empty)
                    projStep.Text = "Step";
                else projStep.Text = test.getProjStep();
            else projStep.Text = "Step";
        }
    }

}