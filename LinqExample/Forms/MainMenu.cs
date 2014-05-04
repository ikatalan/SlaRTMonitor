﻿using LinqExample.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LinqExample
{
    public partial class MainMenu : Form
    {

        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

            if (SingletoneUser.Role != 0)
            {
                button1.Enabled = false;
            }

        }

        void child_FormClosed(object sender, FormClosedEventArgs e)//handles the forms
        {
            //when child form is closed, the parent reappears
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            UserManagement usrManagement = new UserManagement();
            usrManagement.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            usrManagement.Show(); //show child
            this.Hide(); //hide parent
        }


        
        private void button5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SimulatedDataLoader frmDataLoader = new SimulatedDataLoader();
            frmDataLoader.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            frmDataLoader.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnOpenGraphForm_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Graphs frmDataLoader = new Graphs();
            frmDataLoader.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            frmDataLoader.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Dashboard frmDataLoader = new Dashboard();
            frmDataLoader.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            frmDataLoader.Show(); //show child
            this.Hide(); //hide parent
        }

        private void SlaManager_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SLAManagerContract managerContract = new SLAManagerContract();
            managerContract.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            managerContract.Show(); //show child
            this.Hide(); //hide parent
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ReportsByTime reportByTime = new ReportsByTime();
            reportByTime.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            reportByTime.Show(); //show child
            this.Hide(); //hide parent
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PredictionReport predictionReport = new PredictionReport();
            predictionReport.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            predictionReport.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnSlaComparison_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SLAComparison slaComparison = new SLAComparison();
            slaComparison.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            slaComparison.Show(); //show child
            this.Hide(); //hide parent

        }

        private void btnUniversalDashboard_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            UniversalDashoboard dashboard = new UniversalDashoboard();
            dashboard.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed
            dashboard.Show(); //show child
            this.Hide(); //hide parent
        }

        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{

        //    base.OnFormClosing(e);

        //    if (e.CloseReason == CloseReason.WindowsShutDown) return;

        //    // Confirm user wants to close
        //    switch (MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
        //    {
        //        case DialogResult.No:
        //            e.Cancel = true;
        //            break;
        //        default:
        //            e.Cancel = false;
        //            break;

        //    }

        //}

        //Need to fix 
        //FormClosingEventArgs e
        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Display a MsgBox asking the user to save changes or abort. 
            if (MessageBox.Show("Are you sure you want to close?", "Closing",
               MessageBoxButtons.YesNo) == DialogResult.No)
            {
                // Cancel the Closing event from closing the form.
                e.Cancel = true;
                // Call method to save file...
            }
            



            //In case windows is trying to shut down, don't hold the process up
       //     if (e.CloseReason == CloseReason.WindowsShutDown) return;

      
            //    // Assume that X has been clicked and act accordingly.
            //    // Confirm user wants to close
            //switch (MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            //    {
            //        //Stay on this form
            //        case DialogResult.No:
            //            e.Cancel = true;
            //            break;
            //        default:
            //            break;
            //    }
           

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            SingletoneUser.UserPass = null;
            SingletoneUser.UserName = null;
            this.Close();
        }

 
    }
}
