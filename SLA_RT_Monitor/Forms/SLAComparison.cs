using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace LinqExample.Forms
{
    public partial class SLAComparison : Form
    {
        struct ThresholdItem
        {
            public int id;
            public string name;
            public int threshold_type_id;
            public int value;
        }

        class DeviceItem : Object
        {
            public int id;
            public string name;

            public override string ToString()
            {
                return name;
            }


        }

        
        private SqlConnection dbConnection2;
        private SqlConnection dbConnection3;//Open connections forSlaContracts table
        
        private SqlDataAdapter measurmentsValuesAdapter; // used for getting measurements valus for a specific device.
        private SqlDataAdapter singleThresholdValueAdapter;// used for getting threshold gaugeValue from Contracts


        private SqlConnection dbConnection;
        private SqlCommand devicesSqlCommand;

        private SqlConnection dbConnection4;
        private SqlCommand calculatedMeasurementsSqlCommand;


        public SLAComparison()
        {
            InitializeComponent();
            myPane = zg1.GraphPane;//connect the control to the pane
        }
        GraphPane myPane; 

        class TimeScopeItem {
            public TimeScopeItem(string _text, TimeSpan _span)
            {
                text = _text;
                span = _span;
            }

            public override string ToString()
            {
                return text;
            }
            public string Text
            {
                get {
                    return text;
                }
                
            }
            public TimeSpan TimeSpan
            {
                get
                {
                 return span;
                }
            }
            string text;
            TimeSpan span;
        }

        //Run when the Graph load //was private
        public void SLAComparison_Load(object sender, EventArgs e)
        {
            cmbBoxTimeScope.Items.Add(new TimeScopeItem("last 12 hours", new TimeSpan(12,0,0)));
            cmbBoxTimeScope.Items.Add(new TimeScopeItem("last 3 days", new TimeSpan(3,0,0,0)));
            cmbBoxTimeScope.Items.Add(new TimeScopeItem("last week", new TimeSpan(7,0,0,0)));
            cmbBoxTimeScope.Items.Add(new TimeScopeItem("last month", new TimeSpan(30,0,0,0)));
            cmbBoxTimeScope.Items.Add(new TimeScopeItem("beginning of time", new TimeSpan(500,0, 0, 0)));
         
            dbConnection = new global::System.Data.SqlClient.SqlConnection();
            dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            dbConnection2 = new global::System.Data.SqlClient.SqlConnection();
            dbConnection2.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            dbConnection3 = new global::System.Data.SqlClient.SqlConnection();
            dbConnection3.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            try
            {
                this.thresholdsTableAdapter.Fill(this.sLA_RT_monitoringDataSetThreshold.Thresholds);
            }
            catch (Exception)
            {
            }

            //         
            devicesSqlCommand = new SqlCommand(
                @"SELECT id, name FROM [dbo].[Devices] "
              + @"WHERE type=@device_type ",
                dbConnection);


            if (((devicesSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                devicesSqlCommand.Connection.Open();
            }

            devicesSqlCommand.CommandType = global::System.Data.CommandType.Text;
            devicesSqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_type", global::System.Data.SqlDbType.NChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_type", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));



            // used for getting measurements valus for a specific device.
            measurmentsValuesAdapter = new SqlDataAdapter(
                @"SELECT timestamp, value  "
                + @" FROM [dbo].[SimulatedMeasurements] a"
                + @" JOIN [dbo].[Devices] b ON a.device_id=b.id "
                + @" WHERE b.name=@device_name "
                + @" AND a.threshold_id=@threshold_id "
                + @" ORDER BY timestamp;",
                dbConnection2);
            
            if (((measurmentsValuesAdapter.SelectCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                measurmentsValuesAdapter.SelectCommand.Connection.Open();
            }
            

            measurmentsValuesAdapter.SelectCommand.CommandType = global::System.Data.CommandType.Text;
            measurmentsValuesAdapter.SelectCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_name", global::System.Data.SqlDbType.NChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_name", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            measurmentsValuesAdapter.SelectCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));



            dbConnection4 = new global::System.Data.SqlClient.SqlConnection();
            dbConnection4.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            // used for getting measurements valus for a specific device.
            calculatedMeasurementsSqlCommand = new SqlCommand(
                @"SELECT threshold_id, AVG(value)  "
                + @" FROM [dbo].[SimulatedMeasurements] a"
                + @" WHERE device_id=@device_id "
                + @" AND timestamp>=@timestamp "
                + @" GROUP BY threshold_id "
                + @" ORDER BY threshold_id ",
                dbConnection4);

            if (((calculatedMeasurementsSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                calculatedMeasurementsSqlCommand.Connection.Open();
            }


            calculatedMeasurementsSqlCommand.CommandType = global::System.Data.CommandType.Text;
            calculatedMeasurementsSqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            calculatedMeasurementsSqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@timestamp", global::System.Data.SqlDbType.DateTime, 0, global::System.Data.ParameterDirection.Input, 0, 0, "timestamp", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));



            // used for getting threshold gaugeValue from Contracts
            singleThresholdValueAdapter = new SqlDataAdapter(
                @"SELECT value "
                + @"FROM [dbo].[SlaContracts] "
                + @"WHERE threshold_id=@threshold_id;",
                dbConnection3);


            if (((singleThresholdValueAdapter.SelectCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                singleThresholdValueAdapter.SelectCommand.Connection.Open();
            }

            singleThresholdValueAdapter.SelectCommand.CommandType = global::System.Data.CommandType.Text;
            singleThresholdValueAdapter.SelectCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_name", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));

            //Fill combobox of device types from DB.
            InitCmbDeviceType();

            cmbBoxTimeScope.SelectedIndex = 0;
        }

        private void InitCmbDeviceType()
        {
            SqlConnection dbConnection4;
            SqlCommand deviceTypeSqlCommand;

            dbConnection4 = new global::System.Data.SqlClient.SqlConnection();
            dbConnection4.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            // Used for filling list of items (device_name) per threshold_id            
            deviceTypeSqlCommand = new SqlCommand(
                @"SELECT type FROM [dbo].[Devices] "
              + @"GROUP BY type;",
                dbConnection4);

            if (((deviceTypeSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                deviceTypeSqlCommand.Connection.Open();
            }

            deviceTypeSqlCommand.CommandType = global::System.Data.CommandType.Text;

            global::System.Data.ConnectionState previousConnectionState = deviceTypeSqlCommand.Connection.State;
            if (((deviceTypeSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                deviceTypeSqlCommand.Connection.Open();
            }

            SqlDataReader deviceTypeReader = null;

            try
            {
                deviceTypeReader = deviceTypeSqlCommand.ExecuteReader();

                cmbBoxDeviceType.Items.Clear();

                //Fill combobox with types
                while (deviceTypeReader.Read())
                {
                    cmbBoxDeviceType.Items.Add(deviceTypeReader.GetString(0));
                }
                // start with index 0 selected.
                if (cmbBoxDeviceType.Items.Count > 0)
                {
                    cmbBoxDeviceType.SelectedIndex = 0;
                }

                deviceTypeReader.Close();//Close the reader since we are opening it every time when selected item

                cmbBoxDeviceTypeIndexChanged(null, null);
            }
            catch (Exception)
            {
            }
        }


        private List<ThresholdItem> ReadFromDBThresholdType(string deviceType)
        {
            SqlConnection dbConnection4;
            SqlCommand thresholdTypeSqlCommand;

            dbConnection4 = new global::System.Data.SqlClient.SqlConnection();
            dbConnection4.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            thresholdTypeSqlCommand = new SqlCommand(
                @"SELECT id, name, threshold_type_id, b.value FROM [dbo].[Thresholds] a "
              + @"JOIN [dbo].[SlaContracts] b ON b.threshold_id = a.id "
              + @"WHERE b.device_type=@device_type "
              + @"ORDER BY a.id ASC",
                dbConnection4);

            if (((thresholdTypeSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                thresholdTypeSqlCommand.Connection.Open();
            }

            thresholdTypeSqlCommand.CommandType = global::System.Data.CommandType.Text;
            thresholdTypeSqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_type", global::System.Data.SqlDbType.NChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_type", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));

            global::System.Data.ConnectionState previousConnectionState = thresholdTypeSqlCommand.Connection.State;
            if (((thresholdTypeSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                thresholdTypeSqlCommand.Connection.Open();
            }

            SqlDataReader thresholdTypeReader = null;

            List<ThresholdItem> retVal = new List<ThresholdItem>();

            try
            {
                thresholdTypeSqlCommand.Parameters[0].Value = deviceType;
                thresholdTypeReader = thresholdTypeSqlCommand.ExecuteReader();
                
                while (thresholdTypeReader.Read())
                {
                    ThresholdItem item;
                    item.id = thresholdTypeReader.GetInt32(0);
                    item.name = thresholdTypeReader.GetString(1);
                    item.threshold_type_id = thresholdTypeReader.GetInt32(2);
                    item.value = thresholdTypeReader.GetInt32(3);
                    retVal.Add(item);
                }

                thresholdTypeReader.Close();//Close the reader since we are opening it every time when selected item

                return retVal;
            }
            catch (Exception e123)
            {
                MessageBox.Show(e123.ToString());
                return null;
            }
        }

        //Create the Graph and configure is proprties
        private void CreateGraph(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            List<ThresholdItem> listCurrentThresholds = this.ReadFromDBThresholdType(cmbBoxDeviceType.SelectedItem.ToString());
          
            // Set the titles and axis labels per selection
            myPane.Title.Text = cmbBoxDeviceType.GetItemText(cmbBoxDeviceType.SelectedItem);

            myPane.XAxis.Title.Text = "Thresholds";
            myPane.YAxis.Title.Text = "";

            myPane.CurveList.Clear();// clear the graph

            for ( int i =0; i < listCurrentThresholds.Count(); ++i ) 
            {
                ThresholdItem item = listCurrentThresholds[i];
                item.name = item.name.Replace(" ", String.Empty);//remove whitespaces
                LineItem myLine = new LineItem(item.name + "Threshold", 
                    new double[] { 
                        ((float)(i * 2 + 1)) / (listCurrentThresholds.Count() * 2) - (1.0 / (listCurrentThresholds.Count()*2 + 1)), 
                        ((float)(i * 2 + 1)) / (listCurrentThresholds.Count() * 2) + (1.0 / (listCurrentThresholds.Count()*2 + 1)) }, 
                    new double[] { item.value, item.value }, 
                    Color.Black, 
                    SymbolType.Diamond, 3.0f);
                myLine.IsX2Axis = true;
                myPane.CurveList.Add(myLine);
            }

            List<BarItem> listBarItems = new List<BarItem>();

            //Create Random colors to show on Graph
            Color[] barColors = new Color[]{
                //set of orange
                Color.FromArgb(49,130,189),
                Color.FromArgb(49,163,84),
                Color.FromArgb(99,99,99),
                Color.Azure,
                Color.Bisque,
                Color.Coral,
                Color.Crimson,
                Color.ForestGreen,
                Color.Lavender,
                Color.Navy
            };

            for (int idx = 0; idx < listDevices.SelectedItems.Count; ++idx )
            {
                DeviceItem currDevice = (DeviceItem)listDevices.SelectedItems[idx];
                String currDeviceName = currDevice.name;

                //Get one point for each threshold according to the device type.
                PointPairList listDeviceValues = GetDeviceData(listCurrentThresholds, currDevice.id);

                //use this to add line width 3.0F
                BarItem myCurve = new BarItem(currDeviceName, listDeviceValues, barColors[idx%barColors.Count()]);
                listBarItems.Add(myCurve);
            }

            
            for (int idx = 0; idx < listCurrentThresholds.Count(); ++idx)
            {
                ThresholdItem item = listCurrentThresholds[idx];
                item.name = item.name.Replace(" ", String.Empty);//remove whitespaces
                //myPane.CurveList.Average

                try
                {
                    double avg = listBarItems.Where(v => v.Points.Count > idx)
                                             .Select(v => v.Points[idx].Y).Average();

                    double stdDev = Math.Sqrt(listBarItems.Where(v => v.Points.Count > idx)
                                                          .Select(v => v.Points[idx].Y)
                                                          .Average(v => Math.Pow(v - avg, 2)));

                    //MessageBox.Show("i:" + idx + " stdDev: " + stdDev);
                    LineItem myLine = new LineItem(item.name + "Deviation",
                        new double[] { 
                            ((float)(idx * 2 + 1)) / (listCurrentThresholds.Count() * 2) - (1.0 / (listCurrentThresholds.Count()*2 + 1)), 
                            ((float)(idx * 2 + 1)) / (listCurrentThresholds.Count() * 2) + (1.0 / (listCurrentThresholds.Count()*2 + 1)) },
                        new double[] { stdDev, stdDev },
                        Color.FromArgb(240, 59, 32),
                        SymbolType.Diamond, 3.0f);
                    myLine.IsX2Axis = true;
                    myPane.CurveList.Add(myLine);
                }
                catch (InvalidOperationException )
                {

                }
                catch (NullReferenceException)
                {

                }

            }

            myPane.CurveList.AddRange(listBarItems);
            
            
            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);

            // Fill the pane background with a color gradient
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            //This informs ZedGraph to use the labels supplied by the user in Axis.Scale.TextLabels
             //Axis.Default.Type = AxisType.Text;

            //Show tooltips when the mouse hovers over a point
            zgc.IsShowPointValues = true;
            zgc.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            
            // Set the XAxis to date type
            myPane.XAxis.Type = AxisType.Text;
            string[] thresholdNames = new string[listCurrentThresholds.Count];
            for (int i = 0; i < thresholdNames.Count(); ++i)
            {
                thresholdNames[i] = listCurrentThresholds[i].name;
            }

            myPane.XAxis.Scale.TextLabels = thresholdNames;
            myPane.XAxis.IsVisible = true;
            
            myPane.X2Axis.Scale.Min = 0;
            myPane.X2Axis.Scale.Max = 1;
            myPane.X2Axis.IsVisible = false;

            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.IsVisible = true;

    
            // Calculate the Axis Scale Ranges
            axisChangeZedGraph(zgc); //refrsh the graph
        }

        private PointPairList GetDeviceData(List<ThresholdItem> listCurrentThresholds, int device_id)
        {
            PointPairList listValues = new PointPairList();
            try
            {
                SqlDataReader measurementsReader = null;

                calculatedMeasurementsSqlCommand.Parameters["@device_id"].Value = device_id;
                calculatedMeasurementsSqlCommand.Parameters["@timestamp"].Value = GetDateTimeFromTimeScope();

                global::System.Data.ConnectionState previousConnectionState = calculatedMeasurementsSqlCommand.Connection.State;
                if (((calculatedMeasurementsSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    calculatedMeasurementsSqlCommand.Connection.Open();
                }

                measurementsReader = calculatedMeasurementsSqlCommand.ExecuteReader();


                while (measurementsReader.Read())
                {   
                    listValues.Add(
                        measurementsReader.GetInt32(0), //threshjold_id
                        measurementsReader.GetDouble(1) //average gaugeValue for time scope.
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

        private object GetDateTimeFromTimeScope()
        {
            TimeSpan x = new TimeSpan();
            if (cmbBoxTimeScope.SelectedItem != null)
            {
                x = ((TimeScopeItem)cmbBoxTimeScope.SelectedItem).TimeSpan;
            }

            return DateTime.Now.Subtract(x);
        }

       
        // Display customized tooltips when the mouse hovers over a point
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane,
                        CurveItem curve, int iPt)
        {
            // Get the PointPair that is under the mouse
            PointPair pt = curve[iPt];
          
            curve.Label.Text = curve.Label.Text.Replace(" ", String.Empty);//remove whitespaces from device name

            XDate the_date = new XDate(pt.X);//Replace the pair double to date
            return curve.Label.Text + " is " + pt.Y.ToString("f2") + " units : " + pt.X + " ";
        }

        // used for getting threshold gaugeValue from Contracts
        private Int32 GetContractThreshold()
        {
            singleThresholdValueAdapter.SelectCommand.Parameters[0].Value = GetSelectedThresholdId();

            global::System.Data.ConnectionState previousConnectionState = singleThresholdValueAdapter.SelectCommand.Connection.State;
            if (((singleThresholdValueAdapter.SelectCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                singleThresholdValueAdapter.SelectCommand.Connection.Open();
            }

            object result = singleThresholdValueAdapter.SelectCommand.ExecuteScalar();
            if (result == null)
            {
                return 0;
            }
            return (Int32)result;

        }

        private Int32 GetSelectedThresholdId()
        {
            DataRowView dataRow = (DataRowView)cmbBoxDeviceType.SelectedItem;

            LinqExample.SLA_RT_monitoringDataSetThreshold.ThresholdsRow selectedRow =
                (LinqExample.SLA_RT_monitoringDataSetThreshold.ThresholdsRow)dataRow.Row;

            return selectedRow.id;
        }

        //
        private PointPairList GetValuesForDevice(String deviceName, int threshold_id)
        {
            PointPairList listValues = new PointPairList();
            try
            {
                SqlDataReader measurementsReader = null;

                measurmentsValuesAdapter.SelectCommand.Parameters[0].Value = deviceName;
                measurmentsValuesAdapter.SelectCommand.Parameters[1].Value = threshold_id;

                global::System.Data.ConnectionState previousConnectionState = measurmentsValuesAdapter.SelectCommand.Connection.State;
                if (((measurmentsValuesAdapter.SelectCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    measurmentsValuesAdapter.SelectCommand.Connection.Open();
                }

                measurementsReader = measurmentsValuesAdapter.SelectCommand.ExecuteReader();

                
                while (measurementsReader.Read())
                {
                    DateTime x = measurementsReader.GetDateTime(0);//time
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


        private void cmbBoxDeviceTypeIndexChanged(object sender, EventArgs e)
        {
            SqlDataReader devicesReader = null;

            devicesSqlCommand.Parameters["@device_type"].Value = cmbBoxDeviceType.SelectedItem.ToString();

            global::System.Data.ConnectionState previousConnectionState = devicesSqlCommand.Connection.State;
            if (((devicesSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                devicesSqlCommand.Connection.Open();
            }
            try
            {
                devicesReader = devicesSqlCommand.ExecuteReader();
            }
            catch (Exception e123)
            {
                MessageBox.Show(e123.ToString());
            }

            listDevices.Items.Clear();

            while (devicesReader.Read())
            {
                DeviceItem item = new DeviceItem();
                item.id = devicesReader.GetInt32(0);
                item.name = devicesReader.GetString(1);
                listDevices.Items.Add(item);
                listDevices.SelectedItems.Add(item);
            }

            devicesReader.Close();//Close the reader since we are opening it every time when selected item
            CreateGraph(zg1);

        }

        //Back to main menu
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Called When select other device 
        private void listDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateGraph(zg1);
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

        private void zg1_Load(object sender, EventArgs e)
        {

        }


    }
}
