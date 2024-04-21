using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;




namespace ThermalBathGUI
{

    internal class DBController
    {
        public readonly string connectionString = "Data Source=ThermalBath.db;Version=3;";

        public void CreateDatabase()
        {

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Create the Test table
                //string createTestTableQuery = "CREATE TABLE IF NOT EXISTS Test (TestId INTEGER PRIMARY KEY, unitID INTEGER, Temperature REAL, Voltage REAL, curr_combination_id INTEGER)";
                string createTestTableQuery = "CREATE TABLE IF NOT EXISTS Test" +
                    "(TestId INTEGER PRIMARY KEY,Temperature REAL,project_id INTEGER, unit_id INTEGER, curr_combination_id INTEGER, Voltage REAL" +
                    ", Vbe1 REAL, Ib1 REAL, Vbe2 REAL, Ib2 REAL, Vbe3 REAL, Ib3 REAL, Ie1_measured REAL, Ie2_measured REAL, Ie3_measured REAL, Ic1 REAL, Ic2 REAL, Ic3 REAL," +
                    "Ie1_leak REAL, Ie2_leak REAL, Ie3_leak REAL, Ib1_leak REAL, Ib2_leak REAL, Ib3_leak REAL, RS REAL, Idea REAL);";

                using (SQLiteCommand command = new SQLiteCommand(createTestTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create the CurrentCombination table
                string createCurrentCombinationTableQuery = "CREATE TABLE IF NOT EXISTS Current_combination (id INTEGER PRIMARY KEY, Ie1 REAL, Ie2 REAL, Ie3 REAL);";
                using (SQLiteCommand command = new SQLiteCommand(createCurrentCombinationTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create the TDAU table
                string createTDAUTableQuery = "CREATE TABLE IF NOT EXISTS TDAU (id INTEGER PRIMARY KEY, com_port INTEGER, channel INTEGER);";
                using (SQLiteCommand command = new SQLiteCommand(createTDAUTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create the Measurements table
                string createMeasurementsTableQuery = "CREATE TABLE IF NOT EXISTS Project (id INTEGER PRIMARY KEY, project_name TEXT, step TEXT);";
                //string createMeasurementsTableQuery = "CREATE TABLE IF NOT EXISTS Measurements (Id INTEGER PRIMARY KEY, DateTimeStamp DATETIME, TestId INTEGER, Vbe1 REAL, Ib1 REAL, Vbe2 REAL, Ib2 REAL, Vbe3 REAL, Ib3 REAL, Ie1_measured REAL, Ie2_measured REAL, Ie3_measured REAL, Ic1 REAL, Ic2 REAL, Ic3 REAL, RS REAL, Idea REAL)";
                using (SQLiteCommand command = new SQLiteCommand(createMeasurementsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void InsertDataOfCurrentCombinationTable(List<double> ie1, List<double> ie2, List<double> ie3)
        {

            // Create your SQLite connection
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Define SQLite INSERT query
                string insertQuery = $"INSERT INTO Current_combination (Ie1, Ie2, Ie3) VALUES (@Value1, @Value2, @Value3);";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    // Iterate through the combinations of elements from the lists
                    foreach (double item1 in ie1)
                    {
                        foreach (double item2 in ie2)
                        {
                            foreach (double item3 in ie3)
                            {
                                // Set parameter values for the SQLite query
                                command.Parameters.Clear();
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

        public void InsertDataOfTestTable(TestController test)
        {
            int unitsNumber = numberOfUnits(test);
            List<double> tempertureList = GenerateValuesInRange(test.getTempLow(), test.getTempHigh(), test.getTempStep());
            List<int> CurrentCombinationList = RetrieveColumnValues("Current_combination", "id");



            // Create your SQLite connection
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Define SQLite INSERT query
                string insertQuery = $"INSERT INTO Test (unit_id, temperature, voltage, curr_combination_id) VALUES (@Value1, @Value2, @Value3, @Value4)";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    // Iterate through the combinations of elements from the lists
                    foreach (double temperture in tempertureList)
                    {
                        foreach (double current in CurrentCombinationList)
                        {
                            for (int i = 1; i <= unitsNumber; i++)
                            {
                                // Set parameter values for the SQLite query
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@Value1", i);
                                command.Parameters.AddWithValue("@Value2", temperture);
                                command.Parameters.AddWithValue("@Value3", test.getVcc());
                                command.Parameters.AddWithValue("@Value4", current);


                                // Execute the SQLite query to insert the combination into the table
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                connection.Close();
            }
        }

        public void InsertDataOfTDAUTable(TestController test)
        {
            int numOfUnits = numberOfUnits(test);

            int numberOfChannels = 4;


            // Create your SQLite connection
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Define SQLite INSERT query
                string insertQuery = $"INSERT INTO TDAU (com_port, channel) VALUES (@Value1, @Value2)";

                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    // Iterate through the combinations of elements from the lists
                    for (int i = 1; i <= numberOfChannels; i++)
                    {
                                // Set parameter values for the SQLite query
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@Value1", test.tdau1.getCom());
                                command.Parameters.AddWithValue("@Value2", i);

                                // Execute the SQLite query to insert the combination into the table
                                command.ExecuteNonQuery();
                    }

                    for (int i = 1; i <= numberOfChannels; i++)
                    {
                        // Set parameter values for the SQLite query
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Value1", test.tdau2.getCom());
                        command.Parameters.AddWithValue("@Value2", i);

                        // Execute the SQLite query to insert the combination into the table
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }



        public List<int> RetrieveColumnValues(string tableName, string columnName)
        {
            List<int> values = new List<int>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT {columnName} FROM {tableName}";

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

        public void updateIc()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Assuming you have a table named 'YourTable' and columns 'col2', 'col3', and 'col1'
                string updateQuery = "UPDATE Test SET Ic1 = (Test.Ie1_measured - Test.Ib1) , " +
                    "Ic2 = (Test.Ie2_measured - Test.Ib2) , " +
                    "Ic3 = (Test.Ie3_measured - Test.Ib3);";

                using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    // Execute the update query
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public void updateRs_Idea() { 
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Define SQLite UPDATE query
                //string query =    "UPDATE Test SET RS = ((Vbe3-Vbe2)-((Vbe2-Vbe1)*((ln(Ic3/Ic2))/(ln(Ic2/Ic1))))) / ((Ie3_measured-Ie2_measured)-((Ie2_measured-Ie1_measured)*((ln(Ic3/Ic2))/(ln(Ic2/Ic1)))));";
                ////"=((K3-I3)   - ((I3-G3)    *((LN(P3/Q3))  /(LN(Q3/P3)))))   / ((O3-N3)                    -((N3-M3)                    *((LN(R3 /Q3))  /(LN(Q3/P3 )))))"
                ///
                // Fetch rows from the Test table
                string query = "SELECT * FROM Test";
                //"((Vbe3-Vbe2)-((Vbe2-Vbe1)*((LOG(Ic3/Ic2))/(LOG(Ic2/Ic1))))) / ((Ie3_measured-Ie2_measured)-((Ie2_measured-Ie1_measured)*((LOG(Ic3/Ic2))/(LOG(Ic2/Ic1)))));"
                // Create a SQLite command
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

                            //double q = 1.6020E-19;
                            double q = 1.602176634E-19;
                            double k = 1.380649E-23;
                            double x = ((Vbe2 - Vbe1) - ((Ie2_measured - Ie1_measured) * RS));
                            double y = ic2ic1;
                            double z = 273.15;
                            double temp = Convert.ToDouble(reader["Temperature"]);

                            double idea = (q * x) / ((temp + z) * k * y);

                            // Update the RS column in the current row
                            updateRS(connection, Convert.ToInt32(reader["TestId"]), RS, idea);
                        }
                    }
                }
            }
        }

        static void updateRS(SQLiteConnection connection, int testId, double RS, double idea)
        {
            // Update the RS column in the Test table
            string updateQuery = "UPDATE Test SET RS = @RS ,Idea = @idea WHERE TestId = @testId";
            using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@idea", idea);
                updateCommand.Parameters.AddWithValue("@RS", RS);
                updateCommand.Parameters.AddWithValue("@testId", testId);

                updateCommand.ExecuteNonQuery();
            }
        }

        private int numberOfUnits(TestController test)
        {
            int units = 0;
            if (test.tdau1.isAvailable())
            {
                units = 4;
                if (test.tdau2.isAvailable())
                {
                    units += 4;
                }
            }
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



        public void createView()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string viewCreationQuery = @"CREATE VIEW IF NOT EXISTS matan AS
            SELECT
                TestId,
                Temperature,
                unit_id,
                curr_combination_id,
                current_combination.Ie1 AS Ie1_combination,
                current_combination.Ie2 AS Ie2_combination,
                current_combination.Ie3 AS Ie3_combination,
                Voltage,
                Vbe1,
                Ib1,
                Vbe2,
                Ib2,
                Vbe3,
                Ib3,
                Ie1_measured,
                Ie2_measured,
                Ie3_measured,
                Ic1,
                Ic2,
                Ic3,
                Ie1_leak,
                Ie2_leak,
                Ie3_leak,
                Ib1_leak,
                Ib2_leak,
                Ib3_leak,
                RS,
                Idea
            FROM
                Test
            JOIN
                current_combination ON Test.curr_combination_id = current_combination.id;";

                using (SQLiteCommand command = new SQLiteCommand(viewCreationQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }



        //public void ProcessRows()
        //{
        //    string connectionString = "Data Source=ThermalBath.db;Version=3;";

        //    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT * FROM Test"; // Replace YourTableName with the actual name of your table

        //        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        //        {
        //            using (SQLiteDataReader reader = command.ExecuteReader())
        //            {
        //                while (ReadNextRow(reader))
        //                {
        //                    // Process the retrieved data as needed
        //                }
        //            }
        //        }
        //    }
        //}

        //private bool ReadNextRow(SQLiteDataReader reader)
        //{
        //    if (reader.Read())
        //    {
        //        // Access the values in the current row using column indices or column names
        //        int id = reader.GetInt32(0); // Assuming the first column is an integer, adjust accordingly
        //        string column1Value = reader.GetString(reader.GetOrdinal("Column1")); // Replace "Column1" with the actual column name

        //        // Process the retrieved data as needed for this row
        //        Console.WriteLine($"ID: {id}, Column1: {column1Value}");

        //        return true; // Indicates that there is another row to read
        //    }

        //    return false; // Indicates that there are no more rows
        //}
    }
}