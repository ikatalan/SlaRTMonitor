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
        private SqlConnection dbConnection2;//Open connections forSlaContracts table
        private SqlDataAdapter measurmentsAdapter;//used for filling list of items (device_name) per threshold_id      
        private SqlDataAdapter measurmentsValuesAdapter; // used for getting measurements valus for a specific device.
        private SqlDataAdapter singleThresholdValueAdapter;// used for getting threshold value from Contracts
        private LineItem myCurve;

        public Graphs()
        {
            InitializeComponent();
            myPane = zg1.GraphPane;//connect the control to the pane
        }
        GraphPane myPane; 

        //Run when the Graph load
        private void Graphs_Load(object sender, EventArgs e)
        {
            dbConnection = new global::System.Data.SqlClient.SqlConnection();
            dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            dbConnection2 = new global::System.Data.SqlClient.SqlConnection();
            dbConnection2.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;


            this.thresholdsTableAdapter.Fill(this.sLA_RT_monitoringDataSetThreshold.Thresholds);

            // Used for filling list of items (device_name) per threshold_id            
            measurmentsAdapter = new SqlDataAdapter(
                @"SELECT device_name FROM [dbo].[SimulatedMeasurements] "
                    + @"WHERE threshold_id=@threshold_id "
                    + @"GROUP BY device_name;",
                dbConnection);


            if (((measurmentsAdapter.SelectCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                measurmentsAdapter.SelectCommand.Connection.Open();
            }

            measurmentsAdapter.SelectCommand.CommandType = global::System.Data.CommandType.Text;
            measurmentsAdapter.SelectCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_name", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));



            // used for getting measurements valus for a specific device.
            measurmentsValuesAdapter = new SqlDataAdapter(
                @"SELECT timestamp, value  "
                + @" FROM [dbo].[SimulatedMeasurements] "
                + @" WHERE device_name=@device_name "
                + @" ORDER BY timestamp;",
                dbConnection);
            
            if (((measurmentsValuesAdapter.SelectCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                measurmentsValuesAdapter.SelectCommand.Connection.Open();
            }
            

            measurmentsValuesAdapter.SelectCommand.CommandType = global::System.Data.CommandType.Text;
            measurmentsValuesAdapter.SelectCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_name", global::System.Data.SqlDbType.NChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_name", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));



            // used for getting threshold value from Contracts
            singleThresholdValueAdapter = new SqlDataAdapter(
                @"SELECT value "
                + @"FROM [dbo].[SlaContracts] "
                + @"WHERE threshold_id=@threshold_id;",
                dbConnection2);


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

            measurmentsAdapter.SelectCommand.Parameters[0].Value = selectedRow.id;

            global::System.Data.ConnectionState previousConnectionState = measurmentsAdapter.SelectCommand.Connection.State;
            if (((measurmentsAdapter.SelectCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                measurmentsAdapter.SelectCommand.Connection.Open();
            }
               
             measurementsReader = measurmentsAdapter.SelectCommand.ExecuteReader();
          
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
          //  myPane.Title.FontSpec.FontColor = Color.Blue;

            myPane.XAxis.Title.Text = "Time (Sec)";
            myPane.YAxis.Title.Text = "Threshold";

            myPane.CurveList.Clear();// clear the graph

            Int32 thresholdValue = GetThreshold();//Read the Threshold values
            PointPairList thresholdPointList = new PointPairList();
            thresholdPointList.Add(new PointPair(myPane.XAxis.Scale.Max, thresholdValue));
            myPane.AddCurve(cmbBoxThresholdTypes.SelectedText, thresholdPointList, Color.FromArgb(255, 0, 0, 0), SymbolType.XCross);

            //Create Random colors to show on Graph
            Random randGenerator = new Random();
            foreach (object device in listDevices.SelectedItems)
            {
                String currDeviceName = (String)device;

                PointPairList listDeviceValues = GetValuesForDevice(currDeviceName);

                int r = (int)(randGenerator.NextDouble() * 255);
                int g = (int)(randGenerator.NextDouble() * 255);
                int b = (int)(randGenerator.NextDouble() * 255);
                
                //use this to add line width 3.0F
                myCurve = new LineItem(currDeviceName, listDeviceValues, Color.FromArgb(255, r, g, b), SymbolType.XCross, 3.0f);
                myPane.CurveList.Add(myCurve);
             //   myCurve = myPane.AddCurve(currDeviceName, listDeviceValues, Color.FromArgb(255,r,g,b), SymbolType.XCross);


                // Fill the area under the curve with a white-red gradient at 45 degrees
                //myCurve.Line.Fill = new Fill(Color.White, Color.Red, 45F);
                // Make the symbols opaque by filling them with white
                //myCurve.Symbol.Fill = new Fill(Color.White);
            }

          
          
            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);

            // Fill the pane background with a color gradient
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            // Add a text box with instructions
            //TextObj text = new TextObj(
            //    "Zoom: left mouse & drag\nPan: middle mouse & drag\nContext Menu: right mouse",
            //    0.05f, 0.95f, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom);
            //text.FontSpec.StringAlignment = StringAlignment.Near;

            //myPane.GraphObjList.Add(text);

            //This informs ZedGraph to use the labels supplied by the user in Axis.Scale.TextLabels
             Axis.Default.Type = AxisType.Text;

            //Show tooltips when the mouse hovers over a point
            zgc.IsShowPointValues = true;
            zgc.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);


            
            // Set the XAxis to date type
            myPane.XAxis.Type = AxisType.Date;
            // Manually set the x axis range
        //    myPane.XAxis.Scale.Min = 0;
        //    myPane.XAxis.Scale.Max = 800;
            // Display the Y axis grid lines
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.IsVisible = true;
          //  myPane.XAxis.Scale.MinorStep = 1.0;
            //myPane.XAxis.Scale.MajorStep = 60.0;
           // myPane.XAxis.Scale.MinorUnit = DateUnit.Second;
           myPane.XAxis.Scale.MajorUnit = DateUnit.Second;


        //    XAxis.Scale.MinAuto=true;
          //  XAxis.Scale.MajorStepAuto=true;
            //XAxis.Scale.MaxAuto = true;

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
            return curve.Label.Text + " is " + pt.Y.ToString("f2") + " units at Time: " + pt.X.ToString("f1") + " ";
           
        }

        // used for getting threshold value from Contracts
        private Int32 GetThreshold()
        {
            DataRowView dataRow = (DataRowView)cmbBoxThresholdTypes.SelectedItem;

            LinqExample.SLA_RT_monitoringDataSetThreshold.ThresholdsRow selectedRow =
                (LinqExample.SLA_RT_monitoringDataSetThreshold.ThresholdsRow)dataRow.Row;

            singleThresholdValueAdapter.SelectCommand.Parameters[0].Value = selectedRow.id;

            global::System.Data.ConnectionState previousConnectionState = singleThresholdValueAdapter.SelectCommand.Connection.State;
            if (((singleThresholdValueAdapter.SelectCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                singleThresholdValueAdapter.SelectCommand.Connection.Open();
            }


            return (Int32)singleThresholdValueAdapter.SelectCommand.ExecuteScalar(); 
        }

        //
        private PointPairList GetValuesForDevice(String deviceName)
        {
            
            PointPairList listValues = new PointPairList();
            try
            {
                SqlDataReader measurementsReader = null;

                measurmentsValuesAdapter.SelectCommand.Parameters[0].Value = deviceName;

                global::System.Data.ConnectionState previousConnectionState = measurmentsValuesAdapter.SelectCommand.Connection.State;
                if (((measurmentsValuesAdapter.SelectCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    measurmentsValuesAdapter.SelectCommand.Connection.Open();
                }

                measurementsReader = measurmentsValuesAdapter.SelectCommand.ExecuteReader();

                
                while (measurementsReader.Read())
                {
               
               //     DateTime oaBaseDate = measurementsReader.GetDateTime(0).ToUniversalTime();
               //     double result = oaBaseDate.Add(DateTime.Now.TimeOfDay).ToOADate();
                  
                  // var y = measurementsReader.GetInt32(1);
                //    var y = DateTime.Parse(DateTime.FromOADate(measurementsReader.GetDateTime(0).ToOADate()).ToString());
                    double x = (double)new XDate(measurementsReader.GetDateTime(0));
                    listValues.Add(
                      //x,
                      CalcStepSize(x,200000),
                       // measurementsReader.GetDateTime(0).ToOADate(),
                     // (double.(measurementsReader.GetDateTime(0).ToUniversalTime()),//timestemp
                        measurementsReader.GetInt32(1) //value
                    );
                }

                measurementsReader.Close();
            }
            catch (Exception e)
            {
                
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
