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
        class ThresholdData
        {
            public ThresholdData(bool _isAbove, int _minVal, int _maxVal)
            {
                minVal = _minVal;
                maxVal = _maxVal;
                isAbove = _isAbove;
            }

            public int minVal;
            public int maxVal;
            public bool isAbove;
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

        class ContractData
        {
            public List<KeyValuePair<int,float>> listThresholdIds;
            public string deviceType;
        }

        static int interval = 20000;//Interval 

        char[] trailingSpace = { ' ' };
        private List<DeviceData> devicesData = new List<DeviceData>();
        private Dictionary<int, ThresholdData> thresholds = new Dictionary<int, ThresholdData>();

        private Dictionary<string, ContractData> thresholdForDeviceType;

        private SqlConnection dbConnection;//connections for insertSimulatedMeasurementCmd
        private SqlCommand insertSimulatedMeasurementCmd;//Insert the data created to table SimulatedMeasurement

        private SqlConnection dbConnectionDevices;//Connections for allDevices
        private SqlCommand allDevices;//Get All devices currently in the system

        private SqlConnection dbConnectionThresholdTypes;//Connections for allThresholdTypes
        private SqlCommand allThresholdTypes; // fetch all threshold ids with min/max values

        private SqlConnection dbConnectionContracts;
        private SqlCommand allContracts;//Read contract and get device_type, threshold_id, value

        private SqlConnection dbConnectionPastMeasurements;//Connections for pastMeasurements
        private SqlCommand pastMeasurements;//Get AVG value for a device


        Random randGenerator = new Random((int)DateTime.Now.Ticks);//create random seed based on ticks


        private System.Threading.Thread t1 = null;

        private bool shouldContinue;

        public RTDataGenerator()
        {
            shouldContinue = false;
        }

        public void Start()
        {
            //Init thread object.
            t1 = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Generator));

            {// Read all devices data.
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
                {                                   // id,                      //type
                    devicesData.Add(new DeviceData(devicesReader.GetInt32(0), devicesReader.GetString(1).ToLower()));
                }
            }

            { // Read all thresholds
                dbConnectionThresholdTypes = new SqlConnection(global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString);

                // fetch all threshold ids with min/max values
                allThresholdTypes = new SqlCommand("SELECT id, threshold_type_id, minValue, maxValue FROM [dbo].[Thresholds]", dbConnectionThresholdTypes);

                if (((allThresholdTypes.Connection.State & global::System.Data.ConnectionState.Open) != global::System.Data.ConnectionState.Open))
                {
                    allThresholdTypes.Connection.Open();
                }

                SqlDataReader thresholdTypesReader = allThresholdTypes.ExecuteReader();

                thresholds = new Dictionary<int, ThresholdData>();
                while (thresholdTypesReader.Read())
                {                                       //id,                                       //threshold_type_id,                        //minValue,                         //maxValue
                    thresholds[thresholdTypesReader.GetInt32(0)] = new ThresholdData(thresholdTypesReader.GetInt32(1) == 1, thresholdTypesReader.GetInt32(2), thresholdTypesReader.GetInt32(3));
                }
            }

            { // Read all contract and map contract to device type.
                dbConnectionContracts = new SqlConnection(global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString);

                allContracts = new SqlCommand("SELECT device_type, threshold_id, value FROM [dbo].[SLAContracts]", dbConnectionContracts);

                if (((allContracts.Connection.State & global::System.Data.ConnectionState.Open) != global::System.Data.ConnectionState.Open))
                {
                    allContracts.Connection.Open();
                }
               
               
               SqlDataReader contractsReader = allContracts.ExecuteReader();
                
               
                thresholdForDeviceType = new Dictionary<string, ContractData>();
                while (contractsReader.Read())
                {
                                            //device_type
                    string currDeviceType = contractsReader.GetString(0).ToLower().TrimEnd(trailingSpace);
                    if (thresholdForDeviceType.ContainsKey(currDeviceType))
                    {                                                                                       //threshold_id        
                        if (!thresholdForDeviceType[currDeviceType].listThresholdIds.Exists(x => x.Key == contractsReader.GetInt32(1)))
                        {
                            thresholdForDeviceType[currDeviceType].listThresholdIds.Add(
                                new KeyValuePair<int, float>(
                                    //threshold_id,                                     //value
                                    contractsReader.GetInt32(1), (float)contractsReader.GetInt32(2)));
                        }
                    }
                    else
                    {
                        ContractData newContractData = new ContractData();
                        newContractData.deviceType = currDeviceType;
                        newContractData.listThresholdIds = new List<KeyValuePair<int, float>>();
                        newContractData.listThresholdIds.Add(new KeyValuePair<int, float>(contractsReader.GetInt32(1),(float)contractsReader.GetInt32(2)));
                        
                        thresholdForDeviceType.Add(currDeviceType, newContractData);
                        
                    }
                }

            }

            {
                dbConnection = new global::System.Data.SqlClient.SqlConnection();
                dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Init insert into simulated measurements sql comand
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
                insertSimulatedMeasurementCmd.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@value", global::System.Data.SqlDbType.Float, 0, global::System.Data.ParameterDirection.Input, 0, 0, "value", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                insertSimulatedMeasurementCmd.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@timestamp", global::System.Data.SqlDbType.DateTime, 0, global::System.Data.ParameterDirection.Input, 0, 0, "timestamp", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));

                global::System.Data.ConnectionState previousConnectionState = insertSimulatedMeasurementCmd.Connection.State;
                if (((insertSimulatedMeasurementCmd.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    insertSimulatedMeasurementCmd.Connection.Open();
                }
            }

            {// Read past measurements of device & threshold pair.
                dbConnectionPastMeasurements = new SqlConnection(global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString);

                //Get All devices currently in the system
                pastMeasurements = new SqlCommand(
                    "SELECT AVG(a.value) " +
                    "FROM ( " +
	                "    SELECT TOP 2 timestamp, value FROM [SLA_RT_monitoring].[dbo].[SimulatedMeasurements] " +
	                "    WHERE device_id=@device_id " +
	                "    AND threshold_id=@threshold_id " +
	                "    AND timestamp>@timestamp " +
	                "    ORDER BY timestamp DESC " +
                    "    ) a ",
                    dbConnectionPastMeasurements);

                if (((pastMeasurements.Connection.State & global::System.Data.ConnectionState.Open) != global::System.Data.ConnectionState.Open))
                {
                    pastMeasurements.Connection.Open();
                }

                pastMeasurements.CommandType = global::System.Data.CommandType.Text;
                pastMeasurements.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                pastMeasurements.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                pastMeasurements.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@timestamp", global::System.Data.SqlDbType.DateTime, 0, global::System.Data.ParameterDirection.Input, 0, 0, "timestamp", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            }

            shouldContinue = true;
            t1.Start(this);
        }

        //Stop the generate data
        public void Stop()
        {
            shouldContinue = false;
            t1.Abort();
        }

        private int irregularPeak = 0;

        private void Generator (object arg) {
            RTDataGenerator me = (RTDataGenerator) arg;
            Random randGenerator = new Random();

            while (shouldContinue)
            {
                foreach (DeviceData currDevice in devicesData)
                {
                    DeviceData theChosenDevice = currDevice;

                    if (thresholdForDeviceType.ContainsKey(theChosenDevice.type))
                    {
                        ContractData currContract = thresholdForDeviceType[theChosenDevice.type];

                        foreach (KeyValuePair<int, float> thresholdWithContractValue in currContract.listThresholdIds)
                        {
                            int thresholdId = thresholdWithContractValue.Key;
                            ThresholdData theChosenRange = thresholds[thresholdId];

                            insertSimulatedMeasurementCmd.Parameters[0].Value = theChosenDevice.id;
                            insertSimulatedMeasurementCmd.Parameters[1].Value = thresholdId;
                            insertSimulatedMeasurementCmd.Parameters[2].Value = 
                                GetValueForDeviceThresholdPair(theChosenDevice, thresholdId, currContract, (irregularPeak%37) == 0);
                            insertSimulatedMeasurementCmd.Parameters[3].Value = DateTime.Now;

                            Debug.WriteLine("device: " + theChosenDevice.id + " threshold_id: " + thresholdId + " value: " + insertSimulatedMeasurementCmd.Parameters[2].Value + " time: " + insertSimulatedMeasurementCmd.Parameters[3].Value);

                            insertSimulatedMeasurementCmd.ExecuteNonQuery();

                            ++irregularPeak;
                        }
                    }
                }
                
                System.Threading.Thread.Sleep(interval);
            }
        }

        private object GetValueForDeviceThresholdPair(DeviceData deviceData, int thresholdId, ContractData contractData, bool generageIrregularData )
        {
            KeyValuePair<int, float> currContractValue = contractData.listThresholdIds.Find(x => x.Key == thresholdId);
            ThresholdData thresholdData = thresholds[thresholdId];

            float nextValue = 0;

            if (generageIrregularData)
            {
                if (!thresholdData.isAbove)
                {
                    nextValue = (float)randGenerator.Next((int)Math.Ceiling(currContractValue.Value), thresholdData.maxVal);
                }
                else
                {
                    nextValue = (float)randGenerator.Next(thresholdData.minVal, (int)Math.Floor(currContractValue.Value));
                }
            }
            else
            {

                pastMeasurements.Parameters["@device_id"].Value = deviceData.id;
                pastMeasurements.Parameters["@threshold_id"].Value = thresholdId;
                pastMeasurements.Parameters["@timestamp"].Value = DateTime.Now.Subtract(new TimeSpan(0, 0, (int)(((float)interval) / (1000) * 5)));



                double lastMeasurementsAverageValue = 0;
                if (!Double.TryParse(pastMeasurements.ExecuteScalar().ToString(), out lastMeasurementsAverageValue))
                {
                    if (thresholdData.isAbove)
                    {
                        nextValue = currContractValue.Value * 1.2f;
                    }
                    else
                    {
                        nextValue = currContractValue.Value * 0.8f;
                    }
                }
                else
                {
                    float min = (float)(lastMeasurementsAverageValue * 0.85);
                    float max = (float)(lastMeasurementsAverageValue * 1.15);

                    if (thresholdData.isAbove)
                    {
                        if (min < currContractValue.Value ||
                            max < currContractValue.Value)
                        {
                            max = thresholdData.maxVal;
                            min = currContractValue.Value; 
                        }
                    }
                    else
                    {
                        if (max > currContractValue.Value ||
                            min > currContractValue.Value)
                        {
                            max = currContractValue.Value;
                            min = thresholdData.minVal;
                        }
                    }


                    if (min < thresholdData.minVal)
                    {
                        min = thresholdData.minVal * 1.1f;
                    }


                    if (max > thresholdData.maxVal)
                    {
                        max = thresholdData.maxVal * 0.9f;
                    }


                    nextValue = (float)(randGenerator.NextDouble() * (max - min) + min);
                }
            }

            return Math.Round(nextValue, 2);
        }

    }
}

	
	
		
	
	