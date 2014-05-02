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

namespace LinqExample
{
    public partial class Graphs : Form
    {
        private SqlConnection dbConnection;//Open connections for SimulatedMeasurements table
        private SqlConnection dbConnection2;
        private SqlConnection dbConnection3;//Open connections forSlaContracts table
        private SqlCommand measurmentsSqlCommand;//used for filling list of items (device_name) per threshold_id      
        private SqlDataAdapter measurmentsValuesAdapter; // used for getting measurements valus for a specific device.
        private SqlDataAdapter singleThresholdValueAdapter;// used for getting threshold value from Contracts
        

        public Graphs()
        {
            InitializeComponent();
            myPane = zg1.GraphPane;//connect the control to the pane
        }
        GraphPane myPane; 

        //Run when the Graph load //was privte
        public void Graphs_Load(object sender, EventArgs e)
        {
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


            // Used for filling list of items (device_name) per threshold_id            
            measurmentsSqlCommand = new SqlCommand(
                @"SELECT b.name FROM [dbo].[SimulatedMeasurements] a "
                    + @"JOIN [dbo].[Devices] b ON a.device_id=b.id "
                    + @"WHERE a.threshold_id=@threshold_id "
                    + @"GROUP BY b.name;",
                dbConnection);


            if (((measurmentsSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                measurmentsSqlCommand.Connection.Open();
            }

            measurmentsSqlCommand.CommandType = global::System.Data.CommandType.Text;
            measurmentsSqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));



            // used for getting measurements valus for a specific device.
            measurmentsValuesAdapter = new SqlDataAdapter(
                @"SELECT timestamp, value  "
                + @" FROM [dbo].[SimulatedMeasurements] a"
                + @" JOIN [dbo].[Devices] b ON a.device_id=b.id "
                + @" WHERE b.name=@device_name "
                + @" AND a.threshold_id=@threshold_id "
                + @" AND [timestamp] >= DATEADD(HOUR, -4, GETDATE())  " // showing the last 4 hours since it become diffcult to read
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



            // used for getting threshold value from Contracts
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


            cmbBoxThresholdTypes_SelectedIndexChanged(null, null);//Show the first value
        }
        //Run when select devices from the list 
        private void cmbBoxThresholdTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataReader measurementsReader=null;
            DataRowView dataRow = (DataRowView)cmbBoxThresholdTypes.SelectedItem;

            LinqExample.SLA_RT_monitoringDataSetThreshold.ThresholdsRow selectedRow =
                (LinqExample.SLA_RT_monitoringDataSetThreshold.ThresholdsRow) dataRow.Row;

            measurmentsSqlCommand.Parameters[0].Value = selectedRow.id;

            global::System.Data.ConnectionState previousConnectionState = measurmentsSqlCommand.Connection.State;
            if (((measurmentsSqlCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                measurmentsSqlCommand.Connection.Open();
            }
            try
            {
                measurementsReader = measurmentsSqlCommand.ExecuteReader();
            }
            catch (Exception e123)
            {
                MessageBox.Show(e123.ToString());
            }
          
            listDevices.Items.Clear();

            while (measurementsReader.Read())
            {
                listDevices.Items.Add(measurementsReader.GetString(0));
                listDevices.SelectedItems.Add(measurementsReader.GetString(0));
            }

                       
            measurementsReader.Close();//Close the reader since we are opening it every time when selected item
            CreateGraph(zg1);
           
        }
        //Create the Graph and configure is proprties
        private void CreateGraph(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;
          
            // Set the titles and axis labels per selection
            myPane.Title.Text = cmbBoxThresholdTypes.GetItemText(cmbBoxThresholdTypes.SelectedItem);
            // Change the color of the title
         //   myPane.Title.FontSpec.Size = 10.0f * (this.Size.Width / 100); 

            myPane.XAxis.Title.Text = "Time (Sec)";
            myPane.YAxis.Title.Text = "Threshold";

            myPane.CurveList.Clear();// clear the graph

            XDate minDate = XDate.JulDayMax;
            XDate maxDate = XDate.JulDayMin;
            //Create Random colors to show on Graph
            Random randGenerator = new Random();
            foreach (object device in listDevices.SelectedItems)
            {
                String currDeviceName = device.ToString();

                PointPairList listDeviceValues = GetValuesForDevice(currDeviceName, GetSelectedThresholdId());

                int r = (int)(randGenerator.NextDouble() * 255);
                int g = (int)(randGenerator.NextDouble() * 255);
                int b = (int)(randGenerator.NextDouble() * 255);
                
                //use this to add line width 3.0F
                LineItem myCurve = new LineItem(currDeviceName, listDeviceValues, Color.FromArgb(255, r, g, b), SymbolType.XCross);
                myPane.CurveList.Add(myCurve);
                
            //    myPane.CurveList.Add(new BarItem(currDeviceName, listDeviceValues, Color.FromArgb(255, r, g, b)));
             
                if ( listDeviceValues.Count > 0 )
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
            }

            Int32 thresholdValue = GetContractThreshold();//Read the Threshold values
            PointPairList thresholdPointList = new PointPairList();
            thresholdPointList.Add(new PointPair(minDate, thresholdValue));
            thresholdPointList.Add(new PointPair(maxDate, thresholdValue));

            myPane.CurveList.Insert(0,new LineItem("Threshold", thresholdPointList, Color.FromArgb(255, 0, 0, 0), SymbolType.XCross, 3.0f));

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

        // used for getting threshold value from Contracts
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
            DataRowView dataRow = (DataRowView)cmbBoxThresholdTypes.SelectedItem;

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
        //will give the step size for the Xaxis
        protected double CalcStepSize(double range, double targetSteps)
        {
            // Calculate an initial guess at step size
            double tempStep = range / targetSteps;

            // Get the magnitude of the step size
            double mag = Math.Floor(Math.Log10(tempStep));
            double magPow = Math.Pow((double)10.0, mag);

            // Calculate most significant digit of the new step size
            double magMsd = ((int)(tempStep / magPow + .5));

            // promote the MSD to either 1, 2, or 5
            if (magMsd > 5.0)
                magMsd = 10.0;
            else if (magMsd > 2.0)
                magMsd = 5.0;
            else if (magMsd > 1.0)
                magMsd = 2.0;

            return magMsd * magPow;
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
