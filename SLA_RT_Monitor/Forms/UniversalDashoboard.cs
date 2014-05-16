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


            //Init Deveices panel by type.
            UniversalDashboardDeviceType x;

            x = new UniversalDashboardDeviceType("Printer");
            dashboardElement.Add("Printer", x);

            x = new UniversalDashboardDeviceType("Network");
            dashboardElement.Add("Network", x);

            x = new UniversalDashboardDeviceType("Storage");
            dashboardElement.Add("Storage", x);

            x = new UniversalDashboardDeviceType("Server");
            dashboardElement.Add("Server", x);
            
            int i = 0; 

            //Place dynamically the panels on form.
            foreach (UniversalDashboardDeviceType deviceTypeControl in dashboardElement.Values)
            {
                deviceTypeControl.Location = new Point(20, (int)(45 + 110 * (i * 1.2)));
                deviceTypeControl.Size = new Size(this.Size.Width - 60, 110);
                 
                deviceTypeControl.Anchor = (AnchorStyles) AnchorStyles.Left | AnchorStyles.Right;
                deviceTypeControl.BorderStyle = BorderStyle.FixedSingle;
                deviceTypeControl.MouseLeave += deviceTypeControl_MouseLeave;
                deviceTypeControl.MouseEnter += deviceTypeControl_MouseEnter;

                this.Controls.Add(deviceTypeControl);
                ++i;
            }
        }

        void deviceTypeControl_MouseEnter(object sender, EventArgs e)
        {
            //high-light on mouse enter
            UniversalDashboardDeviceType deviceTypeControl = (UniversalDashboardDeviceType)sender;
            deviceTypeControl.BackColor = Color.Beige;
        }

        void deviceTypeControl_MouseLeave(object sender, EventArgs e)
        {
            //!high-light on mouse leave
            UniversalDashboardDeviceType deviceTypeControl = (UniversalDashboardDeviceType)sender;
            deviceTypeControl.BackColor = Color.FromArgb(255, 240, 240, 240);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
