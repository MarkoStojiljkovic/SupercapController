﻿namespace SupercapController
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
            this.buttonTakeSample = new System.Windows.Forms.Button();
            this.textBoxFixedVoltage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDisplay = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxGain = new System.Windows.Forms.TextBox();
            this.buttonCopyGain = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTakeSample
            // 
            this.buttonTakeSample.Location = new System.Drawing.Point(16, 69);
            this.buttonTakeSample.Margin = new System.Windows.Forms.Padding(4);
            this.buttonTakeSample.Name = "buttonTakeSample";
            this.buttonTakeSample.Size = new System.Drawing.Size(108, 28);
            this.buttonTakeSample.TabIndex = 0;
            this.buttonTakeSample.Text = "Take Sample";
            this.buttonTakeSample.UseVisualStyleBackColor = true;
            this.buttonTakeSample.Click += new System.EventHandler(this.buttonTakeSample_Click);
            // 
            // textBoxFixedVoltage
            // 
            this.textBoxFixedVoltage.Location = new System.Drawing.Point(16, 38);
            this.textBoxFixedVoltage.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxFixedVoltage.Name = "textBoxFixedVoltage";
            this.textBoxFixedVoltage.Size = new System.Drawing.Size(107, 22);
            this.textBoxFixedVoltage.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fixed voltage in mV";
            // 
            // textBoxDisplay
            // 
            this.textBoxDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDisplay.Location = new System.Drawing.Point(165, 18);
            this.textBoxDisplay.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxDisplay.Multiline = true;
            this.textBoxDisplay.Name = "textBoxDisplay";
            this.textBoxDisplay.Size = new System.Drawing.Size(496, 271);
            this.textBoxDisplay.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 106);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 28);
            this.button1.TabIndex = 5;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxGain
            // 
            this.textBoxGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxGain.Location = new System.Drawing.Point(174, 297);
            this.textBoxGain.Name = "textBoxGain";
            this.textBoxGain.ReadOnly = true;
            this.textBoxGain.Size = new System.Drawing.Size(144, 15);
            this.textBoxGain.TabIndex = 6;
            this.textBoxGain.Text = "Gain";
            // 
            // buttonCopyGain
            // 
            this.buttonCopyGain.Location = new System.Drawing.Point(16, 142);
            this.buttonCopyGain.Name = "buttonCopyGain";
            this.buttonCopyGain.Size = new System.Drawing.Size(107, 28);
            this.buttonCopyGain.TabIndex = 7;
            this.buttonCopyGain.Text = "Copy Gain";
            this.buttonCopyGain.UseVisualStyleBackColor = true;
            this.buttonCopyGain.Click += new System.EventHandler(this.buttonCopyGain_Click);
            // 
            // Calibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 321);
            this.Controls.Add(this.buttonCopyGain);
            this.Controls.Add(this.textBoxGain);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxDisplay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFixedVoltage);
            this.Controls.Add(this.buttonTakeSample);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Calibrate";
            this.Text = "Calibrate";
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
    }
}