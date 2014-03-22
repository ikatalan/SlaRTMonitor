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

        private SqlConnection dbConnection;//Open connections for devices table
        private SqlCommand devicesMeasurmentsCommand;

        private SqlConnection dbConnection2;//Open values for this device.
        private SqlCommand devicesMeasurmentsByThresholdCommand;

        private SqlConnection dbConnection3;//Open contract per threshold per device
        private SqlCommand thresholdContractCommand;

        private SqlConnection dbConnection4;//Open contract id for deviceType and thresholdId
        private SqlCommand contractIdCommand;

        private Thread fetcherThread;

        private int deviceId;
        private String deviceType;
        bool shouldContinue;


        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetGuageValueCallback(AGauge currGuage, System.Windows.Forms.Label currLabel, string thresholdName, float value);
        

        public Dashboard()
        {
            InitializeComponent();
   
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sLA_RT_monitoringDevicesDataSet.Devices' table. You can move, or remove it, as needed.
            this.devicesTableAdapter.Fill(this.sLA_RT_monitoringDevicesDataSet.Devices);

            fetcherThread = new Thread(new ParameterizedThreadStart(GuageDataFetcher));
            shouldContinue = true;
            fetcherThread.Start(this);

            dbConnection = new global::System.Data.SqlClient.SqlConnection();
            dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            // Used for filling list of items (device_name) per threshold_id            
            devicesMeasurmentsCommand = new SqlCommand(
                @"SELECT DISTINCT a.threshold_id, a.value, a.timestamp, b.minValue, b.maxValue, b.name FROM [dbo].[SimulatedMeasurements] a "
                + @"JOIN [dbo].[Thresholds] b ON a.threshold_id=b.id "
                + @"WHERE device_id= @device_id "
                + @"ORDER BY timestamp",
                dbConnection);


            if (((devicesMeasurmentsCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                devicesMeasurmentsCommand.Connection.Open();
            }

            devicesMeasurmentsCommand.CommandType = global::System.Data.CommandType.Text;
            devicesMeasurmentsCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));


            { //Init graph fetching data select command.
                dbConnection2 = new global::System.Data.SqlClient.SqlConnection();
                dbConnection2.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for filling list of items (device_name) per threshold_id            
                devicesMeasurmentsByThresholdCommand = new SqlCommand(
                    @"SELECT DISTINCT a.threshold_id, a.value, a.timestamp, b.minValue, b.maxValue, b.name FROM [dbo].[SimulatedMeasurements] a "
                    + @"JOIN [dbo].[Thresholds] b ON a.threshold_id=b.id "
                    + @"WHERE (device_id= @device_id) AND (threshold_id=@threshold_id) AND (timestamp > DATEADD(DAY, -1, GETUTCDATE())) "
                    + @"ORDER BY timestamp",
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
                dbConnection3 = new global::System.Data.SqlClient.SqlConnection();
                dbConnection3.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for filling list of items (device_name) per threshold_id            
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
                dbConnection4 = new global::System.Data.SqlClient.SqlConnection();
                dbConnection4.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for filling list of items (device_name) per threshold_id            
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
            


            try
            {
                deviceId = Int32.Parse(((DataRowView)listDevices.SelectedItem)[0].ToString());
                deviceType = ((DataRowView)listDevices.SelectedItem)[2].ToString(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            deviceId = Int32.Parse(((DataRowView)listDevices.SelectedItem)[0].ToString());
            deviceType = ((DataRowView)listDevices.SelectedItem)[2].ToString(); 
        }



        static private void GuageDataFetcher(object arg)
        {
            Dashboard me = (Dashboard)arg;

            while (me.shouldContinue)
            {
                int deviceId = me.deviceId;
                string deviceType = me.deviceType;

                me.devicesMeasurmentsCommand.Parameters["@device_id"].Value = deviceId;

                SqlDataReader reader = me.devicesMeasurmentsCommand.ExecuteReader();
                float value = 0;//add all the time the value
                int idx = 1;
                while (reader.Read())
                {
                    int thresholdId = reader.GetInt32(0);
                    int thresholdValue = reader.GetInt32(1);
                    //timestamp = reader.Get...(2)
                    int minValue = reader.GetInt32(3);
                    int maxValue = reader.GetInt32(4);
                    string thresholdName = reader.GetString(5);


                    //check here
                    value = value + (((float)thresholdValue) / (maxValue - minValue) * 100);

                    switch (idx)
                    {
                        case 1:
                            {
                                me.SetGuageValue(me.gauge1, me.lblGuage1, thresholdName, value);
                                //Fetch Graph Data for Graph1.
                                me.FillGraphWithData(me.zg1, thresholdId, deviceId, deviceType);


                            } break;
                        case 2:
                            {
                                me.SetGuageValue(me.gauge2, me.lblGuage2, thresholdName, value);
                                //Fetch Graph Data for Graph2.
                                me.FillGraphWithData(me.zg2, thresholdId, deviceId, deviceType);
                            } break;
                        case 3:
                            {
                                me.SetGuageValue(me.gauge3, me.lblGuage3, thresholdName, value);
                                //Fetch Graph Data for Graph3.
                                me.FillGraphWithData(me.zg3, thresholdId, deviceId, deviceType);
                            } break;
                    }

                    if (idx == 3)
                    {
                        break;
                    }

                    idx++;
                }

                reader.Close();


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

        private void FillGraphWithData(ZedGraph.ZedGraphControl zgc, int thresholdId, int deviceId, string deviceType)
        {
            
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels per selection
            myPane.Title.Text = "last 24 hours";
            // Change the color of the title
            //  myPane.Title.FontSpec.FontColor = Color.Blue;

            myPane.XAxis.Title.Text = "Time (Sec)";
            myPane.YAxis.Title.Text = "%";

            myPane.CurveList.Clear();// clear the graph

            myPane.Legend.IsVisible = false;

            XDate minDate = XDate.JulDayMax;
            XDate maxDate = XDate.JulDayMin;
            //Create Random colors to show on Graph
            
            PointPairList listDeviceValues = GetValuesForDevice(deviceId, thresholdId);

            //use this to add line width 3.0F
            LineItem myCurve = new LineItem("", listDeviceValues, Color.Red, SymbolType.XCross);
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

            //Show tooltips when the mouse hovers over a point
            zgc.IsShowPointValues = true;
            zgc.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

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

                devicesMeasurmentsByThresholdCommand.Parameters["@device_id"].Value = deviceId;
                devicesMeasurmentsByThresholdCommand.Parameters["@threshold_id"].Value = thresholdId;

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
                        measurementsReader.GetInt32(1) //value
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
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (currGuage.InvokeRequired)
            {
                SetGuageValueCallback d = new SetGuageValueCallback(SetGuageValue);
                this.Invoke(d, new object[] { currGuage, currLabel, thresholdName, value});
                //AGauge currGuage, Label currLabel, string thresholdName, float value
            }
            else
            {
                currGuage.Value = value;

                thresholdName = thresholdName.Replace(" ", String.Empty);//remove whitespaces
                currLabel.Text = thresholdName + " - " + value + "%";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
