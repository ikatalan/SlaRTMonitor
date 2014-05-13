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
        DashboardIncidentsProvider incidentsProvider;
        Timer updateTimer;

        public UniversalDashboardDeviceType()
        {
            InitializeComponent();
        }

        public UniversalDashboardDeviceType(string deviceType)
        {
            InitializeComponent();
            this.DeviceTypeText = deviceType;
            incidentsProvider = new DashboardIncidentsProvider();
            updateTimer = new Timer();
            updateTimer.Interval = 1000;
            updateTimer.Tick += updateTimer_Tick;
            
        }

        void updateTimer_Tick(object sender, EventArgs e)
        {
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

        List<int> listThresholds = new List<int>();

        private void UniversalDashboardDeviceType_Load(object sender, EventArgs e)
        {

            SqlConnection dbConnection4;//Open connections for devices table
            SqlCommand contractIdCommand;
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

            while (reader2.Read())
            {
                listThresholds.Add(reader2.GetInt32(0));
            }

            reader2.Close();//close the reader when not using.
            SqlConnection dbConnection;//Open connections for devices table
            SqlCommand deviceByTypeCommand;

            //Open connections for devices table
            dbConnection = new global::System.Data.SqlClient.SqlConnection();
            dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

            // Used for having all the information per Device ID       
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
                SingleDeviceData newDeviceData = new SingleDeviceData(reader.GetInt32(0), reader.GetString(1));

                Label x = new Label();
                
                x.Location = new Point((int)(40 + 65 * (i*1.2)), 35 );
                x.Size = new System.Drawing.Size(60, 60);

                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, x.Width, x.Height);

                x.Region = new Region(path);
                x.BackColor = GetColorByIncidentsNumber(newDeviceData.deviceId);
                x.TextAlign = ContentAlignment.TopCenter;
              //  x.Text = newDeviceData.deviceName;
                x.Text += SplitName(newDeviceData.deviceName)[0];
                x.Text += "\n";
                x.Text += SplitName(newDeviceData.deviceName)[1];
                x.Text += "\n";
                x.TextAlign = ContentAlignment.MiddleCenter;
                //x.Text += SplitName(newDeviceData.deviceName)[2];
               
                x.Tag = newDeviceData;
                x.Cursor = Cursors.Hand;
                x.Click += x_Click;

                this.Controls.Add(x);
                ++i;
            }

            updateTimer.Start();

        }
        
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
   
        private Color GetColorByIncidentsNumber(int deviceId)
        {
            DateTime lastHour = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
            int summary = 0;
            foreach (int thresholdId in listThresholds)
            {
                summary += incidentsProvider.GetIncedentsFor(thresholdId, deviceId, lastHour);
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

        void x_Click(object sender, EventArgs e)
        {
            SingleDeviceData clickedDevice = (SingleDeviceData)((Label)sender).Tag;

            Dashboard dashboard = new Dashboard(clickedDevice.deviceId);
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
