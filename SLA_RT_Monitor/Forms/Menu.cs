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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            UniversalDashoboard dashboard = new UniversalDashoboard();
            dashboard.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            dashboard.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnGraphs_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Graphs frmDataLoader = new Graphs();
            frmDataLoader.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            frmDataLoader.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnReportsByTime_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ReportsByTime reportByTime = new ReportsByTime();
            reportByTime.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            reportByTime.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnPredictionReport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PredictionReport predictionReport = new PredictionReport();
            predictionReport.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            predictionReport.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnSLAComparison_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SLAComparison slaComparison = new SLAComparison();
            slaComparison.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            slaComparison.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnSLAManager_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SLAManagerContract slaManager = new SLAManagerContract();
            slaManager.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            slaManager.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnUserMnagement_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            UserManagement usrManagement = new UserManagement();
            usrManagement.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            usrManagement.Show(); //show child
            this.Hide(); //hide parent
        }

        void child_FormClosed(object sender, FormClosedEventArgs e)//handles the forms
        {
            //when child form is closed, the parent reappears
            this.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

            if (SingletoneUser.Role != 0)
            {
                btnUserMnagement.Enabled = false;
            }
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
              DialogResult result = MessageBox.Show("Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            SingletoneUser.UserPass = null;
            SingletoneUser.UserName = null;
            this.Close();
        }
    }
}
