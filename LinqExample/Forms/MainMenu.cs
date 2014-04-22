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
            this.FormClosing +=  MainMenu_FormClosing;
           
           
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
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

        private void button3_Click(object sender, EventArgs e)
        {
            SLAManager manager = new SLAManager();
            manager.FormClosed += new FormClosedEventHandler(child2_FormClosed);  //add handler to catch when child form is closed
            manager.Show(); //show child
            this.Hide(); //hide parent
        }
        void child2_FormClosed(object sender, FormClosedEventArgs e)//handles the forms
        {
            //when child form is closed, the parent reappears
            this.Show();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
         
        }

       


        private void MainMenu_Load(object sender, EventArgs e)
        {
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SimulatedDataLoader frmDataLoader = new SimulatedDataLoader();
            frmDataLoader.FormClosed += new FormClosedEventHandler(child2_FormClosed);  //add handler to catch when child form is closed
            frmDataLoader.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnOpenGraphForm_Click(object sender, EventArgs e)
        {
            Graphs frmDataLoader = new Graphs();
            frmDataLoader.FormClosed += new FormClosedEventHandler(child2_FormClosed);  //add handler to catch when child form is closed
            frmDataLoader.Show(); //show child
            this.Hide(); //hide parent
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard frmDataLoader = new Dashboard();
            frmDataLoader.FormClosed += new FormClosedEventHandler(child2_FormClosed);  //add handler to catch when child form is closed
            frmDataLoader.Show(); //show child
            this.Hide(); //hide parent
        }

        private void SlaManager_Click(object sender, EventArgs e)
        {
            SLAManagerContract managerContract = new SLAManagerContract();
            managerContract.FormClosed += new FormClosedEventHandler(child2_FormClosed);  //add handler to catch when child form is closed
            managerContract.Show(); //show child
            this.Hide(); //hide parent
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReportsByTime reportByTime = new ReportsByTime();
            reportByTime.FormClosed += new FormClosedEventHandler(child2_FormClosed);  //add handler to catch when child form is closed
            reportByTime.Show(); //show child
            this.Hide(); //hide parent
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PredictionReport reportByTime = new PredictionReport();
            reportByTime.FormClosed += new FormClosedEventHandler(child2_FormClosed);  //add handler to catch when child form is closed
            reportByTime.Show(); //show child
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


     
       

      

    }
}
