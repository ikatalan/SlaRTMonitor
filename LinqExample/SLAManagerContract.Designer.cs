namespace LinqExample
{
    partial class SLAManagerContract
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SLAManagerContract));
            this.dataGridViewSLAManger = new System.Windows.Forms.DataGridView();
            this.devicetypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thresholdidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.threshold_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slaContractsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sLA_RT_monitoringDataSetSlaContracts = new LinqExample.SLA_RT_monitoringDataSetSlaContracts();
            this.button1 = new System.Windows.Forms.Button();
            this.slaContractsTableAdapter = new LinqExample.SLA_RT_monitoringDataSetSlaContractsTableAdapters.SlaContractsTableAdapter();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSLAManger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaContractsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSetSlaContracts)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSLAManger
            // 
            this.dataGridViewSLAManger.AutoGenerateColumns = false;
            this.dataGridViewSLAManger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.devicetypeDataGridViewTextBoxColumn,
            this.thresholdidDataGridViewTextBoxColumn,
            this.threshold_name,
            this.valueDataGridViewTextBoxColumn});
            this.dataGridViewSLAManger.DataSource = this.slaContractsBindingSource;
            this.dataGridViewSLAManger.Location = new System.Drawing.Point(12, 31);
            this.dataGridViewSLAManger.Name = "dataGridViewSLAManger";
            this.dataGridViewSLAManger.RowTemplate.Height = 24;
            this.dataGridViewSLAManger.Size = new System.Drawing.Size(693, 364);
            this.dataGridViewSLAManger.TabIndex = 0;
            // 
            // devicetypeDataGridViewTextBoxColumn
            // 
            this.devicetypeDataGridViewTextBoxColumn.DataPropertyName = "device_type";
            this.devicetypeDataGridViewTextBoxColumn.HeaderText = "Device Type";
            this.devicetypeDataGridViewTextBoxColumn.Name = "devicetypeDataGridViewTextBoxColumn";
            this.devicetypeDataGridViewTextBoxColumn.Width = 163;
            // 
            // thresholdidDataGridViewTextBoxColumn
            // 
            this.thresholdidDataGridViewTextBoxColumn.DataPropertyName = "threshold_id";
            this.thresholdidDataGridViewTextBoxColumn.HeaderText = "Threshold ID";
            this.thresholdidDataGridViewTextBoxColumn.Name = "thresholdidDataGridViewTextBoxColumn";
            this.thresholdidDataGridViewTextBoxColumn.Width = 162;
            // 
            // threshold_name
            // 
            this.threshold_name.HeaderText = "Threshold Name";
            this.threshold_name.Name = "threshold_name";
            this.threshold_name.Width = 163;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            this.valueDataGridViewTextBoxColumn.Width = 162;
            // 
            // slaContractsBindingSource
            // 
            this.slaContractsBindingSource.DataMember = "SlaContracts";
            this.slaContractsBindingSource.DataSource = this.sLA_RT_monitoringDataSetSlaContracts;
            // 
            // sLA_RT_monitoringDataSetSlaContracts
            // 
            this.sLA_RT_monitoringDataSetSlaContracts.DataSetName = "SLA_RT_monitoringDataSetSlaContracts";
            this.sLA_RT_monitoringDataSetSlaContracts.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 420);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Back To MainMenu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.backMainMenu_Click);
            // 
            // slaContractsTableAdapter
            // 
            this.slaContractsTableAdapter.ClearBeforeFill = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(187, 420);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(127, 23);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load SLA Agreement";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Visible = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(341, 420);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(155, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save New SLA Agreement";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Location = new System.Drawing.Point(561, 420);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(67, 23);
            this.btnUnlock.TabIndex = 9;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // SLAManagerContract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 464);
            this.ControlBox = false;
            this.Controls.Add(this.btnUnlock);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridViewSLAManger);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SLAManagerContract";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SLAManager";
            this.Load += new System.EventHandler(this.SLAManagerContract_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSLAManger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaContractsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSetSlaContracts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSLAManger;
        private SLA_RT_monitoringDataSetSlaContracts sLA_RT_monitoringDataSetSlaContracts;
        private System.Windows.Forms.BindingSource slaContractsBindingSource;
        private SLA_RT_monitoringDataSetSlaContractsTableAdapters.SlaContractsTableAdapter slaContractsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn devicetypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn thresholdidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn threshold_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUnlock;
    }
}