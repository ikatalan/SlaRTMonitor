using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinqExample.Forms
{
    public partial class UniversalDashoboard : Form
    {
        public Dictionary<string, UniversalDashboardDeviceType> dashboardElement;

        public UniversalDashoboard()
        {
            InitializeComponent();

            dashboardElement = new Dictionary<string, UniversalDashboardDeviceType>();

            UniversalDashboardDeviceType x;

            x = new UniversalDashboardDeviceType("Printers");
            dashboardElement.Add("Printers", x);

            x = new UniversalDashboardDeviceType("Network");
            dashboardElement.Add("Network", x);

            x = new UniversalDashboardDeviceType("Storage");
            dashboardElement.Add("Storage", x);

            int i = 0; 
            foreach (UniversalDashboardDeviceType deviceType in dashboardElement.Values)
            {
                deviceType.Location = new Point(20, (int)(60 + 120 * (i * 1.2)));
                deviceType.Size = new Size(this.Size.Width, 120);

                this.Controls.Add(deviceType);


                ++i;
            }

        }

        private void UniversalDashoboard_Load(object sender, EventArgs e)
        {

        }
    }
}
