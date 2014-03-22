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

namespace LinqExample
{
    public partial class Dashboard : Form
    {

        private SqlConnection dbConnection;//Open connections for devices table
        private SqlCommand devicesMeasurmentsCommand;

        private Thread fetcherThread;

        private int deviceId;
        bool shouldContinue;


        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetGuageValueCallback(AGauge currGuage, Label currLabel, string thresholdName, float value);
        

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
                @"SELECT DISTINCT  a.threshold_id, a.value, a.timestamp, b.minValue, b.maxValue, b.name FROM [dbo].[SimulatedMeasurements] a "
                + @"JOIN [dbo].[Thresholds] b ON a.threshold_id=b.id " 
                + @"WHERE device_id= @device_id "
                + @"ORDER BY timestamp  ",
                dbConnection);


            if (((devicesMeasurmentsCommand.Connection.State & global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                devicesMeasurmentsCommand.Connection.Open();
            }

            devicesMeasurmentsCommand.CommandType = global::System.Data.CommandType.Text;
            devicesMeasurmentsCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));


            try
            {
                deviceId = Int32.Parse(((DataRowView)listDevices.SelectedItem)[0].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            deviceId = Int32.Parse(((DataRowView)listDevices.SelectedItem)[0].ToString()); 
        }



        static private void GuageDataFetcher(object arg) 
        {
            Dashboard me = (Dashboard)arg;

            while (me.shouldContinue)
            {
                int deviceId = me.deviceId;

                me.devicesMeasurmentsCommand.Parameters["@device_id"].Value = deviceId;

                SqlDataReader reader =  me.devicesMeasurmentsCommand.ExecuteReader();
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
                    value = value+ (((float)thresholdValue)/(maxValue-minValue) * 100);

                    switch (idx)
                    {
                        case 1:
                        {
                            me.SetGuageValue(me.gauge1, me.lblGuage1, thresholdName, value);
                            
                        } break;
                        case 2:
                        {
                            me.SetGuageValue(me.gauge2, me.lblGuage2, thresholdName, value);
                        } break;
                        case 3:
                        {
                            me.SetGuageValue(me.gauge3, me.lblGuage3, thresholdName, value);
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

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            shouldContinue = false;
        }


        private void SetGuageValue(AGauge currGuage, Label currLabel, string thresholdName, float value)
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
