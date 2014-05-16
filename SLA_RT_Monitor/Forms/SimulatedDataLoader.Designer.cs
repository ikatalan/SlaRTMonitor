namespace LinqExample
{
    partial class SimulatedDataLoader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulatedDataLoader));
            this.btnLoad = new System.Windows.Forms.Button();
            this.gridSimulatedData = new System.Windows.Forms.DataGridView();
            this.sLARTmonitoringDataSetMeasurementsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sLA_RT_monitoringDataSetMeasurements = new LinqExample.SLA_RT_monitoringDataSetMeasurements();
            this.simulatedMeasurementsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.thresholdsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.sLA_RT_monitoringDataSetThreshold = new LinqExample.SLA_RT_monitoringDataSetThreshold();
            this.sLARTmonitoringDataSetThresholdBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.thresholdsTableAdapter = new LinqExample.SLA_RT_monitoringDataSetThresholdTableAdapters.ThresholdsTableAdapter();
            this.simulatedMeasurementsTableAdapter = new LinqExample.SLA_RT_monitoringDataSetMeasurementsTableAdapters.SimulatedMeasurementsTableAdapter();
            this.device_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.threshold_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridSimulatedData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLARTmonitoringDataSetMeasurementsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSetMeasurements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simulatedMeasurementsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSetThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLARTmonitoringDataSetThresholdBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(145, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load Simulated Data";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // gridSimulatedData
            // 
            this.gridSimulatedData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSimulatedData.AutoGenerateColumns = false;
            this.gridSimulatedData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridSimulatedData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSimulatedData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.device_id,
            this.threshold_id,
            this.value,
            this.timestamp});
            this.gridSimulatedData.DataSource = this.sLARTmonitoringDataSetMeasurementsBindingSource;
            this.gridSimulatedData.Location = new System.Drawing.Point(12, 41);
            this.gridSimulatedData.Name = "gridSimulatedData";
            this.gridSimulatedData.Size = new System.Drawing.Size(796, 257);
            this.gridSimulatedData.TabIndex = 1;
            // 
            // sLARTmonitoringDataSetMeasurementsBindingSource
            // 
            this.sLARTmonitoringDataSetMeasurementsBindingSource.DataSource = this.sLA_RT_monitoringDataSetMeasurements;
            this.sLARTmonitoringDataSetMeasurementsBindingSource.Position = 0;
            // 
            // sLA_RT_monitoringDataSetMeasurements
            // 
            this.sLA_RT_monitoringDataSetMeasurements.DataSetName = "SLA_RT_monitoringDataSetMeasurements";
            this.sLA_RT_monitoringDataSetMeasurements.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 304);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Back To MainMenu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // sLA_RT_monitoringDataSetThreshold
            // 
            this.sLA_RT_monitoringDataSetThreshold.DataSetName = "SLA_RT_monitoringDataSetThreshold";
            this.sLA_RT_monitoringDataSetThreshold.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sLARTmonitoringDataSetThresholdBindingSource
            // 
            this.sLARTmonitoringDataSetThresholdBindingSource.DataSource = this.sLA_RT_monitoringDataSetThreshold;
            this.sLARTmonitoringDataSetThresholdBindingSource.Position = 0;
            // 
            // thresholdsTableAdapter
            // 
            this.thresholdsTableAdapter.ClearBeforeFill = true;
            // 
            // simulatedMeasurementsTableAdapter
            // 
            this.simulatedMeasurementsTableAdapter.ClearBeforeFill = true;
            // 
            // device_id
            // 
            this.device_id.HeaderText = "Device Name";
            this.device_id.Name = "device_id";
            // 
            // threshold_id
            // 
            this.threshold_id.HeaderText = "Threshold Name";
            this.threshold_id.Name = "threshold_id";
            // 
            // gaugeValue
            // 
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            // 
            // timestamp
            // 
            this.timestamp.DataPropertyName = "timestamp";
            this.timestamp.HeaderText = "Timestamp";
            this.timestamp.Name = "timestamp";
            this.timestamp.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.timestamp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SimulatedDataLoader
            // 
            this.ClientSize = new System.Drawing.Size(820, 334);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gridSimulatedData);
            this.Controls.Add(this.btnLoad);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SimulatedDataLoader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SimulatedData";
            this.Load += new System.EventHandler(this.SimulatedDataLoader_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridSimulatedData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLARTmonitoringDataSetMeasurementsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSetMeasurements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simulatedMeasurementsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSetThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLARTmonitoringDataSetThresholdBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.BindingSource simulatedMeasurementsBindingSource;
        private LinqExample.SLA_RT_monitoringDataSetMeasurementsTableAdapters.SimulatedMeasurementsTableAdapter simulatedMeasurementsTableAdapter;
        private System.Windows.Forms.DataGridView gridSimulatedData;
        private System.Windows.Forms.BindingSource sLARTmonitoringDataSetMeasurementsBindingSource;
        private SLA_RT_monitoringDataSetThreshold sLA_RT_monitoringDataSetThreshold;
        private System.Windows.Forms.BindingSource sLARTmonitoringDataSetThresholdBindingSource;
        private System.Windows.Forms.BindingSource thresholdsBindingSource;
        private SLA_RT_monitoringDataSetThresholdTableAdapters.ThresholdsTableAdapter thresholdsTableAdapter;
        private System.Windows.Forms.Button button1;
        private SLA_RT_monitoringDataSetMeasurements sLA_RT_monitoringDataSetMeasurements;
        private System.Windows.Forms.DataGridViewTextBoxColumn device_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn threshold_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestamp;
    }
}