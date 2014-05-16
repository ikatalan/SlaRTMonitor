using LinqExample.SLA_RT_monitoringDataSetThresholdTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinqExample
{
    public partial class SimulatedDataLoader : Form
    {

        Dictionary<int, String> deviceIdToName;
        SqlConnection devicesConnection;
        SqlCommand allDevices;

        public SimulatedDataLoader()
        {
            InitializeComponent();
        }
        //Load new file with simulated data
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\Users\\%USERNAME%";//Go to Desktop
            //   openFileDialog1.Filter = "Excel Files(.csv)|*.csv|Excel Files(.xls)|*.xls|Excel Files(.xlsx)|*.xlsx";
            openFileDialog1.Filter = "Excel Files(.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Read the csv file 
                    DataTable simulatedDataTable = GetDataTableFromCsv(openFileDialog1.FileName, true);

                    LinqExample.SLA_RT_monitoringDataSetMeasurementsTableAdapters.SimulatedMeasurementsTableAdapter adapter1
                        = new LinqExample.SLA_RT_monitoringDataSetMeasurementsTableAdapters.SimulatedMeasurementsTableAdapter();

                    BindingSource SBind = new BindingSource();
                    SBind.DataSource = simulatedDataTable;

                    gridSimulatedData.AutoGenerateColumns = false;
                    gridSimulatedData.DataSource = simulatedDataTable;

                    for (int i = 0; i < gridSimulatedData.ColumnCount; ++i)
                    {
                        gridSimulatedData.Columns[i].DataPropertyName = simulatedDataTable.Columns[i].Caption;
                    }

                    gridSimulatedData.DataSource = SBind;
                    gridSimulatedData.Refresh();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }


        DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            string header = isFirstRowHeader ? "Yes" : "No";

            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            string sql = @"SELECT * FROM [" + fileName + "]";

            
            SqlDataReader thresholdReader = this.thresholdsTableAdapter.Adapter.SelectCommand.ExecuteReader();

            Dictionary<int, string> typeValues = new Dictionary<int, string>();
            while (thresholdReader.Read())
            {
                int typeId = thresholdReader.GetInt32(0);
                string typeName = thresholdReader.GetString(2);

                typeValues[typeId] = typeName;
            }

            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                      ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {

                SqlDataAdapter measurmentsAdapter= new SqlDataAdapter(
                    @"SELECT * FROM  [dbo].[SimulatedMeasurements]", 
                    simulatedMeasurementsTableAdapter.Connection);

                measurmentsAdapter.InsertCommand = new SqlCommand(
                    @"INSERT INTO [dbo].[SimulatedMeasurements] ( 
                              [device_id], [threshold_id], [value], [timestamp]) 
                      VALUES (@device_id, @threshold_id, @value, @timestamp);",
                    simulatedMeasurementsTableAdapter.Connection);
                
                measurmentsAdapter.InsertCommand.CommandType = global::System.Data.CommandType.Text;
                measurmentsAdapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                measurmentsAdapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                measurmentsAdapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@value", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "value", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                measurmentsAdapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@timestamp", global::System.Data.SqlDbType.DateTime, 0, global::System.Data.ParameterDirection.Input, 0, 0, "timestamp", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));


                global::System.Data.ConnectionState previousConnectionState = measurmentsAdapter.InsertCommand.Connection.State;
                if (((measurmentsAdapter.InsertCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    measurmentsAdapter.InsertCommand.Connection.Open();
                }
                

                DataTable csvDataTable = new DataTable();
                adapter.Fill(csvDataTable);

                LinqExample.SLA_RT_monitoringDataSetMeasurements.SimulatedMeasurementsDataTable measurementsDataTable 
                    = new LinqExample.SLA_RT_monitoringDataSetMeasurements.SimulatedMeasurementsDataTable(); 

                foreach (DataRow currRow in csvDataTable.Rows)
                {
                    //Start to read after Timestemp cloumn
                    for (int i = 3; i < currRow.ItemArray.Count(); ++i)
                    {
                        if (currRow.ItemArray[i].ToString() != "")//if measure availble to this device
                        {
                            measurmentsAdapter.InsertCommand.Parameters[0].Value = currRow.ItemArray[0];
                            measurmentsAdapter.InsertCommand.Parameters[1].Value = csvDataTable.Columns[i].Caption;
                            measurmentsAdapter.InsertCommand.Parameters[2].Value = Convert.ToInt32(currRow.ItemArray[i]);

                        ///don't change
                          //  measurmentsAdapter.InsertCommand.Parameters[4].Value = DateTime.Parse((string)currRow.ItemArray[2]);

                           // measurmentsAdapter.InsertCommand.Parameters[4].Value = DateTime.Parse((string)currRow.ItemArray[2].ToString());

                            if (DateTime.Parse((string)currRow.ItemArray[1].ToString()) != null)
                            {
                                measurmentsAdapter.InsertCommand.Parameters[3].Value = DateTime.Parse((string)currRow.ItemArray[1].ToString());
                            }
                            else
                           {
                                measurmentsAdapter.InsertCommand.Parameters[3].Value = DateTime.Now;
                           }
                           
                           
                            int returnValue = measurmentsAdapter.InsertCommand.ExecuteNonQuery();


                            measurementsDataTable.AddSimulatedMeasurementsRow(
                                deviceIdToName[Convert.ToInt32(currRow.ItemArray[0])],
                                typeValues[Int32.Parse(csvDataTable.Columns[i].Caption)],
                                Convert.ToInt32(currRow.ItemArray[i]),
                                DateTime.Parse((string)currRow.ItemArray[1].ToString()));
                                

                        }
                    }
                }

                return measurementsDataTable;
            }
        }

        private void SimulatedDataLoader_Load(object sender, EventArgs e)
        {
            this.thresholdsTableAdapter.Connection.Open();
            // TODO: This line of code loads data into the 'sLA_RT_monitoringDataSetThreshold1.Thresholds' table. You can move, or remove it, as needed.
            this.thresholdsTableAdapter.Fill(this.sLA_RT_monitoringDataSetThreshold.Thresholds);
            // TODO: This line of code loads data into the 'sLA_RT_monitoringDataSetMeasurements.SimulatedMeasurements' table. You can move, or remove it, as needed.
            this.simulatedMeasurementsTableAdapter.Fill(this.sLA_RT_monitoringDataSetMeasurements.SimulatedMeasurements);

            devicesConnection = new SqlConnection(global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString);
            allDevices = new SqlCommand("SELECT id, name FROM [dbo].[Devices]", devicesConnection);

            if (((allDevices.Connection.State & global::System.Data.ConnectionState.Open)
             != global::System.Data.ConnectionState.Open))
            {
                allDevices.Connection.Open();
            }

            SqlDataReader devicesReader = allDevices.ExecuteReader();

            deviceIdToName = new Dictionary<int, string>();

            while (devicesReader.Read())
            {
                deviceIdToName[devicesReader.GetInt32(0)] = devicesReader.GetString(1);
            }
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
