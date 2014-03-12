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
            ((System.ComponentModel.ISupportInitialize)(this.devicesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDevicesDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // gauge1
            // 
            this.gauge1.BaseArcColor = System.Drawing.Color.Gray;
            this.gauge1.BaseArcRadius = 80;
            this.gauge1.BaseArcStart = 175;
            this.gauge1.BaseArcSweep = 190;
            this.gauge1.BaseArcWidth = 2;
            this.gauge1.Center = new System.Drawing.Point(100, 100);
            this.gauge1.Location = new System.Drawing.Point(318, 12);
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
            this.gauge1.ScaleLinesMinorColor = System.Drawing.Color.Gray;
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
            this.gauge2.Location = new System.Drawing.Point(558, 12);
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
            this.gauge2.ScaleLinesMinorColor = System.Drawing.Color.Gray;
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
            this.gauge3.Location = new System.Drawing.Point(794, 12);
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
            this.gauge3.ScaleLinesMinorColor = System.Drawing.Color.Gray;
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
            this.listDevices.Size = new System.Drawing.Size(205, 394);
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
            this.lblDevices.Size = new System.Drawing.Size(46, 13);
            this.lblDevices.TabIndex = 3;
            this.lblDevices.Text = "Devices";
            // 
            // devicesTableAdapter
            // 
            this.devicesTableAdapter.ClearBeforeFill = true;
            // 
            // lblGuage1
            // 
            this.lblGuage1.Location = new System.Drawing.Point(376, 135);
            this.lblGuage1.Name = "lblGuage1";
            this.lblGuage1.Size = new System.Drawing.Size(91, 13);
            this.lblGuage1.TabIndex = 4;
            this.lblGuage1.Text = "Guage 1";
            this.lblGuage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGuage2
            // 
            this.lblGuage2.Location = new System.Drawing.Point(618, 135);
            this.lblGuage2.Name = "lblGuage2";
            this.lblGuage2.Size = new System.Drawing.Size(91, 13);
            this.lblGuage2.TabIndex = 4;
            this.lblGuage2.Text = "Guage 2";
            this.lblGuage2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGuage3
            // 
            this.lblGuage3.Location = new System.Drawing.Point(851, 135);
            this.lblGuage3.Name = "lblGuage3";
            this.lblGuage3.Size = new System.Drawing.Size(91, 13);
            this.lblGuage3.TabIndex = 4;
            this.lblGuage3.Text = "Guage 3";
            this.lblGuage3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 467);
            this.Controls.Add(this.lblGuage3);
            this.Controls.Add(this.lblGuage2);
            this.Controls.Add(this.lblGuage1);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.listDevices);
            this.Controls.Add(this.gauge3);
            this.Controls.Add(this.gauge2);
            this.Controls.Add(this.gauge1);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dashboard_FormClosing);
            this.Load += new System.EventHandler(this.Dashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.devicesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDevicesDataSet)).EndInit();
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

                    
    }
}