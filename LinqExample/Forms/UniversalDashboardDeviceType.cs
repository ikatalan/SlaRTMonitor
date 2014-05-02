using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinqExample.Forms
{
    public partial class UniversalDashboardDeviceType : UserControl
    {
        public UniversalDashboardDeviceType()
        {
            InitializeComponent();
        }

        public UniversalDashboardDeviceType(string deviceType)
        {
            InitializeComponent();
            this.DeviceTypeText = deviceType;
        }

        private class SingleDeviceData
        {
            public SingleDeviceData(string _deviceName)
            {
                this.deviceName = _deviceName;
            }
            public string deviceName;
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

        private SingleDeviceData[] devices = { 
            new SingleDeviceData("printer 1"), 
            new SingleDeviceData("printer 2"), 
            new SingleDeviceData("printer 3") 
        };

        private void UniversalDashboardDeviceType_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < devices.Count(); ++i)
            {
                Label x = new Label();
                
                x.Location = new Point((int)(40 + 65 * (i*1.2)) ,60 );
                x.Size = new System.Drawing.Size(60, 60);

                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, x.Width, x.Height);

                x.Region = new Region(path);
                x.BackColor = Color.Red;
                x.TextAlign = ContentAlignment.MiddleCenter;
                x.Text = devices[i].deviceName;
      
                this.Controls.Add(x);
            }
        }

    }
}
