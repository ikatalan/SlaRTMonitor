using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace LinqExample
{
    public partial class SLAManager : Form
    {
        private bool changed;
        public SLAManager()
        {
            changed = false;
            InitializeComponent();
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                DialogResult result = MessageBox.Show("Save Data?", "Back to main menu", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    saveSLAAgreement_click(null, null);
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


        private void loadSLAAgreement_click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\Users\\%USERNAME%";//Go to Desktop
         //   openFileDialog1.Filter = "Excel Files(.csv)|*.csv|Excel Files(.xls)|*.xls|Excel Files(.xlsx)|*.xlsx";
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

                    dataGridView2.AutoGenerateColumns = false;
                    dataGridView2.DataSource = slaData;

                    for (int i = 1; i < dataGridView2.ColumnCount; ++i)
                    {
                        dataGridView2.Columns[i].DataPropertyName = slaData.Columns[i-1].Caption;
                    }

                    dataGridView2.DataSource = SBind;
                    dataGridView2.Refresh();
                    changed = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
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

        //Unlock - need to contuine from here
        private void unlock_click(object sender, EventArgs e)
        {
            // Need to open a Confirm Dialog for p/W protection.
            if (btnUnlock.Text == "Lock") {
                btnSave.Visible = false;
                btnLoad.Visible = false;
                btnUnlock.Text = "Unlock";
            }
           
           String input = string.Empty; // will hold the passowrd
           input = "1234";
            
            InputBox.InputBoxValidation validation = delegate(string password)
            {
                if (password == "")
                {
                    return "Value cannot be empty.";
                }
                if (password == input)
                {
                    dataGridView2.AllowUserToAddRows = true;
                    dataGridView2.AllowUserToDeleteRows = true;
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

        //Save SLA Agreement 
        private void saveSLAAgreement_click(object sender, EventArgs e)
        {
            if (DialogResult.Cancel == MessageBox.Show("Are you sure you want to replace the current SLA agreemnet", "SLA Agreement change", MessageBoxButtons.OKCancel))
            {
                return;
            }

            LinqExample.SLA_RT_monitoringDataSet1TableAdapters.SlaAgreementTableAdapter adapter1
                = new SLA_RT_monitoringDataSet1TableAdapters.SlaAgreementTableAdapter();

            adapter1.DeleteAll();

            LinqExample.SLA_RT_monitoringDataSet1.SlaAgreementDataTable s = new SLA_RT_monitoringDataSet1.SlaAgreementDataTable(); 

            DataTable data = (DataTable)((BindingSource)this.dataGridView2.DataSource).DataSource;
            foreach (DataRow row in data.Rows) 
            {
                s.AddSlaAgreementRow((string)row[0], (string)row[1], (string)row[2], ((System.Int32)row[3]).ToString());
            }

            int rowsAffected = adapter1.Update(s);
            
            data.AcceptChanges();
        }

        private void SLAManager_Load(object sender, EventArgs e)
        {
            changed = false;
            this.slaAgreementTableAdapter.Fill(this.sLA_RT_monitoringDataSet1.SlaAgreement);

            BindingSource SBind = new BindingSource();
            SBind.DataSource = this.sLA_RT_monitoringDataSet1.SlaAgreement;

            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = this.sLA_RT_monitoringDataSet1.SlaAgreement;

            for (int i = 1; i < dataGridView2.ColumnCount; ++i)
            {
                dataGridView2.Columns[i].DataPropertyName = this.sLA_RT_monitoringDataSet1.SlaAgreement.Columns[i].Caption;
            }

            dataGridView2.DataSource = SBind;
            dataGridView2.Refresh();
        }

        private void sLARTmonitoringDataSetBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }




    }
}
