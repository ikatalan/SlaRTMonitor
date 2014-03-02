using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;//SqL
using System.Drawing;
using System.Linq;//Linq
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Media; //for sound

namespace LinqExample
{
    public partial class UserManagement : Form
    {
        private System.Timers.Timer closeTimer = new System.Timers.Timer(2000) { AutoReset = false };
        public UserManagement()
        {
            InitializeComponent();

            
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sLA_RT_monitoringDataSet.Users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.sLA_RT_monitoringDataSet.Users);

        }
              

        void closeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!btnSave.Enabled)
                btnSave.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
        //Update/Save
        private void button1_Click(object sender, EventArgs e)
        {

            btnSave.Enabled = false;
            try
            {
                //Need to md5 the password
                this.Validate();
                this.usersBindingSource.EndEdit();
                this.usersTableAdapter.Update(this.sLA_RT_monitoringDataSet.Users);
                SystemSounds.Hand.Play();
                MessageBox.Show("Update successful");
            }
            catch (System.Exception )
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("Update failed");
            }
            btnSave.Enabled = true;   
  
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

            MessageBox.Show(this.dataGridView1.Columns[e.ColumnIndex].HeaderText);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

        //Delete row
        private void button1_Click_1(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)usersBindingSource.Current; //Get the current row view
            DataRow dr = drv.Row;  //Get it out of the Row View so I can grab an array
            dr.Delete();
            this.usersTableAdapter.Update(this.sLA_RT_monitoringDataSet.Users);
            System.Windows.Forms.MessageBox.Show("Line Deleted From Database");
           
        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.usersTableAdapter.FillBy(this.sLA_RT_monitoringDataSet.Users);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void usersBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

       
    }
}
