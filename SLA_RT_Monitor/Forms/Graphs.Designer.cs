namespace LinqExample
{
    partial class Graphs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Graphs));
            this.cmbBoxThresholdTypes = new System.Windows.Forms.ComboBox();
            this.sLA_RT_monitoringDataSetThreshold = new LinqExample.SLA_RT_monitoringDataSetThreshold();
            this.listDevices = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.thresholdsTableAdapter = new LinqExample.SLA_RT_monitoringDataSetThresholdTableAdapters.ThresholdsTableAdapter();
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSetThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBoxThresholdTypes
            // 
            this.cmbBoxThresholdTypes.DataSource = this.sLA_RT_monitoringDataSetThreshold;
            this.cmbBoxThresholdTypes.DisplayMember = "Thresholds.name";
            this.cmbBoxThresholdTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxThresholdTypes.FormattingEnabled = true;
            this.cmbBoxThresholdTypes.Location = new System.Drawing.Point(13, 31);
            this.cmbBoxThresholdTypes.Name = "cmbBoxThresholdTypes";
            this.cmbBoxThresholdTypes.Size = new System.Drawing.Size(121, 21);
            this.cmbBoxThresholdTypes.TabIndex = 0;
            this.cmbBoxThresholdTypes.SelectedIndexChanged += new System.EventHandler(this.cmbBoxThresholdTypes_SelectedIndexChanged);
            // 
            // sLA_RT_monitoringDataSetThreshold
            // 
            this.sLA_RT_monitoringDataSetThreshold.DataSetName = "SLA_RT_monitoringDataSetThreshold";
            this.sLA_RT_monitoringDataSetThreshold.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // listDevices
            // 
            this.listDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listDevices.FormattingEnabled = true;
            this.listDevices.Location = new System.Drawing.Point(13, 84);
            this.listDevices.Name = "listDevices";
            this.listDevices.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listDevices.Size = new System.Drawing.Size(121, 316);
            this.listDevices.TabIndex = 1;
            this.toolTip1.SetToolTip(this.listDevices, "Use CTRL+Mouse left to choose the devices ");
            this.listDevices.SelectedIndexChanged += new System.EventHandler(this.listDevices_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ThresholdTypes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Devices";
            // 
            // thresholdsTableAdapter
            // 
            this.thresholdsTableAdapter.ClearBeforeFill = true;
            // 
            // zg1
            // 
            this.zg1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zg1.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg1.Location = new System.Drawing.Point(157, 31);
            this.zg1.Name = "zg1";
            this.zg1.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.Size = new System.Drawing.Size(667, 401);
            this.zg1.TabIndex = 0;
            this.zg1.Load += new System.EventHandler(this.zg1_Load);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(12, 409);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Back To MainMenu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // Graphs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 444);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listDevices);
            this.Controls.Add(this.zg1);
            this.Controls.Add(this.cmbBoxThresholdTypes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Graphs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graphs";
            this.Load += new System.EventHandler(this.Graphs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sLA_RT_monitoringDataSetThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbBoxThresholdTypes;
        private System.Windows.Forms.ListBox listDevices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private SLA_RT_monitoringDataSetThresholdTableAdapters.ThresholdsTableAdapter thresholdsTableAdapter;
        private SLA_RT_monitoringDataSetThreshold sLA_RT_monitoringDataSetThreshold;
        private ZedGraph.ZedGraphControl zg1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}