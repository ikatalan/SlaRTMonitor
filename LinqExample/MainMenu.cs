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
            this.Closing += new CancelEventHandler(this.Form1_Closing);
           
          
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

        private void Form1_Closing(Object sender, CancelEventArgs e)
        {
            MessageBox.Show("Goodbye....Closing");
            this.Hide();
                  
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
            
            //SLAManager manager = new SLAManager();
            //manager.FormClosed += new FormClosedEventHandler(child2_FormClosed);  //add handler to catch when child form is closed
            //manager.Show(); //show child
            //this.Hide(); //hide parent

        }
       

      

    }
}
