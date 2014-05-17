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
                MessageBox.Show("Update Successfully", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception )
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("Update failed", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
            btnSave.Enabled = true;   
  
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

           // MessageBox.Show(this.dataGridView1.Columns[e.ColumnIndex].HeaderText);
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
            MessageBox.Show("Delete Successfully", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
           
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

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[3].Value = "Viewer";

        }
        public string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        public string base64Decode2(string sData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(sData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (this.dataGridView1.Columns[e.ColumnIndex].HeaderText == "Password")
            //{
            //    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            //    {
            //        string str = base64Encode(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            //        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = str;
            //        this.usersTableAdapter.Update(this.sLA_RT_monitoringDataSet.Users);
            //        //MessageBox.Show(str);
            //    }


            //}

        }

       
    }
}
