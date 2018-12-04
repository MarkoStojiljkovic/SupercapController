namespace SupercapController
{
    partial class FormMultiVoltageViewer
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColDevId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonGetVoltage = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColDevId});
            this.dataGridView1.Location = new System.Drawing.Point(13, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(287, 361);
            this.dataGridView1.TabIndex = 0;
            // 
            // ColDevId
            // 
            this.ColDevId.HeaderText = "Device ID";
            this.ColDevId.Name = "ColDevId";
            this.ColDevId.ReadOnly = true;
            this.ColDevId.Width = 80;
            // 
            // buttonGetVoltage
            // 
            this.buttonGetVoltage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonGetVoltage.Location = new System.Drawing.Point(13, 381);
            this.buttonGetVoltage.Name = "buttonGetVoltage";
            this.buttonGetVoltage.Size = new System.Drawing.Size(75, 23);
            this.buttonGetVoltage.TabIndex = 1;
            this.buttonGetVoltage.Text = "Get Voltage";
            this.buttonGetVoltage.UseVisualStyleBackColor = true;
            this.buttonGetVoltage.Click += new System.EventHandler(this.buttonGetVoltage_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(235, 391);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(37, 13);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Status";
            // 
            // FormMultiVoltageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 424);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonGetVoltage);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormMultiVoltageViewer";
            this.Text = "FormMultiVoltageViewer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDevId;
        private System.Windows.Forms.Button buttonGetVoltage;
        private System.Windows.Forms.Label labelStatus;
    }
}