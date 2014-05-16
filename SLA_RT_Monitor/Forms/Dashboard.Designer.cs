namespace LinqExample
{
    partial class Dashboard
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
            System.Windows.Forms.AGaugeRange aGaugeRange1 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange2 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange3 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange4 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange5 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange6 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange7 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange8 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange9 = new System.Windows.Forms.AGaugeRange();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.gauge1 = new System.Windows.Forms.AGauge();
            this.gauge2 = new System.Windows.Forms.AGauge();
            this.gauge3 = new System.Windows.Forms.AGauge();
            this.listDevices = new System.Windows.Forms.ListBox();
            this.devicesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sLA_RT_monitoringDevicesDataSet = new LinqExample.SLA_RT_monitoringDevicesDataSet();
            this.lblDevices = new System.Windows.Forms.Label();
            this.devicesTableAdapter = new LinqExample.SLA_RT_monitoringDevicesDataSetTableAdapters.DevicesTableAdapter();
            this.lblGuage1 = new System.Windows.Forms.Label();
            this.lblGuage2 = new System.Windows.Forms.Label();
            this.lblGuage3 = new System.Windows.Forms.Label();
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.zg2 = new ZedGraph.ZedGraphControl();
            this.zg3 = new ZedGraph.ZedGraphControl();
            this.dataGridIncidents = new System.Windows.Forms.DataGridView();
            this.device_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.device_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.threshold_text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.devicesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDevicesDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIncidents)).BeginInit();
            this.SuspendLayout();
            // 
            // gauge1
            // 
            this.gauge1.BackColor = System.Drawing.SystemColors.Control;
            this.gauge1.BaseArcColor = System.Drawing.Color.Gray;
            this.gauge1.BaseArcRadius = 80;
            this.gauge1.BaseArcStart = 175;
            this.gauge1.BaseArcSweep = 190;
            this.gauge1.BaseArcWidth = 2;
            this.gauge1.Center = new System.Drawing.Point(100, 100);
            this.gauge1.Cursor = System.Windows.Forms.Cursors.Default;
            aGaugeRange1.Color = System.Drawing.Color.Red;
            aGaugeRange1.EndValue = 100F;
            aGaugeRange1.InnerRadius = 70;
            aGaugeRange1.InRange = false;
            aGaugeRange1.Name = "CritRange";
            aGaugeRange1.OuterRadius = 80;
            aGaugeRange1.StartValue = 90F;
            aGaugeRange2.Color = System.Drawing.Color.Green;
            aGaugeRange2.EndValue = 70F;
            aGaugeRange2.InnerRadius = 70;
            aGaugeRange2.InRange = false;
            aGaugeRange2.Name = "OkRange";
            aGaugeRange2.OuterRadius = 80;
            aGaugeRange2.StartValue = 0F;
            aGaugeRange3.Color = System.Drawing.Color.Yellow;
            aGaugeRange3.EndValue = 90F;
            aGaugeRange3.InnerRadius = 70;
            aGaugeRange3.InRange = false;
            aGaugeRange3.Name = "WarnRange";
            aGaugeRange3.OuterRadius = 80;
            aGaugeRange3.StartValue = 70F;
            this.gauge1.GaugeRanges.Add(aGaugeRange1);
            this.gauge1.GaugeRanges.Add(aGaugeRange2);
            this.gauge1.GaugeRanges.Add(aGaugeRange3);
            this.gauge1.Location = new System.Drawing.Point(182, 16);
            this.gauge1.MaxValue = 100F;
            this.gauge1.MinValue = 0F;
            this.gauge1.Name = "gauge1";
            this.gauge1.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.gauge1.NeedleColor2 = System.Drawing.Color.DimGray;
            this.gauge1.NeedleRadius = 80;
            this.gauge1.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.gauge1.NeedleWidth = 2;
            this.gauge1.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.gauge1.ScaleLinesInterInnerRadius = 73;
            this.gauge1.ScaleLinesInterOuterRadius = 80;
            this.gauge1.ScaleLinesInterWidth = 1;
            this.gauge1.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.gauge1.ScaleLinesMajorInnerRadius = 70;
            this.gauge1.ScaleLinesMajorOuterRadius = 80;
            this.gauge1.ScaleLinesMajorStepValue = 20F;
            this.gauge1.ScaleLinesMajorWidth = 2;
            this.gauge1.ScaleLinesMinorColor = System.Drawing.Color.Black;
            this.gauge1.ScaleLinesMinorInnerRadius = 75;
            this.gauge1.ScaleLinesMinorOuterRadius = 80;
            this.gauge1.ScaleLinesMinorTicks = 9;
            this.gauge1.ScaleLinesMinorWidth = 1;
            this.gauge1.ScaleNumbersColor = System.Drawing.Color.Black;
            this.gauge1.ScaleNumbersFormat = null;
            this.gauge1.ScaleNumbersRadius = 95;
            this.gauge1.ScaleNumbersRotation = 0;
            this.gauge1.ScaleNumbersStartScaleLine = 0;
            this.gauge1.ScaleNumbersStepScaleLines = 1;
            this.gauge1.Size = new System.Drawing.Size(215, 187);
            this.gauge1.TabIndex = 1;
            this.gauge1.Value = 0F;
            // 
            // gauge2
            // 
            this.gauge2.BaseArcColor = System.Drawing.Color.Gray;
            this.gauge2.BaseArcRadius = 80;
            this.gauge2.BaseArcStart = 175;
            this.gauge2.BaseArcSweep = 190;
            this.gauge2.BaseArcWidth = 2;
            this.gauge2.Center = new System.Drawing.Point(100, 100);
            this.gauge2.Cursor = System.Windows.Forms.Cursors.Default;
            aGaugeRange4.Color = System.Drawing.Color.Red;
            aGaugeRange4.EndValue = 100F;
            aGaugeRange4.InnerRadius = 70;
            aGaugeRange4.InRange = false;
            aGaugeRange4.Name = "CritRange";
            aGaugeRange4.OuterRadius = 80;
            aGaugeRange4.StartValue = 90F;
            aGaugeRange5.Color = System.Drawing.Color.Green;
            aGaugeRange5.EndValue = 70F;
            aGaugeRange5.InnerRadius = 70;
            aGaugeRange5.InRange = false;
            aGaugeRange5.Name = "OkRange";
            aGaugeRange5.OuterRadius = 80;
            aGaugeRange5.StartValue = 0F;
            aGaugeRange6.Color = System.Drawing.Color.Yellow;
            aGaugeRange6.EndValue = 90F;
            aGaugeRange6.InnerRadius = 70;
            aGaugeRange6.InRange = false;
            aGaugeRange6.Name = "WarnRange";
            aGaugeRange6.OuterRadius = 80;
            aGaugeRange6.StartValue = 70F;
            this.gauge2.GaugeRanges.Add(aGaugeRange4);
            this.gauge2.GaugeRanges.Add(aGaugeRange5);
            this.gauge2.GaugeRanges.Add(aGaugeRange6);
            this.gauge2.Location = new System.Drawing.Point(525, 16);
            this.gauge2.MaxValue = 100F;
            this.gauge2.MinValue = 0F;
            this.gauge2.Name = "gauge2";
            this.gauge2.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.gauge2.NeedleColor2 = System.Drawing.Color.DimGray;
            this.gauge2.NeedleRadius = 80;
            this.gauge2.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.gauge2.NeedleWidth = 2;
            this.gauge2.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.gauge2.ScaleLinesInterInnerRadius = 73;
            this.gauge2.ScaleLinesInterOuterRadius = 80;
            this.gauge2.ScaleLinesInterWidth = 1;
            this.gauge2.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.gauge2.ScaleLinesMajorInnerRadius = 70;
            this.gauge2.ScaleLinesMajorOuterRadius = 80;
            this.gauge2.ScaleLinesMajorStepValue = 20F;
            this.gauge2.ScaleLinesMajorWidth = 2;
            this.gauge2.ScaleLinesMinorColor = System.Drawing.Color.Black;
            this.gauge2.ScaleLinesMinorInnerRadius = 75;
            this.gauge2.ScaleLinesMinorOuterRadius = 80;
            this.gauge2.ScaleLinesMinorTicks = 9;
            this.gauge2.ScaleLinesMinorWidth = 1;
            this.gauge2.ScaleNumbersColor = System.Drawing.Color.Black;
            this.gauge2.ScaleNumbersFormat = null;
            this.gauge2.ScaleNumbersRadius = 95;
            this.gauge2.ScaleNumbersRotation = 0;
            this.gauge2.ScaleNumbersStartScaleLine = 0;
            this.gauge2.ScaleNumbersStepScaleLines = 1;
            this.gauge2.Size = new System.Drawing.Size(215, 187);
            this.gauge2.TabIndex = 1;
            this.gauge2.Value = 0F;
            // 
            // gauge3
            // 
            this.gauge3.BaseArcColor = System.Drawing.Color.Gray;
            this.gauge3.BaseArcRadius = 80;
            this.gauge3.BaseArcStart = 175;
            this.gauge3.BaseArcSweep = 190;
            this.gauge3.BaseArcWidth = 2;
            this.gauge3.Center = new System.Drawing.Point(100, 100);
            this.gauge3.Cursor = System.Windows.Forms.Cursors.Default;
            aGaugeRange7.Color = System.Drawing.Color.Yellow;
            aGaugeRange7.EndValue = 90F;
            aGaugeRange7.InnerRadius = 70;
            aGaugeRange7.InRange = false;
            aGaugeRange7.Name = "WarnRange";
            aGaugeRange7.OuterRadius = 80;
            aGaugeRange7.StartValue = 70F;
            aGaugeRange8.Color = System.Drawing.Color.Red;
            aGaugeRange8.EndValue = 100F;
            aGaugeRange8.InnerRadius = 70;
            aGaugeRange8.InRange = false;
            aGaugeRange8.Name = "CritRange";
            aGaugeRange8.OuterRadius = 80;
            aGaugeRange8.StartValue = 90F;
            aGaugeRange9.Color = System.Drawing.Color.Green;
            aGaugeRange9.EndValue = 70F;
            aGaugeRange9.InnerRadius = 70;
            aGaugeRange9.InRange = false;
            aGaugeRange9.Name = "OkRange";
            aGaugeRange9.OuterRadius = 80;
            aGaugeRange9.StartValue = 0F;
            this.gauge3.GaugeRanges.Add(aGaugeRange7);
            this.gauge3.GaugeRanges.Add(aGaugeRange8);
            this.gauge3.GaugeRanges.Add(aGaugeRange9);
            this.gauge3.Location = new System.Drawing.Point(872, 16);
            this.gauge3.MaxValue = 100F;
            this.gauge3.MinValue = 0F;
            this.gauge3.Name = "gauge3";
            this.gauge3.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.gauge3.NeedleColor2 = System.Drawing.Color.DimGray;
            this.gauge3.NeedleRadius = 80;
            this.gauge3.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.gauge3.NeedleWidth = 2;
            this.gauge3.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.gauge3.ScaleLinesInterInnerRadius = 73;
            this.gauge3.ScaleLinesInterOuterRadius = 80;
            this.gauge3.ScaleLinesInterWidth = 1;
            this.gauge3.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.gauge3.ScaleLinesMajorInnerRadius = 70;
            this.gauge3.ScaleLinesMajorOuterRadius = 80;
            this.gauge3.ScaleLinesMajorStepValue = 20F;
            this.gauge3.ScaleLinesMajorWidth = 2;
            this.gauge3.ScaleLinesMinorColor = System.Drawing.Color.Black;
            this.gauge3.ScaleLinesMinorInnerRadius = 75;
            this.gauge3.ScaleLinesMinorOuterRadius = 80;
            this.gauge3.ScaleLinesMinorTicks = 9;
            this.gauge3.ScaleLinesMinorWidth = 1;
            this.gauge3.ScaleNumbersColor = System.Drawing.Color.Black;
            this.gauge3.ScaleNumbersFormat = null;
            this.gauge3.ScaleNumbersRadius = 95;
            this.gauge3.ScaleNumbersRotation = 0;
            this.gauge3.ScaleNumbersStartScaleLine = 0;
            this.gauge3.ScaleNumbersStepScaleLines = 1;
            this.gauge3.Size = new System.Drawing.Size(215, 187);
            this.gauge3.TabIndex = 1;
            this.gauge3.Value = 0F;
            // 
            // listDevices
            // 
            this.listDevices.DataSource = this.devicesBindingSource;
            this.listDevices.DisplayMember = "name";
            this.listDevices.FormattingEnabled = true;
            this.listDevices.Location = new System.Drawing.Point(13, 52);
            this.listDevices.Name = "listDevices";
            this.listDevices.Size = new System.Drawing.Size(93, 511);
            this.listDevices.TabIndex = 2;
            this.listDevices.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // devicesBindingSource
            // 
            this.devicesBindingSource.DataMember = "Devices";
            this.devicesBindingSource.DataSource = this.sLA_RT_monitoringDevicesDataSet;
            // 
            // sLA_RT_monitoringDevicesDataSet
            // 
            this.sLA_RT_monitoringDevicesDataSet.DataSetName = "SLA_RT_monitoringDevicesDataSet";
            this.sLA_RT_monitoringDevicesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lblDevices
            // 
            this.lblDevices.AutoSize = true;
            this.lblDevices.Location = new System.Drawing.Point(13, 33);
            this.lblDevices.Name = "lblDevices";
            this.lblDevices.Size = new System.Drawing.Size(45, 13);
            this.lblDevices.TabIndex = 3;
            this.lblDevices.Text = "Devices";
            // 
            // devicesTableAdapter
            // 
            this.devicesTableAdapter.ClearBeforeFill = true;
            // 
            // lblGuage1
            // 
            this.lblGuage1.Location = new System.Drawing.Point(228, 139);
            this.lblGuage1.Name = "lblGuage1";
            this.lblGuage1.Size = new System.Drawing.Size(120, 13);
            this.lblGuage1.TabIndex = 4;
            this.lblGuage1.Text = "Guage 1";
            this.lblGuage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGuage2
            // 
            this.lblGuage2.Location = new System.Drawing.Point(572, 139);
            this.lblGuage2.Name = "lblGuage2";
            this.lblGuage2.Size = new System.Drawing.Size(120, 13);
            this.lblGuage2.TabIndex = 4;
            this.lblGuage2.Text = "Guage 2";
            this.lblGuage2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGuage3
            // 
            this.lblGuage3.Location = new System.Drawing.Point(918, 139);
            this.lblGuage3.Name = "lblGuage3";
            this.lblGuage3.Size = new System.Drawing.Size(120, 13);
            this.lblGuage3.TabIndex = 4;
            this.lblGuage3.Text = "Guage 3";
            this.lblGuage3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zg1
            // 
            this.zg1.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg1.Location = new System.Drawing.Point(123, 204);
            this.zg1.Name = "zg1";
            this.zg1.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.Size = new System.Drawing.Size(321, 182);
            this.zg1.TabIndex = 0;
            // 
            // zg2
            // 
            this.zg2.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg2.Location = new System.Drawing.Point(473, 204);
            this.zg2.Name = "zg2";
            this.zg2.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg2.ScrollGrace = 0D;
            this.zg2.ScrollMaxX = 0D;
            this.zg2.ScrollMaxY = 0D;
            this.zg2.ScrollMaxY2 = 0D;
            this.zg2.ScrollMinX = 0D;
            this.zg2.ScrollMinY = 0D;
            this.zg2.ScrollMinY2 = 0D;
            this.zg2.Size = new System.Drawing.Size(321, 182);
            this.zg2.TabIndex = 0;
            // 
            // zg3
            // 
            this.zg3.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg3.Location = new System.Drawing.Point(824, 204);
            this.zg3.Name = "zg3";
            this.zg3.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg3.ScrollGrace = 0D;
            this.zg3.ScrollMaxX = 0D;
            this.zg3.ScrollMaxY = 0D;
            this.zg3.ScrollMaxY2 = 0D;
            this.zg3.ScrollMinX = 0D;
            this.zg3.ScrollMinY = 0D;
            this.zg3.ScrollMinY2 = 0D;
            this.zg3.Size = new System.Drawing.Size(321, 182);
            this.zg3.TabIndex = 0;
            // 
            // dataGridIncidents
            // 
            this.dataGridIncidents.AllowUserToAddRows = false;
            this.dataGridIncidents.AllowUserToDeleteRows = false;
            this.dataGridIncidents.AllowUserToResizeRows = false;
            this.dataGridIncidents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridIncidents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridIncidents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridIncidents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.device_name,
            this.device_type,
            this.threshold_text,
            this.value,
            this.timestamp});
            this.dataGridIncidents.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridIncidents.Location = new System.Drawing.Point(123, 439);
            this.dataGridIncidents.Name = "dataGridIncidents";
            this.dataGridIncidents.RowHeadersVisible = false;
            this.dataGridIncidents.Size = new System.Drawing.Size(1022, 196);
            this.dataGridIncidents.TabIndex = 6;
            // 
            // device_name
            // 
            this.device_name.DataPropertyName = "device_name";
            this.device_name.HeaderText = "Device Name";
            this.device_name.Name = "device_name";
            // 
            // device_type
            // 
            this.device_type.DataPropertyName = "device_type";
            this.device_type.HeaderText = "Device Type";
            this.device_type.Name = "device_type";
            // 
            // threshold_text
            // 
            this.threshold_text.DataPropertyName = "threshold_text";
            this.threshold_text.HeaderText = "Threshold Name";
            this.threshold_text.Name = "threshold_text";
            // 
            // gaugeValue
            // 
            this.value.DataPropertyName = "value";
            this.value.HeaderText = "Threshold Value";
            this.value.Name = "value";
            // 
            // timestamp
            // 
            this.timestamp.DataPropertyName = "timestamp";
            this.timestamp.HeaderText = "Time";
            this.timestamp.Name = "timestamp";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(493, 389);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 47);
            this.label1.TabIndex = 7;
            this.label1.Text = "Current Incidents";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(12, 576);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 59);
            this.button2.TabIndex = 8;
            this.button2.Text = "Back To Universal Dashboard";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 652);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridIncidents);
            this.Controls.Add(this.lblGuage3);
            this.Controls.Add(this.lblGuage2);
            this.Controls.Add(this.lblGuage1);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.listDevices);
            this.Controls.Add(this.gauge3);
            this.Controls.Add(this.gauge2);
            this.Controls.Add(this.gauge1);
            this.Controls.Add(this.zg1);
            this.Controls.Add(this.zg2);
            this.Controls.Add(this.zg3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dashboard_FormClosing);
            this.Load += new System.EventHandler(this.Dashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.devicesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDevicesDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIncidents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.AGauge gauge1;
        private System.Windows.Forms.AGauge gauge2;
        private System.Windows.Forms.AGauge gauge3;
        private System.Windows.Forms.ListBox listDevices;
        private System.Windows.Forms.Label lblDevices;
        private SLA_RT_monitoringDevicesDataSet sLA_RT_monitoringDevicesDataSet;
        private System.Windows.Forms.BindingSource devicesBindingSource;
        private SLA_RT_monitoringDevicesDataSetTableAdapters.DevicesTableAdapter devicesTableAdapter;
        private System.Windows.Forms.Label lblGuage1;
        private System.Windows.Forms.Label lblGuage2;
        private System.Windows.Forms.Label lblGuage3;

        private ZedGraph.ZedGraphControl zg1;
        private ZedGraph.ZedGraphControl zg2;
        private ZedGraph.ZedGraphControl zg3;
        private System.Windows.Forms.DataGridView dataGridIncidents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn device_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn device_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn threshold_text;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestamp;


                    
    }
}