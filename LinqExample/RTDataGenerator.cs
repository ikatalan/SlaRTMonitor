using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinqExample
{
    class RTDataGenerator
    {
        
        static String[] deviceNames = new String[] { "Printer 3", "Printer 11", "Printer 21" };
        static String[] deviceTypes = new String[] { "Printer" };
        static int minValue = 0;
        static int maxValue = 200;
        static int[] thresholdTypes = new int[] { 1, 2, 3 };
        static int interval = 500;

        
        private SqlConnection dbConnection;
        private SqlCommand insertSimulatedMeasurementCmd;
        private System.Threading.Thread t1 = null;

        private bool shouldContinue;

        public RTDataGenerator()
        {
            shouldContinue = false;
        }

        public void Start()
        {
            t1 = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Generator));

            shouldContinue = true;
            t1.Start(this);
        }

        public void Stop()
        {
            shouldContinue = false;
            t1.Abort();
        }

        private void Generator (object arg) {
            
            RTDataGenerator me = (RTDataGenerator) arg;

            Random randGenerator = new Random();

            dbConnection = new global::System.Data.SqlClient.SqlConnection();
            dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            // Used for filling list of items (device_name) per threshold_id            
            insertSimulatedMeasurementCmd = new SqlCommand(
                @"INSERT INTO [SLA_RT_monitoring].[dbo].[SimulatedMeasurements] "
                    + @"([device_name] "
                    + @",[device_type] "
                    + @",[threshold_id] "
                    + @",[value] "
                    + @",[timestamp]) "
                    + @"VALUES (@device_name, @device_type, @threshold_id, @value, @timestamp)",
                dbConnection);


            if (((insertSimulatedMeasurementCmd.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                insertSimulatedMeasurementCmd.Connection.Open();
            }

            insertSimulatedMeasurementCmd.CommandType = global::System.Data.CommandType.Text;
            insertSimulatedMeasurementCmd.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_name", global::System.Data.SqlDbType.NChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_name", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            insertSimulatedMeasurementCmd.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_type", global::System.Data.SqlDbType.NChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_type", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            insertSimulatedMeasurementCmd.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            insertSimulatedMeasurementCmd.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@value", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "value", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            insertSimulatedMeasurementCmd.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@timestamp", global::System.Data.SqlDbType.DateTime, 0, global::System.Data.ParameterDirection.Input, 0, 0, "timestamp", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));

            global::System.Data.ConnectionState previousConnectionState = insertSimulatedMeasurementCmd.Connection.State;
            if (((insertSimulatedMeasurementCmd.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                insertSimulatedMeasurementCmd.Connection.Open();
            }

            while (shouldContinue)
            {
                // SEMI-DIFICULT PART

                insertSimulatedMeasurementCmd.Parameters[0].Value = deviceNames[(int)(randGenerator.NextDouble()* (deviceNames.Count()))];
                insertSimulatedMeasurementCmd.Parameters[1].Value = deviceTypes[(int)(randGenerator.NextDouble() * (deviceTypes.Count()))];
                insertSimulatedMeasurementCmd.Parameters[2].Value = thresholdTypes[(int)(randGenerator.NextDouble() * (thresholdTypes.Count()))];
                insertSimulatedMeasurementCmd.Parameters[3].Value = randGenerator.NextDouble() * (maxValue - minValue + 1) + minValue;
                insertSimulatedMeasurementCmd.Parameters[4].Value = DateTime.Now;

                insertSimulatedMeasurementCmd.ExecuteNonQuery();

                System.Threading.Thread.Sleep(interval);

            }



        }

    }
}
