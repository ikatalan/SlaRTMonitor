using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LinqExample.Forms
{
    public partial class UniversalDashboardDeviceType : UserControl
    {
        // used to hold the device data in the Tag property of a Label
        private class SingleDeviceData
        {
            public SingleDeviceData(int p1, string p2)
            {
                this.deviceId = p1;
                this.deviceName = p2;
            }

            public string deviceName;
            public int deviceId;

        }

        DashboardIncidentsProvider incidentsProvider;
        Timer updateTimer;
        List<int> listThresholds = new List<int>();

        public UniversalDashboardDeviceType()
        {
            InitializeComponent();
        }

        public UniversalDashboardDeviceType(string deviceType)
        {
            InitializeComponent();
            this.DeviceTypeText = deviceType;
            //Init incidents provider to show back color for device.
            incidentsProvider = new DashboardIncidentsProvider();

            //Init timer for incidents repeated checks.
            updateTimer = new Timer();
            updateTimer.Interval = 1000;
            updateTimer.Tick += updateTimer_Tick;
            
        }

        void updateTimer_Tick(object sender, EventArgs e)
        {
            //Update the device color by rereading the incidents.
            foreach (Control currControl in this.Controls)
            {
                if (currControl is Label)
                {
                    Label currLabel = (Label)currControl;

                    if (currLabel.Tag is SingleDeviceData)
                    {
                        SingleDeviceData data = (SingleDeviceData)currLabel.Tag;
                        currLabel.BackColor = GetColorByIncidentsNumber(data.deviceId);
                    }
                }
            }
            
        }

        public string DeviceTypeText
        {
            get
            {
                return lblDeviceType.Text;
            }
            set
            {
                lblDeviceType.Text = value;
            }
        }


        private void UniversalDashboardDeviceType_Load(object sender, EventArgs e)
        {

            SqlConnection dbConnection4; //Connection for contractIdCommand
            SqlCommand contractIdCommand; // Select query to get thresholds by deviceType.
            {
                //Open contract id for deviceTypeControl and thresholdId
                dbConnection4 = new global::System.Data.SqlClient.SqlConnection();
                dbConnection4.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for having contract ID per  threshold_id and  device_type          
                contractIdCommand = new SqlCommand(
                    @"SELECT threshold_id "
                    + @"FROM [dbo].[SlaContracts] "
                    + @"WHERE device_type=@device_type ",
                    dbConnection4);

                if (((contractIdCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    contractIdCommand.Connection.Open();
                }

                contractIdCommand.CommandType = global::System.Data.CommandType.Text;
                contractIdCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_type", global::System.Data.SqlDbType.NChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_type", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                contractIdCommand.Parameters["@device_type"].Value = this.DeviceTypeText;
            }

            SqlDataReader reader2 = contractIdCommand.ExecuteReader();

            // Add thresholdIds to the list of matching trhresholds.
            while (reader2.Read())
            {
                listThresholds.Add(reader2.GetInt32(0));
            }

            reader2.Close();//close the reader when not using.
            SqlConnection dbConnection; // connections for deviceByTypeCommand
            SqlCommand deviceByTypeCommand; // select id and name from Devices table by deviceType.

          
            dbConnection = new global::System.Data.SqlClient.SqlConnection();
            dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            // Used for having all the information per Device type       
            deviceByTypeCommand = new SqlCommand(
                @"SELECT id, name "
                + @"FROM [dbo].[Devices] "
                + @"WHERE type = @device_type ",
                dbConnection);

            global::System.Data.ConnectionState previousConnectionState = deviceByTypeCommand.Connection.State;
            if (((deviceByTypeCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                deviceByTypeCommand.Connection.Open();
            }

            deviceByTypeCommand.CommandType = global::System.Data.CommandType.Text;
            deviceByTypeCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_type", global::System.Data.SqlDbType.NChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_type", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));

            deviceByTypeCommand.Parameters["@device_type"].Value = this.DeviceTypeText;
            SqlDataReader reader = deviceByTypeCommand.ExecuteReader();

            int i = 0;
            while (reader.Read())
            {
                // Init deviceData with query result.
                SingleDeviceData newDeviceData = new SingleDeviceData(reader.GetInt32(0), reader.GetString(1));

                //Create a label for this device.
                Label deviceLabel = new Label();
                
                // arrange labels in a row. Left to right.
                deviceLabel.Location = new Point((int)(40 + 65 * (i*1.2)), 35 );
                deviceLabel.Size = new System.Drawing.Size(60, 60);

                // Change the shape of the label to a circle.
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, deviceLabel.Width, deviceLabel.Height);
                deviceLabel.Region = new Region(path);
                
                // get the needed backcolor by ioncidents data
                deviceLabel.BackColor = GetColorByIncidentsNumber(newDeviceData.deviceId);
                deviceLabel.TextAlign = ContentAlignment.TopCenter;

                // Put the text on the label.
                deviceLabel.Text += SplitName(newDeviceData.deviceName)[0];
                deviceLabel.Text += "\n";
                deviceLabel.Text += SplitName(newDeviceData.deviceName)[1];
                deviceLabel.Text += "\n";
                deviceLabel.TextAlign = ContentAlignment.MiddleCenter;
                if (SplitName(newDeviceData.deviceName)[2] != null)
                {
                    deviceLabel.Text += SplitName(newDeviceData.deviceName)[2];
                }

                // Insert device data in the Tag property ( for using on Click event and updateTimer).
                deviceLabel.Tag = newDeviceData;
                deviceLabel.Cursor = Cursors.Hand;
                deviceLabel.Click += x_Click;

                //Add to form controls.
                this.Controls.Add(deviceLabel);
                ++i;
            }

            //Update incidents every interval.
            updateTimer.Start();

        }
        
        // Split device name with seperator  ' '.
        private List<string> SplitName(string deviceName)
        {
            
            int maxCharsInLine = 5; //fit name in one line
            string[] words = deviceName.Split(' '); // Split the text into words
            List<string> result = new List<string>();

            result.Add(words[0]);
            for (int i = 1; i < words.Length; i++)
            {
                // If the previous word and the current word (plus whitespace) do not exceed the limitation
                if (result[result.Count - 1].Length + 1 + words[i].Length <= maxCharsInLine)
                    result[result.Count - 1] += "  " + words[i]; // Append the current word to the previous one
                else
                    result.Add(words[i]); // Put the current word to a new line
            }
          
            return result;

        }
   
        // map incedents number to a specific color.
        private Color GetColorByIncidentsNumber(int deviceId)
        {
            DateTime startIncidentsFrom = DateTime.Now.Subtract(new TimeSpan(0, 30, 0));
            int summary = 0;
            foreach (int thresholdId in listThresholds)
            {
                summary += incidentsProvider.GetIncedentsFor(thresholdId, deviceId, startIncidentsFrom);
            }

            if (summary > 5)
            {
                return Color.Red;
            }
            else if (summary > 0)
            {
                return Color.Yellow;
            }
            else
            {
                return Color.Green;
            }

        }

        // On click open the Dashboard with a specific device selected.
        void x_Click(object sender, EventArgs e)
        {
            SingleDeviceData clickedDevice = (SingleDeviceData)((Label)sender).Tag;

            Dashboard dashboard = new Dashboard(clickedDevice.deviceId); //Create the next form Dashoboard. with preselected deviceId.
            dashboard.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            dashboard.Show(); //show child
            this.Parent.Hide(); //hide parent
        }


        void child_FormClosed(object sender, FormClosedEventArgs e)//handles the forms
        {
            this.Parent.Show();
        }
    }
}
