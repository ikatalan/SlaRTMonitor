using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinqExample
{

    
    class RTDataGenerator
    {

        class Range
        {
            public Range(int _minVal, int _maxVal)
            {
                minVal = _minVal;
                maxVal = _maxVal;
            }

            public int minVal;
            public int maxVal;
        }

        class DeviceData
        {
            public DeviceData(int _id, string _type)
            {
                id = _id;
                type = _type;
            }
            public int id;
            public string type;
        }

        static int interval = 3000;//Three sec

        char[] trailingSpace = { ' ' };
        private List<DeviceData> devicesData = new List<DeviceData>();
        private Dictionary<int, Range> thresholds = new Dictionary<int, Range>();


        private Dictionary<string, List<int>> thresholdForDeviceType;
        
        
        private SqlConnection dbConnection;
        private SqlCommand insertSimulatedMeasurementCmd;

        private SqlConnection dbConnectionDevices;
        private SqlCommand allDevices;

        private SqlConnection dbConnectionThresholdTypes;
        private SqlCommand allThresholdTypes;

        private SqlConnection dbConnectionContracts;
        private SqlCommand allContracts;


        private System.Threading.Thread t1 = null;

        private bool shouldContinue;

        public RTDataGenerator()
        {
            shouldContinue = false;
        }

        public void Start()
        {
            t1 = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Generator));

            dbConnectionDevices = new SqlConnection(global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString);

            //Get All devices currently in the system
            allDevices = new SqlCommand("SELECT id, type FROM [dbo].[Devices]", dbConnectionDevices);

            if (((allDevices.Connection.State & global::System.Data.ConnectionState.Open) != global::System.Data.ConnectionState.Open))
            {
                allDevices.Connection.Open();
            }

            SqlDataReader devicesReader = allDevices.ExecuteReader();

            //Init device data vector with ID and TYPE
            devicesData = new List<DeviceData>();
            while (devicesReader.Read())
            {
                devicesData.Add(new DeviceData(devicesReader.GetInt32(0), devicesReader.GetString(1).ToLower()));
            }

            dbConnectionThresholdTypes = new SqlConnection(global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString);

            // fetch all threshold ids with min/max values
            allThresholdTypes = new SqlCommand("SELECT id, minValue, maxValue FROM [dbo].[Thresholds]", dbConnectionThresholdTypes);

            if (((allThresholdTypes.Connection.State & global::System.Data.ConnectionState.Open) != global::System.Data.ConnectionState.Open))
            {
                allThresholdTypes.Connection.Open();
            }

            SqlDataReader thresholdTypesReader = allThresholdTypes.ExecuteReader();

            thresholds = new Dictionary<int, Range>();
            while (thresholdTypesReader.Read())
            {
                thresholds[thresholdTypesReader.GetInt32(0)] = new Range(thresholdTypesReader.GetInt32(1), thresholdTypesReader.GetInt32(2));
            }

            dbConnectionContracts = new SqlConnection(global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString);

            allContracts = new SqlCommand("SELECT device_type, threshold_id FROM [dbo].[SLAContracts]", dbConnectionContracts);

            if (((allContracts.Connection.State & global::System.Data.ConnectionState.Open) != global::System.Data.ConnectionState.Open))
            {
                allContracts.Connection.Open();
            }

            SqlDataReader contractsReader = allContracts.ExecuteReader();

            thresholdForDeviceType = new Dictionary<string, List<int>>();
            while (contractsReader.Read())
            {
                string currDeviceType = contractsReader.GetString(0).ToLower().TrimEnd(trailingSpace);
                if (thresholdForDeviceType.ContainsKey(currDeviceType))
                {
                    if (!thresholdForDeviceType[currDeviceType].Contains(contractsReader.GetInt32(1)))
                    {
                        thresholdForDeviceType[currDeviceType].Add(contractsReader.GetInt32(1));
                    }
                }
                else
                {
                    thresholdForDeviceType.Add(currDeviceType, new List<int>(contractsReader.GetInt32(1)));
                }
            }


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
                    + @"([device_id] "
                    + @",[threshold_id] "
                    + @",[value] "
                    + @",[timestamp]) "
                    + @"VALUES (@device_id, @threshold_id, @value, @timestamp)",
                dbConnection);


            if (((insertSimulatedMeasurementCmd.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                insertSimulatedMeasurementCmd.Connection.Open();
            }

            insertSimulatedMeasurementCmd.CommandType = global::System.Data.CommandType.Text;
            insertSimulatedMeasurementCmd.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
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
                DeviceData theChosenDevice = devicesData[(int)(randGenerator.NextDouble() * (devicesData.Count()))]; 
                int theChosenThresholdId = GetRandomThresholdIdForDeviceType(randGenerator, theChosenDevice.type);

                if (theChosenThresholdId == -1)
                {
                    Debug.WriteLine("device: " + theChosenDevice.id + " threshold_id: " + theChosenThresholdId);
                    continue;
                }
                Range theChosenRange = thresholds[theChosenThresholdId];

                insertSimulatedMeasurementCmd.Parameters[0].Value = theChosenDevice.id;
                insertSimulatedMeasurementCmd.Parameters[1].Value = theChosenThresholdId;
                insertSimulatedMeasurementCmd.Parameters[2].Value = randGenerator.NextDouble() * (theChosenRange.maxVal - theChosenRange.minVal + 1) + theChosenRange.minVal;
                insertSimulatedMeasurementCmd.Parameters[3].Value = DateTime.Now;

                Debug.WriteLine("device: " + theChosenDevice.id + " threshold_id: " + theChosenThresholdId + " value: " + insertSimulatedMeasurementCmd.Parameters[2].Value + " time: " + insertSimulatedMeasurementCmd.Parameters[3].Value);

                insertSimulatedMeasurementCmd.ExecuteNonQuery();

                System.Threading.Thread.Sleep(interval);
            }
        }

        public int GetRandomThresholdIdForDeviceType(Random randGenerator, string deviceType) 
        {
            if (!thresholdForDeviceType.ContainsKey(deviceType))
            {
                return -1;
            }
            List<int> thresholdForDevice = thresholdForDeviceType[deviceType];
            if (thresholdForDevice.Count > 0)
            {
                return thresholdForDevice[(int)(randGenerator.NextDouble() * (thresholdForDevice.Count()))];
                
            }
            return -1;
        }

    }
}

	
	
		
	
	