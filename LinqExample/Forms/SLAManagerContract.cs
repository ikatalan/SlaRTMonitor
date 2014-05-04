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
        private SqlConnection dbConnection;
        private SqlCommand ThresholdNamePerContractCommand;
        Dictionary<int, String> thresholdIdToName;

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
            btnUnlock.Enabled = (SingletoneUser.Role == 0 ? true : false);
            dataGridViewSLAManger.Enabled = (SingletoneUser.Role == 0 ? true : false);


            changed = false;
            this.slaContractsTableAdapter.Fill(this.sLA_RT_monitoringDataSetSlaContracts.SlaContracts);

          
             //Open contract per threshold per device
               dbConnection = new global::System.Data.SqlClient.SqlConnection();
               dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for having the contract threshold values per contract ID           
                ThresholdNamePerContractCommand = new SqlCommand(
                       @"SELECT id, name " 
                      + @"FROM [SLA_RT_monitoring].[dbo].[Thresholds]" ,
                          dbConnection);


                if (((ThresholdNamePerContractCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    ThresholdNamePerContractCommand.Connection.Open();
                }

                SqlDataReader devicesReader = ThresholdNamePerContractCommand.ExecuteReader();

                thresholdIdToName = new Dictionary<int, string>();

                while (devicesReader.Read())
                {
                    int typeId = devicesReader.GetInt32(0);
                    string typeName = devicesReader.GetString(1);
                    thresholdIdToName[typeId] = typeName;
                }

            if (dataGridViewSLAManger.Rows.Count >= 1) { OldContractExist = 1; } // need to know if we have contract in the database

            BindingSource SBind = new BindingSource();
            SBind.DataSource = GetDataWithThresholdName(this.sLA_RT_monitoringDataSetSlaContracts.SlaContracts);
            dataGridViewSLAManger.DataSource = SBind;
            dataGridViewSLAManger.AutoGenerateColumns = false;
            dataGridViewSLAManger.Refresh();


        }

        private DataTable GetDataWithThresholdName(DataTable slaContractsDataTable)
        {
            if (!slaContractsDataTable.Columns.Contains("threshold_name"))
            {
                slaContractsDataTable.Columns.Add("threshold_name");
            }

            foreach (DataRow row in slaContractsDataTable.Rows)
            {
                row["threshold_name"] = thresholdIdToName[Int32.Parse(row["threshold_id"].ToString())];
            }

            return slaContractsDataTable;

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
                    DataTable slaData = GetDataWithThresholdName(GetDataTableFromCsv(openFileDialog1.FileName, true));

                    BindingSource SBind = new BindingSource();
                    SBind.DataSource = slaData;

                    dataGridViewSLAManger.AutoGenerateColumns = false;
                    dataGridViewSLAManger.DataSource = slaData;

                    //Load all the information from the execl to the datagrid
                    for (int i = 0; i < dataGridViewSLAManger.ColumnCount; ++i)
                    {
                        if (i != 2 && i != 3)
                        {
                            dataGridViewSLAManger.Columns[i].DataPropertyName = slaData.Columns[i].Caption;
                        }
                        else if (i == 2)
                        {
                            dataGridViewSLAManger.Columns[i + 1].DataPropertyName = slaData.Columns[i].Caption;
                           
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

        //Unlock with user password before doing any change
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
       
            InputBox.InputBoxValidation validation = delegate(string password)
            {
                if (password == "")
                {
                    return "Value cannot be empty.";
                }
                if (password == SingletoneUser.UserPass) // Check if password is the same as the logged-in user.
                {
                  
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
