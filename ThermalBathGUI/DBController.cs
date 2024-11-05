using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using static System.Net.Mime.MediaTypeNames;




namespace ThermalBathGUI
{

    internal class DBController
    {
        public readonly string connectionString = "Data Source=ThermalBathNew.db;Version=3;";

        public void CreateDatabase()
        {

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Create the Measurement table
                string createTableQuery = "CREATE TABLE IF NOT EXISTS Test (" +
                    "Test_Id INTEGER, Proj_Id INTEGER,  Diode_Id INTEGER, Current_Com INTEGER, Temperature REAL, Voltage REAL, " +
                    "Vbe1 REAL, Ib1 REAL, Vbe2 REAL, Ib2 REAL, Vbe3 REAL, Ib3 REAL, " +
                    "Ie1_measured REAL, Ie2_measured REAL, Ie3_measured REAL, " +
                    "Ic1 REAL, Ic2 REAL, Ic3 REAL, " +
                    "Ie1_leak REAL, Ie2_leak REAL, Ie3_leak REAL, " +
                    "Ib1_leak REAL, Ib2_leak REAL, Ib3_leak REAL, RS REAL, Idea REAL, " +
                    "PRIMARY KEY(Test_Id, Proj_id), " +
                    "FOREIGN KEY(Proj_Id) REFERENCES Project_Info);";

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create the Current_Combination table
                createTableQuery = "CREATE TABLE IF NOT EXISTS Current_Combination (" +
                    "Current_Com INTEGER , Proj_id INTEGER, Ie1 REAL, Ie2 REAL, Ie3 REAL," +
                    "PRIMARY KEY(Current_Com, Proj_Id)," +
                    "FOREIGN KEY(Proj_Id) REFERENCES Project_Info);";

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create the TDAU table
                createTableQuery = "CREATE TABLE IF NOT EXISTS TDAU (" +
                    "Diode_Id INTEGER, Proj_id INTEGER, com_port INTEGER, channel INTEGER, SN INTEGER," +
                    "PRIMARY KEY(Diode_Id, Proj_Id)," +
                    "FOREIGN KEY(Proj_Id) REFERENCES Project_Info);";

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create the Project_Info table
                createTableQuery = "CREATE TABLE IF NOT EXISTS Project_Info (" +
                    "Proj_Id INTEGER, Date_Time date, Name TEXT, Step TEXT, User_Name TEXT, User_Email TEXT," +
                    "PRIMARY KEY(Proj_Id));";

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void InsertDataOfProjectInfoTable(int projId, String projName, String projStep, String userName, String userEmail)
        {
            // Create your SQLite connection
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Define the SQLite INSERT query
                string insertQuery = "INSERT INTO Project_Info (Proj_Id, Date_Time, Name, Step, User_Name, User_Email) " +
                                     "VALUES (@ProjId, @DateTime, @Name, @Step, @UserName, @UserEmail);";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    // Set parameter values for the SQLite query
                    command.Parameters.AddWithValue("@ProjId", projId);
                    command.Parameters.AddWithValue("@DateTime", DateTime.Now);  // Assuming you want to use the current date and time
                    command.Parameters.AddWithValue("@Name", projName);
                    command.Parameters.AddWithValue("@Step", projStep);
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@UserEmail", userEmail);

                    // Execute the SQLite query to insert the data into the table
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void InsertDataOfCurrentCombinationTable(int projId, List<double> ie1, List<double> ie2, List<double> ie3)
        {

            // Create your SQLite connection
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                int i = 0;
                connection.Open();

                // Define SQLite INSERT query
                string insertQuery = $"INSERT INTO Current_combination (Current_Com, Proj_id, Ie1, Ie2, Ie3) VALUES (@CurrentCom, @ProjId, @Value1, @Value2, @Value3);";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    // Iterate through the combinations of elements from the lists
                    foreach (double item1 in ie1){
                        foreach (double item2 in ie2){
                            foreach (double item3 in ie3){
                                i++;
                                // Set parameter values for the SQLite query
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@CurrentCom", i);
                                command.Parameters.AddWithValue("@ProjId", projId);
                                command.Parameters.AddWithValue("@Value1", item1);
                                command.Parameters.AddWithValue("@Value2", item2);
                                command.Parameters.AddWithValue("@Value3", item3);

                                // Execute the SQLite query to insert the combination into the table
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                connection.Close();
            }
        }

        public void InsertDataOfTDAUTable()
        {

        }

        public void InsertDataOfTestTable(TestController test)
        {
            int unitsNumber = numberOfUnits(test);
            List<double> tempertureList = GenerateValuesInRange(test.getTempLow(), test.getTempHigh(), test.getTempStep());
            List<int> CurrentCombinationList = RetrieveColumnValues(test.getProjId(),"Current_combination", "Current_Com");
            int j = 0;
            // Get the diode IDs from the TDAU table
            List<int> diodeIds = GetDiodeIdsFromTDAU(test.getProjId());
            double vcc = test.getVcc();

            // Create your SQLite connection
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Define SQLite INSERT query
                string insertQuery = $"INSERT INTO Test (Test_Id, Proj_Id, Diode_Id, Current_Com, Temperature, Voltage) VALUES (@Test, @Proj, @Diode, @Current, @Temp, @Volt)";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    // Iterate through the combinations of elements from the lists
                    foreach (double temperture in tempertureList)
                    {
                        foreach (double current in CurrentCombinationList)
                        {
                            for (int i = 0; i < diodeIds.Count; i++)
                            {
                                j++;
                                // Set parameter values for the SQLite query
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@Test", j);
                                command.Parameters.AddWithValue("@Proj", test.getProjId());
                                command.Parameters.AddWithValue("@Diode", diodeIds[i]);
                                command.Parameters.AddWithValue("@Current", current);
                                command.Parameters.AddWithValue("@Temp", temperture);
                                command.Parameters.AddWithValue("@Volt", vcc);

                                // Execute the SQLite query to insert the combination into the table
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                connection.Close();
            }
        }

        public int getNextProjId()
        {
            int maxProjId = -1;

            // Create your SQLite connection
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Define the SQL query to get the max Proj_Id
                string query = "SELECT MAX(Proj_Id) FROM Project_Info";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    try
                    {
                        // Execute the command and get the result
                        object result = command.ExecuteScalar();


                        // If result is not null, assign it to maxProjId
                        if (result != DBNull.Value)
                        {
                            maxProjId = Convert.ToInt32(result);
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        // Print that the table does not exist or some other SQLite-related issue occurred
                        Console.WriteLine("Table 'Project_Info' does not exist or there was an issue with the query.");
                        Console.WriteLine("Error details: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        // Handle other potential exceptions
                        Console.WriteLine("An unexpected error occurred: " + ex.Message);
                    }
                }

                connection.Close();
            }

            return maxProjId+1;
        }

        

        // Method to get the diode IDs from the TDAU table
        private List<int> GetDiodeIdsFromTDAU(int projId)
        {
            List<int> diodeIds = new List<int>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Query the TDAU table for the specific project to get the Diode_Id values
                string selectQuery = "SELECT Diode_Id FROM TDAU WHERE Proj_Id = @Proj_Id";

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Proj_Id", projId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Add the Diode_Id to the list
                            diodeIds.Add(reader.GetInt32(0)); // Assumes Diode_Id is the first column
                        }
                    }
                }

                connection.Close();
            }

            return diodeIds;
        }

        public void InsertDataOfTDAUTable(TestController test)
        {
            int numOfUnits = numberOfUnits(test);
            int numberOfChannels = 4;
            int j = 0;

            // Create your SQLite connection
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Define SQLite INSERT query
                string insertQuery = $"INSERT INTO TDAU (Diode_Id, Proj_Id, com_port, channel, SN) VALUES (@Diode_Id, @Proj_Id , @com, @ch, @sn)";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    foreach (var tdau in test.tdauList)
                    {
                        // Iterate through the combinations of elements from the lists
                        for (int i = 1; i <= numberOfChannels; i++)
                        {
                            j++;
                            // Set parameter values for the SQLite query
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@Diode_Id", j);
                            command.Parameters.AddWithValue("@Proj_Id", test.getProjId());
                            command.Parameters.AddWithValue("@com", tdau.getCom());
                            command.Parameters.AddWithValue("@ch", i);
                            command.Parameters.AddWithValue("@sn", tdau.getSerialNumber());
                            // Execute the SQLite query to insert the combination into the table
                            command.ExecuteNonQuery();
                        }
                    }
                    connection.Close();
                }
            }
        }



        public List<int> RetrieveColumnValues(int projId, string tableName, string columnName)
        {
            List<int> values = new List<int>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT {columnName} FROM {tableName} WHERE Proj_Id = {projId}";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Access the value in the specified column for each row
                            object columnValue = reader[columnName];

                            if (columnValue != DBNull.Value)
                            {
                                // Process the value here, for example, print it to the console
                                values.Add(int.Parse(columnValue.ToString()));
                            }
                        }
                    }
                }
                connection.Close();
            }
            return values;
        }

        public void updateIc(int Proj_Id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Test " +
                    "SET Ic1 = (Test.Ie1_measured - Test.Ib1) , " +
                    "Ic2 = (Test.Ie2_measured - Test.Ib2) , " +
                    "Ic3 = (Test.Ie3_measured - Test.Ib3)" +
                    "WHERE Proj_id = @Proj_Id;";

                using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Proj_Id", Proj_Id);

                    // Execute the update query
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public void updateRs_Idea(int Proj_Id) { 
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Define SQLite UPDATE query
                //string query =    "UPDATE Test SET RS = ((Vbe3-Vbe2)-((Vbe2-Vbe1)*((ln(Ic3/Ic2))/(ln(Ic2/Ic1))))) / ((Ie3_measured-Ie2_measured)-((Ie2_measured-Ie1_measured)*((ln(Ic3/Ic2))/(ln(Ic2/Ic1)))));";
                ////"=((K3-I3)   - ((I3-G3)    *((LN(P3/Q3))  /(LN(Q3/P3)))))   / ((O3-N3)                    -((N3-M3)                    *((LN(R3 /Q3))  /(LN(Q3/P3 )))))"
                ///
                // Fetch rows from the Test table
                string query = "SELECT Test_Id,Proj_Id,Vbe1,Vbe2,Vbe3, Ie1_measured,Ie2_measured,Ie3_measured,Ic1,Ic2,Ic3,Temperature,RS,Idea " +
                    "FROM Test " +
                    "WHERE Proj_id = @Proj_Id;";
                //"((Vbe3-Vbe2)-((Vbe2-Vbe1)*((LOG(Ic3/Ic2))/(LOG(Ic2/Ic1))))) / ((Ie3_measured-Ie2_measured)-((Ie2_measured-Ie1_measured)*((LOG(Ic3/Ic2))/(LOG(Ic2/Ic1)))));"
                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Proj_Id", Proj_Id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                // Retrieve values from the current row
                                double Vbe1 = Convert.ToDouble(reader["Vbe1"]);
                                double Vbe2 = Convert.ToDouble(reader["Vbe2"]);
                                double Vbe3 = Convert.ToDouble(reader["Vbe3"]);
                                double Ie1_measured = Convert.ToDouble(reader["Ie1_measured"]);
                                double Ie2_measured = Convert.ToDouble(reader["Ie2_measured"]);
                                double Ie3_measured = Convert.ToDouble(reader["Ie3_measured"]);
                                double Ic1 = Convert.ToDouble(reader["Ic1"]);
                                double Ic2 = Convert.ToDouble(reader["Ic2"]);
                                double Ic3 = Convert.ToDouble(reader["Ic3"]);
                                double ic3ic2 = Math.Log(Ic3 / Ic2);
                                double ic2ic1 = Math.Log(Ic2 / Ic1);
                                // Perform the calculation
                                double RSup = (Vbe3 - Vbe2) - ((Vbe2 - Vbe1) * (ic3ic2 / ic2ic1));
                                double RSdown = (Ie3_measured - Ie2_measured) - ((Ie2_measured - Ie1_measured) * (ic3ic2 / ic2ic1));
                                double RS = RSup / RSdown;
                                double q = 1.602176634E-19;
                                double k = 1.380649E-23;
                                double x = ((Vbe2 - Vbe1) - ((Ie2_measured - Ie1_measured) * RS));
                                double y = ic2ic1;
                                double z = 273.15;
                                double temp = Convert.ToDouble(reader["Temperature"]);
                                double idea = (q * x) / ((temp + z) * k * y);
                                // Update the RS column in the current row
                                updateRS(connection, Proj_Id, Convert.ToInt32(reader["Test_Id"]), RS, idea);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
        }
        static void updateRS(SQLiteConnection connection, int Proj_Id, int Test_Id, double RS, double idea)
        {
            // Update the RS column in the Test table
            string updateQuery = "UPDATE Test " +
                "SET RS = @RS, Idea = @Idea " +   // Changed to match the parameter name
                "WHERE Proj_Id = @Proj_Id AND Test_Id = @Test_Id ";

            using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@Idea", idea); // Parameter name matches query
                updateCommand.Parameters.AddWithValue("@RS", RS);
                updateCommand.Parameters.AddWithValue("@Proj_Id", Proj_Id);
                updateCommand.Parameters.AddWithValue("@Test_Id", Test_Id);

                updateCommand.ExecuteNonQuery();
            }
        }

        private int numberOfUnits(TestController test)
        {
            int units = test.tdauList.Count();
            return units;
        }

        public List<double> GenerateValuesInRange(double low, double high, double step)
        {
            List<double> values = new List<double>();
            if (low == high) {
                values.Add(low);
                values.Add(low);
                return values;
            }
            for (double value = low; value <= high; value += step)
            {
                values.Add(value);
            }
            return values;
        }



        public void createView(int Proj_Id, String name)
        {
            String viewName = $"{name}_{DateTime.Now:yyyyMMdd_HHmm}";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string viewCreationQuery = $@"CREATE VIEW {viewName} AS
                    SELECT Test_Id,Proj_id,SN,com_port as Unit,channel,Diode_Id,Current_Com,Temperature,Voltage,
                    Ie1,Ie2,Ie3,Vbe1,Ib1,Vbe2,Ib2,Vbe3,Ib3,Ie1_measured,Ie2_measured,Ie3_measured,
                    Ic1,Ic2,Ic3,Ie1_leak,Ie2_leak,Ie3_leak,Ib1_leak,Ib2_leak,Ib3_leak,RS,Idea
                    FROM Test NATURAL JOIN Current_combination NATURAL JOIN TDAU WHERE Test.Proj_Id={Proj_Id};";

                using (SQLiteCommand command = new SQLiteCommand(viewCreationQuery, connection))
                {
                    //command.Parameters.AddWithValue("@Proj_Id", Proj_Id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}