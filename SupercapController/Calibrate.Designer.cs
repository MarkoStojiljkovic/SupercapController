namespace SupercapController
{
    partial class Calibrate
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
            this.buttonTakeSample = new System.Windows.Forms.Button();
            this.textBoxFixedVoltage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDisplay = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxGain = new System.Windows.Forms.TextBox();
            this.buttonCopyGain = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBoxAutoDecrement = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelMousePosX = new System.Windows.Forms.Label();
            this.labelMousePosY = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonCalib100A = new System.Windows.Forms.Button();
            this.buttonCalib10A = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonTakeSample
            // 
            this.buttonTakeSample.Location = new System.Drawing.Point(12, 56);
            this.buttonTakeSample.Name = "buttonTakeSample";
            this.buttonTakeSample.Size = new System.Drawing.Size(81, 23);
            this.buttonTakeSample.TabIndex = 1;
            this.buttonTakeSample.Text = "Take Sample";
            this.buttonTakeSample.UseVisualStyleBackColor = true;
            this.buttonTakeSample.Click += new System.EventHandler(this.buttonTakeSample_Click);
            // 
            // textBoxFixedVoltage
            // 
            this.textBoxFixedVoltage.Location = new System.Drawing.Point(12, 31);
            this.textBoxFixedVoltage.Name = "textBoxFixedVoltage";
            this.textBoxFixedVoltage.Size = new System.Drawing.Size(81, 20);
            this.textBoxFixedVoltage.TabIndex = 0;
            this.textBoxFixedVoltage.Enter += new System.EventHandler(this.textBoxFixedVoltage_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fixed voltage in mV";
            // 
            // textBoxDisplay
            // 
            this.textBoxDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDisplay.Location = new System.Drawing.Point(206, 15);
            this.textBoxDisplay.Multiline = true;
            this.textBoxDisplay.Name = "textBoxDisplay";
            this.textBoxDisplay.Size = new System.Drawing.Size(286, 415);
            this.textBoxDisplay.TabIndex = 3;
            this.textBoxDisplay.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 23);
            this.button1.TabIndex = 5;
            this.button1.TabStop = false;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxGain
            // 
            this.textBoxGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxGain.Location = new System.Drawing.Point(130, 435);
            this.textBoxGain.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxGain.Name = "textBoxGain";
            this.textBoxGain.ReadOnly = true;
            this.textBoxGain.Size = new System.Drawing.Size(108, 13);
            this.textBoxGain.TabIndex = 6;
            this.textBoxGain.TabStop = false;
            this.textBoxGain.Text = "Gain";
            // 
            // buttonCopyGain
            // 
            this.buttonCopyGain.Location = new System.Drawing.Point(12, 115);
            this.buttonCopyGain.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCopyGain.Name = "buttonCopyGain";
            this.buttonCopyGain.Size = new System.Drawing.Size(80, 23);
            this.buttonCopyGain.TabIndex = 7;
            this.buttonCopyGain.TabStop = false;
            this.buttonCopyGain.Text = "Copy Gain";
            this.buttonCopyGain.UseVisualStyleBackColor = true;
            this.buttonCopyGain.Click += new System.EventHandler(this.buttonCopyGain_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 144);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(103, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.TabStop = false;
            this.checkBox1.Text = "Auto Decrement";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBoxAutoDecrement
            // 
            this.textBoxAutoDecrement.Location = new System.Drawing.Point(13, 191);
            this.textBoxAutoDecrement.Name = "textBoxAutoDecrement";
            this.textBoxAutoDecrement.Size = new System.Drawing.Size(100, 20);
            this.textBoxAutoDecrement.TabIndex = 9;
            this.textBoxAutoDecrement.TabStop = false;
            this.textBoxAutoDecrement.Text = "200";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Decrement by mV";
            // 
            // labelMousePosX
            // 
            this.labelMousePosX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMousePosX.AutoSize = true;
            this.labelMousePosX.Location = new System.Drawing.Point(10, 407);
            this.labelMousePosX.Name = "labelMousePosX";
            this.labelMousePosX.Size = new System.Drawing.Size(38, 13);
            this.labelMousePosX.TabIndex = 11;
            this.labelMousePosX.Text = "Pos X:";
            // 
            // labelMousePosY
            // 
            this.labelMousePosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMousePosY.AutoSize = true;
            this.labelMousePosY.Location = new System.Drawing.Point(10, 433);
            this.labelMousePosY.Name = "labelMousePosY";
            this.labelMousePosY.Size = new System.Drawing.Size(38, 13);
            this.labelMousePosY.TabIndex = 12;
            this.labelMousePosY.Text = "Pos Y:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.Location = new System.Drawing.Point(12, 228);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(145, 127);
            this.dataGridView1.TabIndex = 13;
            this.dataGridView1.TabStop = false;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "X";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 70;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Y";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 70;
            // 
            // buttonCalib100A
            // 
            this.buttonCalib100A.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCalib100A.Location = new System.Drawing.Point(16, 362);
            this.buttonCalib100A.Name = "buttonCalib100A";
            this.buttonCalib100A.Size = new System.Drawing.Size(75, 23);
            this.buttonCalib100A.TabIndex = 14;
            this.buttonCalib100A.TabStop = false;
            this.buttonCalib100A.Text = "Calib 100A";
            this.buttonCalib100A.UseVisualStyleBackColor = true;
            this.buttonCalib100A.Click += new System.EventHandler(this.buttonCalib100A_Click);
            // 
            // buttonCalib10A
            // 
            this.buttonCalib10A.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCalib10A.Location = new System.Drawing.Point(97, 362);
            this.buttonCalib10A.Name = "buttonCalib10A";
            this.buttonCalib10A.Size = new System.Drawing.Size(75, 23);
            this.buttonCalib10A.TabIndex = 15;
            this.buttonCalib10A.TabStop = false;
            this.buttonCalib10A.Text = "Calib 10A";
            this.buttonCalib10A.UseVisualStyleBackColor = true;
            this.buttonCalib10A.Click += new System.EventHandler(this.buttonCalib10A_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(420, 435);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(37, 13);
            this.labelStatus.TabIndex = 17;
            this.labelStatus.Text = "Status";
            // 
            // Calibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 455);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonCalib10A);
            this.Controls.Add(this.buttonCalib100A);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.labelMousePosY);
            this.Controls.Add(this.labelMousePosX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxAutoDecrement);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.buttonCopyGain);
            this.Controls.Add(this.textBoxGain);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxDisplay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFixedVoltage);
            this.Controls.Add(this.buttonTakeSample);
            this.KeyPreview = true;
            this.Name = "Calibrate";
            this.Text = "Calibrate";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Calibrate_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTakeSample;
        private System.Windows.Forms.TextBox textBoxFixedVoltage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDisplay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxGain;
        private System.Windows.Forms.Button buttonCopyGain;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBoxAutoDecrement;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelMousePosX;
        private System.Windows.Forms.Label labelMousePosY;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Button buttonCalib100A;
        private System.Windows.Forms.Button buttonCalib10A;
        private System.Windows.Forms.Label labelStatus;
    }
}