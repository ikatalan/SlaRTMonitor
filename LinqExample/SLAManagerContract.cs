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

namespace LinqExample
{
    public partial class SLAManagerContract : Form
    {
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

            BindingSource SBind = new BindingSource();
            SBind.DataSource = this.sLA_RT_monitoringDataSetSlaContracts.SlaContracts;

            dataGridViewSLAManger.AutoGenerateColumns = false;
            dataGridViewSLAManger.DataSource = this.sLA_RT_monitoringDataSetSlaContracts.SlaContracts;

            for (int i = 1; i < dataGridViewSLAManger.ColumnCount; ++i)
            {
                if (i != 3)
                {
                    dataGridViewSLAManger.Columns[i - 1].DataPropertyName = this.sLA_RT_monitoringDataSetSlaContracts.SlaContracts.Columns[i].Caption;
                }
            }

            dataGridViewSLAManger.DataSource = SBind;
            dataGridViewSLAManger.Refresh();

        }

        private void backMainMenu_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                DialogResult result = MessageBox.Show("Save Data?", "Back to main menu", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                  //  btnSave_Click(null, null);// still need to work on 
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

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            // Need to open a Confirm Dialog for p/W protection.
            if (btnUnlock.Text == "Lock")
            {
                btnSave.Visible = false;
                btnLoad.Visible = false;
                btnUnlock.Text = "Unlock";
                return;
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
                    dataGridViewSLAManger.AllowUserToAddRows = true;
                    dataGridViewSLAManger.AllowUserToDeleteRows = true;
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
            return;//still need to work on this
            if (DialogResult.Cancel == MessageBox.Show("Are you sure you want to replace the current SLA agreemnet", "SLA Agreement change", MessageBoxButtons.OKCancel))
            {
                return;
            }

            LinqExample.SLA_RT_monitoringDataSet1TableAdapters.SlaAgreementTableAdapter adapter1
                = new SLA_RT_monitoringDataSet1TableAdapters.SlaAgreementTableAdapter();

            adapter1.DeleteAll();

            LinqExample.SLA_RT_monitoringDataSet1.SlaAgreementDataTable s = new SLA_RT_monitoringDataSet1.SlaAgreementDataTable();

            DataTable data = (DataTable)((BindingSource)this.dataGridViewSLAManger.DataSource).DataSource;
            foreach (DataRow row in data.Rows)
            {
                s.AddSlaAgreementRow((string)row[0], (string)row[1], (string)row[2], ((System.Int32)row[3]).ToString());
            }

            int rowsAffected = adapter1.Update(s);

            data.AcceptChanges();
        }
    }
}
