using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;

//Handle the SLA contract
namespace LinqExample
{   
    public partial class SLAManagerContract : Form
    {

        Dictionary<int, String> thresholdIdToName;
        SqlConnection thresholdConnection;
        SqlCommand allthreshold;

        static int OldContractExist = 0;
        private bool AlreadySaved =false;
        private bool changed=false;
        public SLAManagerContract()
        {
            changed = false;
            InitializeComponent();
        }

        private void SLAManagerContract_Load(object sender, EventArgs e)
        {
            changed = false;
            this.slaContractsTableAdapter.Fill(this.sLA_RT_monitoringDataSetSlaContracts.SlaContracts);
            //-----------------Need to find the Threshold name per threshold ID 
            thresholdConnection = new SqlConnection(global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString);
            allthreshold = new SqlCommand("SELECT id, name FROM [dbo].[Thresholds]", thresholdConnection);

            if (((allthreshold.Connection.State & global::System.Data.ConnectionState.Open)
             != global::System.Data.ConnectionState.Open))
            {
                allthreshold.Connection.Open();
            }

            SqlDataReader thresholdReader = allthreshold.ExecuteReader();

            thresholdIdToName = new Dictionary<int, string>();

            while (thresholdReader.Read())
            {
                thresholdIdToName[thresholdReader.GetInt32(0)] = thresholdReader.GetString(1);
                int typeId = thresholdReader.GetInt32(0);
                string typeName = thresholdReader.GetString(1);

                thresholdIdToName[typeId] = typeName;
            }

            //----------------

            if (dataGridViewSLAManger.Rows.Count >= 1) { OldContractExist = 1; }// need to know if we have contract in the database

            BindingSource SBind = new BindingSource();
            SBind.DataSource = this.sLA_RT_monitoringDataSetSlaContracts.SlaContracts;

            dataGridViewSLAManger.AutoGenerateColumns = false;
            dataGridViewSLAManger.DataSource = this.sLA_RT_monitoringDataSetSlaContracts.SlaContracts;

            for (int i = 1; i < dataGridViewSLAManger.ColumnCount; ++i)
            {
                if (i != 3)
                {
                    dataGridViewSLAManger.Columns[i - 1].DataPropertyName = this.sLA_RT_monitoringDataSetSlaContracts.SlaContracts.Columns[i].Caption;
                    //dataGridViewSLAManger.Columns[3].DataPropertyName = thresholdIdToName[];
                   
                }
            }

            dataGridViewSLAManger.DataSource = SBind;
            dataGridViewSLAManger.Refresh();

          
        }

        private void backMainMenu_Click(object sender, EventArgs e)
        {
              
            if (changed && AlreadySaved == false)
            {
                DialogResult result = MessageBox.Show("Update new contract?", "Back to MainMenu", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    OldContractExist = 1;
                    btnSave_Click(null, null);
                    this.Close();
                }
                else if (result == DialogResult.No)
                {
                    this.Close();
                }
                else
                {
                    //Cancel 
                }
            }
            else
            {
                this.Close();
            }  

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dataGridViewSLAManger.AllowUserToAddRows = true;
            dataGridViewSLAManger.AllowUserToDeleteRows = true;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\Users\\%USERNAME%";//Go to Desktop
            openFileDialog1.Filter = "Excel Files(.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable slaData = GetDataTableFromCsv(openFileDialog1.FileName, true);

                    BindingSource SBind = new BindingSource();
                    SBind.DataSource = slaData;

                    dataGridViewSLAManger.AutoGenerateColumns = false;
                    dataGridViewSLAManger.DataSource = slaData;

                    for (int i = 0; i < dataGridViewSLAManger.ColumnCount; ++i)
                    {
                      //  dataGridViewSLAManger.Columns[i].DataPropertyName = slaData.Columns[i - 1].Caption;
                        if (i != 2 && i != 3 )
                        {
                            dataGridViewSLAManger.Columns[i].DataPropertyName = slaData.Columns[i].Caption;
                        }
                        else if (i == 2)
                        {
                            dataGridViewSLAManger.Columns[i+1].DataPropertyName = slaData.Columns[i].Caption;
                        }
                    }

                    dataGridViewSLAManger.DataSource = SBind;
                    dataGridViewSLAManger.Refresh();
                    changed = true;
                    btnSave.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                }
            }

        }
        static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            string header = isFirstRowHeader ? "Yes" : "No";

            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            string sql = @"SELECT * FROM [" + fileName + "]";

            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                      ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            // Open a Confirm Dialog for password protection.
            if (btnUnlock.Text == "Lock")
            {
                dataGridViewSLAManger.AllowUserToAddRows = false;
                dataGridViewSLAManger.AllowUserToDeleteRows = false;
                btnSave.Visible = false;
                btnLoad.Visible = false;

                btnUnlock.Text = "Unlock";
                return;
            }
       
            String input = string.Empty; // will hold the passowrd
            input = "1234";// need to have the specfic user password that login 

            InputBox.InputBoxValidation validation = delegate(string password)
            {
                if (password == "")
                {
                    return "Value cannot be empty.";
                }
                if (password == input)
                {
                   // dataGridViewSLAManger.AllowUserToAddRows = true;
                   // dataGridViewSLAManger.AllowUserToDeleteRows = true;
                    btnSave.Visible = true;
                    btnLoad.Visible = true;
                }
                else
                {
                    return "Wrong passowrd, Try again.";
                }
                return "";
            };

            string value = "";
            if (InputBox.Show("Enter your passowrd", "Password:", ref value, validation) == DialogResult.OK)
            {
                btnUnlock.Text = "Lock";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (OldContractExist == 1) //checks if we didn't have cotract before 
            {
                if (DialogResult.Cancel == MessageBox.Show("Are you sure you want to replace the current SLA contract", "SLA contract change", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    
                    return;
                }
                else
                {
                    MessageBox.Show("New contract updated", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AlreadySaved = true;

                }
            }
            else
            {
                MessageBox.Show("New contract updated", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AlreadySaved = true;
           
            }
        
             LinqExample.SLA_RT_monitoringDataSetSlaContractsTableAdapters.SlaContractsTableAdapter adapter1
               = new SLA_RT_monitoringDataSetSlaContractsTableAdapters.SlaContractsTableAdapter();

            adapter1.DeleteAll();// Delete the old contract before applying new one 

         
             LinqExample.SLA_RT_monitoringDataSetSlaContracts.SlaContractsDataTable s = new SLA_RT_monitoringDataSetSlaContracts.SlaContractsDataTable();


             DataTable data = (DataTable)((BindingSource)this.dataGridViewSLAManger.DataSource).DataSource;
            foreach (DataRow row in data.Rows)
            {
              
                s.AddSlaContractsRow((string)row[0], ((System.Int32)row[1]), ((System.Int32)row[2]));
            
            }

            int rowsAffected = adapter1.Update(s);

            data.AcceptChanges();
            btnSave.Enabled = false;
        }
    }
}
