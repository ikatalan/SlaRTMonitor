namespace LinqExample
{
    partial class SLAManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SLAManager));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.device_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.threshold_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.sLARTmonitoringDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sLA_RT_monitoringDataSet2 = new LinqExample.SLA_RT_monitoringDataSet2();
            this.sLA_RT_monitoringDataSet = new LinqExample.SLA_RT_monitoringDataSet();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sLA_RT_monitoringDataSet1 = new LinqExample.SLA_RT_monitoringDataSet1();
            this.slaAgreementBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.slaAgreementTableAdapter = new LinqExample.SLA_RT_monitoringDataSet1TableAdapters.SlaAgreementTableAdapter();
            this.slA_RT_monitoringDataSet21 = new LinqExample.SLA_RT_monitoringDataSet2();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLARTmonitoringDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaAgreementBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slA_RT_monitoringDataSet21)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(707, 470);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSave);
            this.tabPage1.Controls.Add(this.btnUnlock);
            this.tabPage1.Controls.Add(this.btnLoad);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.dataGridView2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(699, 444);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Overall";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(407, 387);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(155, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save New SLA Agreement";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.saveSLAAgreement_click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Location = new System.Drawing.Point(583, 387);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(67, 23);
            this.btnUnlock.TabIndex = 6;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.unlock_click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(171, 387);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(127, 23);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "Load SLA Agreement";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Visible = false;
            this.btnLoad.Click += new System.EventHandler(this.loadSLAAgreement_click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 387);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Back To MainMenu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.device_type,
            this.name,
            this.threshold_id,
            this.value});
            this.dataGridView2.Location = new System.Drawing.Point(3, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(693, 362);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // device_type
            // 
            this.device_type.HeaderText = "Device type";
            this.device_type.Name = "device_type";
            // 
            // name
            // 
            this.name.HeaderText = "Threshold name";
            this.name.Name = "name";
            // 
            // threshold_id
            // 
            this.threshold_id.HeaderText = "Threshold id";
            this.threshold_id.Name = "threshold_id";
            // 
            // value
            // 
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(699, 444);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Manage";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // sLARTmonitoringDataSetBindingSource
            // 
            this.sLARTmonitoringDataSetBindingSource.DataSource = this.sLA_RT_monitoringDataSet2;
            this.sLARTmonitoringDataSetBindingSource.Position = 0;
            this.sLARTmonitoringDataSetBindingSource.CurrentChanged += new System.EventHandler(this.sLARTmonitoringDataSetBindingSource_CurrentChanged);
            // 
            // sLA_RT_monitoringDataSet2
            // 
            this.sLA_RT_monitoringDataSet2.DataSetName = "SLA_RT_monitoringDataSet2";
            this.sLA_RT_monitoringDataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sLA_RT_monitoringDataSet
            // 
            this.sLA_RT_monitoringDataSet.DataSetName = "SLA_RT_monitoringDataSet";
            this.sLA_RT_monitoringDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // sLA_RT_monitoringDataSet1
            // 
            this.sLA_RT_monitoringDataSet1.DataSetName = "SLA_RT_monitoringDataSet1";
            this.sLA_RT_monitoringDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // slaAgreementBindingSource
            // 
            this.slaAgreementBindingSource.DataMember = "SlaAgreement";
            this.slaAgreementBindingSource.DataSource = this.sLA_RT_monitoringDataSet1;
            // 
            // slaAgreementTableAdapter
            // 
            this.slaAgreementTableAdapter.ClearBeforeFill = true;
            // 
            // slA_RT_monitoringDataSet21
            // 
            this.slA_RT_monitoringDataSet21.DataSetName = "SLA_RT_monitoringDataSet2";
            this.slA_RT_monitoringDataSet21.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // SLAManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 470);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SLAManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SLAManager";
            this.Load += new System.EventHandler(this.SLAManager_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLARTmonitoringDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaAgreementBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slA_RT_monitoringDataSet21)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.BindingSource sLARTmonitoringDataSetBindingSource;
        private SLA_RT_monitoringDataSet sLA_RT_monitoringDataSet;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnUnlock;
        private System.Windows.Forms.Button btnSave;
        private SLA_RT_monitoringDataSet1 sLA_RT_monitoringDataSet1;
        private System.Windows.Forms.BindingSource slaAgreementBindingSource;
        private SLA_RT_monitoringDataSet1TableAdapters.SlaAgreementTableAdapter slaAgreementTableAdapter;
        private SLA_RT_monitoringDataSet2 sLA_RT_monitoringDataSet2;
        private SLA_RT_monitoringDataSet2 slA_RT_monitoringDataSet21;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn device_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn threshold_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
    }
}