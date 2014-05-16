using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace LinqExample
{
    public partial class Dashboard : Form
    {

        private SqlConnection dbConnection;//connections for devicesMeasurmentsCommand
        private SqlCommand devicesMeasurmentsCommand; // Select measurements

        private SqlConnection dbConnection2;//connections for devicesMeasurmentsByThresholdCommand.
        private SqlCommand devicesMeasurmentsByThresholdCommand; // Select measurements for the list of device.

        private SqlConnection dbConnection3;//connections thresholdContractCommand
        private SqlCommand thresholdContractCommand; // get contract gaugeValue by contract Id.

        private SqlConnection dbConnection4;//contractIdCommand
        private SqlCommand contractIdCommand; // Get contract for deviceType and threhsoldId

        private Thread fetcherThread; // thread for reading latest measurements data.

        private int deviceId; // the ID of the current selected device 
        private String deviceType; // the type of the current selected device 
        bool shouldContinue; // true when the thread should continue.

        DateTime lastIncidentsCheck; // The last time we have checked incidents.
        bool isStartedReadingValues;   
        
        DashboardIncidentsProvider incidentsProvider; // The provider for incidents data.

        Dictionary<int, int> thresholdForGauge; // used to store on which gauge this threshold appears.

        private int? startDeviceId; // The first device to select when the form is loaded.


        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetGuageValueCallback(AGauge currGuage, System.Windows.Forms.Label currLabel, string thresholdName, float value);

        //delegate to fill the incident datagrid
        delegate void FillIncidentsCallBack(DataTable table);

        // delegate to clear all guages and graph data.
        delegate void ClearDataCallback(int index);
        

        public Dashboard()
        {
            InitializeComponent();
        }

        public Dashboard(int initDeviceId)
        {
            InitializeComponent();
            startDeviceId = initDeviceId; // the device ID to start with.
        }

        //When the form Load
        private void Dashboard_Load(object sender, EventArgs e)
        {
            thresholdForGauge = new Dictionary<int, int>();
            //Sort the datagrid by time ,show the most updated line
            dataGridIncidents.Sort(dataGridIncidents.Columns[4], ListSortDirection.Descending);

            isStartedReadingValues = false;

            lastIncidentsCheck = DateTime.Now.Subtract(new TimeSpan(3, 0, 0));
            //Provide the incident numbers and will fill the IncidentDataGrid
            incidentsProvider = new DashboardIncidentsProvider();


            // Fill the list of devices
            this.devicesTableAdapter.Fill(this.sLA_RT_monitoringDevicesDataSet.Devices);

            //Create the thread with GuageDataFetcher as the starting function.
            fetcherThread = new Thread(new ParameterizedThreadStart(GuageDataFetcher));
            shouldContinue = true;

            //Start the update thread.
            fetcherThread.Start(this);

            //Connection to devicesMeasurmentsCommand
            dbConnection = new global::System.Data.SqlClient.SqlConnection();
            dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            // Used for having all the information about measurements and threshold data per Device ID
            devicesMeasurmentsCommand = new SqlCommand(
                @"SELECT a.threshold_id, a.value, a.timestamp, b.minValue, b.maxValue, b.name "
                + @"FROM [dbo].[SimulatedMeasurements] a "
                + @"JOIN [dbo].[Thresholds] b ON a.threshold_id=b.id "
                + @"WHERE a.device_id= @device_id "
                + @"ORDER BY timestamp DESC",
                dbConnection);

            global::System.Data.ConnectionState previousConnectionState = devicesMeasurmentsCommand.Connection.State;
            if (((devicesMeasurmentsCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                devicesMeasurmentsCommand.Connection.Open();
            }

            devicesMeasurmentsCommand.CommandType = global::System.Data.CommandType.Text;
            devicesMeasurmentsCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));


            { //Open values for devicesMeasurmentsByThresholdCommand
                dbConnection2 = new global::System.Data.SqlClient.SqlConnection();
                dbConnection2.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for filling list of items (device_name) per threshold_id   (last 24 hours)        
                devicesMeasurmentsByThresholdCommand = new SqlCommand(
                    @"SELECT DISTINCT a.threshold_id, a.value, a.timestamp, b.minValue, b.maxValue, b.name FROM [dbo].[SimulatedMeasurements] a "
                    + @"JOIN [dbo].[Thresholds] b ON a.threshold_id=b.id "
                    + @"WHERE (device_id= @device_id) AND (threshold_id=@threshold_id) AND (timestamp > DATEADD(DAY, -1, GETUTCDATE())) "
                    + @"ORDER BY timestamp ",
                    dbConnection2);


                if (((devicesMeasurmentsByThresholdCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    devicesMeasurmentsByThresholdCommand.Connection.Open();
                }

                devicesMeasurmentsByThresholdCommand.CommandType = global::System.Data.CommandType.Text;
                devicesMeasurmentsByThresholdCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                devicesMeasurmentsByThresholdCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));

            }


            {
                //connection for thresholdContractCommand
                dbConnection3 = new global::System.Data.SqlClient.SqlConnection();
                dbConnection3.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for having the contract threshold values per contract ID           
                thresholdContractCommand = new SqlCommand(
                    @"SELECT value "
                    + @"FROM [dbo].[SlaContracts] "
                    + @"WHERE contract_id= @contract_id",
                    dbConnection3);

                if (((thresholdContractCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    thresholdContractCommand.Connection.Open();
                }

                thresholdContractCommand.CommandType = global::System.Data.CommandType.Text;
                thresholdContractCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@contract_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "contract_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            }

            {
                //connection for contractIdCommand
                dbConnection4 = new global::System.Data.SqlClient.SqlConnection();
                dbConnection4.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for having contract ID per  threshold_id and  device_type          
                contractIdCommand = new SqlCommand(
                    @"SELECT contract_id "
                    + @"FROM [dbo].[SlaContracts] "
                    + @"WHERE threshold_id= @threshold_id AND device_type=@device_type",
                    dbConnection4);

                if (((contractIdCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    contractIdCommand.Connection.Open();
                }

                contractIdCommand.CommandType = global::System.Data.CommandType.Text;
                contractIdCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_type", global::System.Data.SqlDbType.NChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_type", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                contractIdCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            }


            /// Init graphs layout.
            InitGraph(zg1); //Left graph
            InitGraph(zg2); //Middle graph
            InitGraph(zg3); //Right graph

            try
            {
                //If no startdeviceId chosen than start the form with the first device Selected.
                if (startDeviceId != null)
                {
                    foreach (DataRowView item in listDevices.Items)
                    {
                        if (Int32.Parse(item[0].ToString()) == startDeviceId)
                        {
                            listDevices.SelectedItem = item;
                        }
                    }        
                } 

                if (listDevices.SelectedItem != null)
                {
                    deviceId = Int32.Parse(((DataRowView)listDevices.SelectedItem)[0].ToString());
                    deviceType = ((DataRowView)listDevices.SelectedItem)[2].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void InitGraph(ZedGraph.ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            myPane.XAxis.Title.Text = "Time (Sec)";
            myPane.YAxis.Title.Text = "%";
            
            //Set the font size 
            myPane.Title.FontSpec.Size = 20.0f;
            myPane.YAxis.Title.FontSpec.Size = 20.0f;
            myPane.YAxis.Scale.FontSpec.Size = 20.0f;
            myPane.XAxis.Title.FontSpec.Size = 20.0f;
            myPane.XAxis.Scale.FontSpec.Size = 20.0f;

            myPane.Legend.IsVisible = false;


            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);

            // Fill the pane background with a color gradient
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            //This informs ZedGraph to use the labels supplied by the user in Axis.Scale.TextLabels
            Axis.Default.Type = AxisType.Text;

            // Set the XAxis to date type
            myPane.XAxis.Type = AxisType.Date;

            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.IsVisible = true;

            // Calculate the Axis Scale Ranges
            axisChangeZedGraph(zgc); //refrsh the graph
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (thresholdForGauge)
            {
                thresholdForGauge.Clear();
            }
            if (listDevices.SelectedItem != null)
            {
                deviceId = Int32.Parse(((DataRowView)listDevices.SelectedItem)[0].ToString());
                deviceType = ((DataRowView)listDevices.SelectedItem)[2].ToString();
            }
        }

        // thread for reading latest measurements data.
        static private void GuageDataFetcher(object arg)
        {
            Dashboard me = (Dashboard)arg;

            while (me.shouldContinue)
            {
                //Get current selected device
                int deviceId = me.deviceId;
                string deviceType = me.deviceType;

                try
                {
                    //If deviceId is valid insert paramteer to query.
                    if (deviceId >= 0)
                    {
                        me.devicesMeasurmentsCommand.Parameters["@device_id"].Value = deviceId;
                    }
                    else
                    {
                        continue;
                    }
                }          
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    
                }

                int idx = 1;

                if (((me.devicesMeasurmentsCommand.Connection.State & global::System.Data.ConnectionState.Open)
                       != global::System.Data.ConnectionState.Open))
                {
                    me.devicesMeasurmentsCommand.Connection.Open();
                }
                SqlDataReader reader = me.devicesMeasurmentsCommand.ExecuteReader();

                float gaugeValue = 0; // the gauge value to show.
                List<int> alreadyFoundthresholdIds = new List<int>(); // ensures we iterate ove a threshold only once.
                bool isFillByOrder = (me.thresholdForGauge.Count == 0); // Keep threshold order on graphs.
                while (reader.Read())
                {
                    int thresholdId = reader.GetInt32(0);
                    double thresholdValue = reader.GetDouble(1);
                    //timestamp = reader.Get...(2)
                    int minValue = reader.GetInt32(3);
                    int maxValue = reader.GetInt32(4);
                    string thresholdName = reader.GetString(5);
                    thresholdName = thresholdName.Replace(" ", String.Empty);//remove whitespaces

                    //threshold exists, continue.
                    if (alreadyFoundthresholdIds.Contains(thresholdId))
                    {
                        continue;
                    }
                    alreadyFoundthresholdIds.Add(thresholdId);

                    // calculate the gauge value normalized to 1-100.
                    gaugeValue = (((float)thresholdValue) / (maxValue - minValue) * 100);
                    gaugeValue = (float)Math.Round(gaugeValue);
                  

                    //Assign the appropriate graph & label & gauge to the current iterated threshold 
                    ZedGraphControl zgc = null;
                    AGauge aguage = null;
                    System.Windows.Forms.Label lbl = null;
                    lock(me.thresholdForGauge) 
                    { 
                        //Find a place to show the the threshold.

                        //Keep threshold in place. 
                        if (isFillByOrder)
                        {
                            switch (idx)
                            {
                                case 1:
                                    {
                                        zgc = me.zg1;
                                        aguage = me.gauge1;
                                        lbl = me.lblGuage1;
                                        me.thresholdForGauge[thresholdId] = 1;
                                    } break;
                                case 2:
                                    {
                                        zgc = me.zg2;
                                        aguage = me.gauge2;
                                        lbl = me.lblGuage2;
                                        me.thresholdForGauge[thresholdId] = 2;
                                    } break;
                                case 3:
                                    {
                                        zgc = me.zg3;
                                        aguage = me.gauge3;
                                        lbl = me.lblGuage3;
                                        me.thresholdForGauge[thresholdId] = 3;
                                    } break;
                            }
                        }
                        else // Find any threshold a new place.
                        {
                            
                            //Read the placement for the current threshold.
                            if (me.thresholdForGauge.ContainsKey(thresholdId))
                            {

                                switch (me.thresholdForGauge[thresholdId])
                                {
                                    case 1:
                                        zgc = me.zg1;
                                        aguage = me.gauge1;
                                        lbl = me.lblGuage1;
                                        break;
                                    case 2:
                                        zgc = me.zg2;
                                        aguage = me.gauge2;
                                        lbl = me.lblGuage2;
                                        break;
                                    case 3:
                                        zgc = me.zg3;
                                        aguage = me.gauge3;
                                        lbl = me.lblGuage3;
                                        break;
                                }
                            }
                            else // if we have more space for a new threshodl (up to 3) assign the new threhsold its place.
                            {
                                if (me.thresholdForGauge.Count == 0)
                                {
                                    zgc = me.zg1;
                                    aguage = me.gauge1;
                                    lbl = me.lblGuage1;
                                    me.thresholdForGauge[thresholdId] = 1;
                                }
                                else if (me.thresholdForGauge.Count == 1)
                                {
                                    zgc = me.zg2;
                                    aguage = me.gauge2;
                                    lbl = me.lblGuage2;
                                    me.thresholdForGauge[thresholdId] = 2;
                                }
                                else if (me.thresholdForGauge.Count == 2)
                                {
                                    zgc = me.zg3;
                                    aguage = me.gauge3;
                                    lbl = me.lblGuage3;
                                    me.thresholdForGauge[thresholdId] = 3;
                                }
                            }
                        }
                    }

                    //If the threshold have a place to be shown. set the data to the Gauge, Graph and Label.
                    if (zgc != null)
                    {
                        me.SetGuageValue(aguage, lbl, thresholdName, gaugeValue);
                        //Fetch Graph Data for Graph1.
                        me.FillGraphWithData(zgc, thresholdId, deviceId, deviceType, thresholdName);
                    }
                        
                    idx++;

                    if (idx == 4)
                    {
                        break;
                    }

                }
                reader.Close();
            
                // Clear gauges for unused places.
                switch (idx)
                {
                    case 0:
                    case 1:
                        me.ClearGauge(1);
                        me.ClearGauge(2);
                        me.ClearGauge(3);
                        break;
                    case 2:
                        me.ClearGauge(2);
                        me.ClearGauge(3);
                        break;
                    case 3:
                        me.ClearGauge(3);
                        break;
                }

                // Read Incidents from the DB and put it in a datagrid.
                me.ReadIncidentsFromDB();
             
            }
        }

        private void ReadIncidentsFromDB()
        {
            //Init the incidents table.
            DataTable incidentsTable = new DataTable();

            List<int> deviceIds = new List<int>();

            foreach (DataRowView listItem in listDevices.Items)
            {
                deviceIds.Add((Int32)listItem[0]);
            }

            foreach( int deviceId in deviceIds) 
            {
                if (deviceId >= 0)
                {
                    devicesMeasurmentsCommand.Parameters["@device_id"].Value = deviceId;
                }

                if (((devicesMeasurmentsCommand.Connection.State & global::System.Data.ConnectionState.Open)
                       != global::System.Data.ConnectionState.Open))
                {
                    devicesMeasurmentsCommand.Connection.Open();
                }


                SqlDataReader reader2 = devicesMeasurmentsCommand.ExecuteReader();

                // Fill incidents table for each threshold 
                List<int> listThresholds = new List<int>();
                while (reader2.Read())
                {
                    isStartedReadingValues = true;

                    int thresholdId = reader2.GetInt32(0);

                    if (!listThresholds.Contains(thresholdId))
                    {
                        listThresholds.Add(thresholdId);
                        incidentsProvider.GetIncedentsFor(thresholdId, deviceId, lastIncidentsCheck, ref incidentsTable);
                    }
                }
                reader2.Close();
                reader2 = null;
            }

            if (isStartedReadingValues)
            {
                lastIncidentsCheck = DateTime.Now;
            }

            if (incidentsTable.Rows.Count != 0)
            {
                //Fill the data grid
                SetIncidentsData(incidentsTable);  
            }

            
        }

        // Display customized tooltips when the mouse hovers over a point
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane,
                        CurveItem curve, int iPt)
        {
            // Get the PointPair that is under the mouse
            PointPair pt = curve[iPt];

            curve.Label.Text = curve.Label.Text.Replace(" ", String.Empty);//remove whitespaces from device name

            XDate the_date = new XDate(pt.X);//Replace the pair double to date
            return curve.Label.Text + " is " + pt.Y.ToString("f2") + " units at Time: " + the_date.DateTime.TimeOfDay + " ";
        }

        delegate void axisChangeZedGraphCallBack(ZedGraphControl zg);
        //Refresh the Graph to show the changes
        private void axisChangeZedGraph(ZedGraphControl zg)
        {
            if (zg.InvokeRequired)
            {
                axisChangeZedGraphCallBack ad = new axisChangeZedGraphCallBack(axisChangeZedGraph);
                zg.Invoke(ad, new object[] { zg });
            }
            else
            {
                zg.AxisChange();
                zg.Invalidate();
                zg.Refresh();
            }
        }

        // Fill the graph with the data provided.
        private void FillGraphWithData(ZedGraph.ZedGraphControl zgc, int thresholdId, int deviceId, string deviceType, string thresholdName)
        {
            if (zgc == null)
            {
                return;
            }

            GraphPane myPane = zgc.GraphPane;
            

            // Set the titles and axis labels per selection
            thresholdName = thresholdName.Replace(" ", String.Empty);//remove whitespaces
            myPane.Title.Text = thresholdName + " - " + "last 24 hours";
           // myPane.Title.Text = "last 24 hours";
            myPane.XAxis.Title.Text = "Time (Sec)";
            myPane.YAxis.Title.Text = "%";

            // Change the color of the title
            //  myPane.Title.FontSpec.FontColor = Color.Blue;

            //Set the font size 
            myPane.Title.FontSpec.Size = 20.0f; 
            myPane.YAxis.Title.FontSpec.Size = 20.0f;
            myPane.YAxis.Scale.FontSpec.Size = 20.0f;
            myPane.XAxis.Title.FontSpec.Size = 20.0f;
            myPane.XAxis.Scale.FontSpec.Size = 20.0f;

          
            myPane.CurveList.Clear();// clear the graph

            myPane.Legend.IsVisible = false;

            XDate minDate = XDate.JulDayMax;
            XDate maxDate = XDate.JulDayMin;
            //Create Random colors to show on Graph
            
            PointPairList listDeviceValues = GetValuesForDevice(deviceId, thresholdId);

            //use this to add line width 3.0F
            LineItem myCurve = new LineItem("", listDeviceValues, Color.Blue, SymbolType.XCross);
            myPane.CurveList.Add(myCurve);

            if (listDeviceValues.Count > 0)
            {
                XDate firstDate = (XDate)(listDeviceValues[0].X);
                XDate lastDate = (XDate)listDeviceValues[listDeviceValues.Count - 1].X;
                if (minDate == XDate.JulDayMax) //The max valid Julian Day, which corresponds to January 1st, 4713 B.C
                {
                    minDate = firstDate;
                }
                else if (firstDate < minDate)
                {
                    minDate = firstDate;
                }

                if (maxDate == XDate.JulDayMin)//The minimum valid Julian Day, which corresponds to January 1st, 4713 B.C
                {
                    maxDate = lastDate;
                }
                else if (lastDate > maxDate)
                {
                    maxDate = lastDate;
                }
            }

            Int32 thresholdValue = GetContractThreshold(deviceType, thresholdId);//Read the Threshold values
            PointPairList thresholdPointList = new PointPairList();
            thresholdPointList.Add(new PointPair(minDate, thresholdValue));
            thresholdPointList.Add(new PointPair(maxDate, thresholdValue));

            myPane.CurveList.Insert(0, new LineItem("Threshold", thresholdPointList, Color.FromArgb(255, 0, 0, 0), SymbolType.XCross, 3.0f));

            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);

            // Fill the pane background with a color gradient
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            //This informs ZedGraph to use the labels supplied by the user in Axis.Scale.TextLabels
            Axis.Default.Type = AxisType.Text;

            ////Show tooltips when the mouse hovers over a point
            //zgc.IsShowPointValues = true;
            //zgc.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            // Set the XAxis to date type
            myPane.XAxis.Type = AxisType.Date;

            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.IsVisible = true;

            // Calculate the Axis Scale Ranges
            axisChangeZedGraph(zgc); //refrsh the graph
            
        }

        private PointPairList GetValuesForDevice(int deviceId, int thresholdId)
        {
            PointPairList listValues = new PointPairList();
            try
            {
                SqlDataReader measurementsReader = null;

                if (deviceId >= 0)
                {
                    devicesMeasurmentsByThresholdCommand.Parameters["@device_id"].Value = deviceId;
                    devicesMeasurmentsByThresholdCommand.Parameters["@threshold_id"].Value = thresholdId;
                }
                     

                global::System.Data.ConnectionState previousConnectionState = devicesMeasurmentsByThresholdCommand.Connection.State;
                if (((devicesMeasurmentsByThresholdCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    devicesMeasurmentsByThresholdCommand.Connection.Open();
                }

                measurementsReader = devicesMeasurmentsByThresholdCommand.ExecuteReader();

                
                while (measurementsReader.Read())
                {
                    DateTime x = measurementsReader.GetDateTime(2);//time
                    listValues.Add(
                      new XDate(x),
                        measurementsReader.GetDouble(1) //gaugeValue
                    );
                }

                measurementsReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return listValues;
        }

        private int GetContractThreshold(string deviceType, int thresholdId)
        {
            //Fetch Contract_id  

            contractIdCommand.Parameters["@device_type"].Value = deviceType;
            contractIdCommand.Parameters["@threshold_id"].Value = thresholdId;

            var contractIdObject = contractIdCommand.ExecuteScalar();
            if (contractIdObject == null)
            {
                return 0;
            }
            int contract_id = Int32.Parse(contractIdObject.ToString());


            //Fetch Threshold data for this contract.
            thresholdContractCommand.Parameters["@contract_id"].Value = contract_id;

            global::System.Data.ConnectionState previousConnectionState = thresholdContractCommand.Connection.State;
            if (((thresholdContractCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                thresholdContractCommand.Connection.Open();
            }


            return (Int32)thresholdContractCommand.ExecuteScalar();
        }


        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            shouldContinue = false;
        }


        private void SetGuageValue(AGauge currGuage, System.Windows.Forms.Label currLabel, string thresholdName, float value)
       // private void SetGuageValue(AGauge currGuage, System.Windows.Forms.Label currLabel, string thresholdName, double gaugeValue)
        {
            if (currLabel == null) { return; }
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (currGuage.InvokeRequired)
            {
                SetGuageValueCallback d = new SetGuageValueCallback(SetGuageValue);
                thresholdName = thresholdName.Replace(" ", String.Empty);//remove whitespaces
                //this.Invoke(d, new object[] { currGuage, currLabel, thresholdName, gaugeValue});
                this.Invoke(d, new object[] { currGuage, currLabel, thresholdName, value });
                //AGauge currGuage, Label currLabel, string thresholdName, float gaugeValue
            }
            else
            {
                currGuage.Value = value;
               //need to fix values but for now lets not go over 100%
                if (value > 100) { value = 100; }
                thresholdName = thresholdName.Replace(" ", String.Empty);//remove whitespaces
                currLabel.Text = thresholdName + " - " + value + "%";
               
            }
        }

        private void SetIncidentsData(DataTable incidentsTable)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (dataGridIncidents.InvokeRequired)
            {
                FillIncidentsCallBack d = new FillIncidentsCallBack(SetIncidentsData);
                this.BeginInvoke(d, new object[] { incidentsTable });
            }
            else
            {
                if (dataGridIncidents.DataSource == null)
                {
                    dataGridIncidents.AutoGenerateColumns = true;
                    dataGridIncidents.DataSource = new DataTable(); 

                }
                DataTable incidentsCurrentData = ((DataTable)dataGridIncidents.DataSource);
                incidentsCurrentData.Merge(incidentsTable);

                dataGridIncidents.Sort(dataGridIncidents.Columns[4], ListSortDirection.Descending);
            }
        }

        private void ClearGauge(int idx)
        {
            if (lblGuage1.InvokeRequired)
            {
                ClearDataCallback d = new ClearDataCallback(ClearGauge);
                this.Invoke(d, new object[] {idx});
            }
            else
            {
                ZedGraph.ZedGraphControl currZG = null;
                System.Windows.Forms.Label currLabel = null;
                System.Windows.Forms.AGauge currGauge = null;

                if (idx == 1)
                {
                    currZG = zg1;
                    currLabel = lblGuage1;
                    currGauge = gauge1;
                }

                if (idx == 2)
                {
                    currZG = zg2;
                    currLabel = lblGuage2;
                    currGauge = gauge2;
                }

                if (idx == 3)
                {
                    currZG = zg3;
                    currLabel = lblGuage3;
                    currGauge = gauge3;
                }

                if (currZG != null)
                {
                    GraphPane myPane = currZG.GraphPane;
                    myPane.Title.Text = "No data.";
                    myPane.CurveList.Clear();// clear the graph
                    currZG.Invalidate();
                    currZG.Refresh();
                }

                if (currLabel != null)
                {
                    currLabel.Text = "No data";
                }

                if (currGauge != null)
                {
                    currGauge.Value = 0;
                }
            }
        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            fetcherThread.Abort(this);//Kiil the thread
            shouldContinue = false;
            this.Close();
        }


    }
}
