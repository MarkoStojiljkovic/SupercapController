namespace SupercapController
{
    partial class FormMeasurementDualChannelPresenter
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxGainCH0 = new System.Windows.Forms.TextBox();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.chartCH0 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dataGridViewCH0 = new System.Windows.Forms.DataGridView();
            this.ColumnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.floatValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.floatValueWithGain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chartCH1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dataGridViewCH1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxGainCH1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSaveData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartCH0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCH0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCH1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCH1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(113, 430);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "CH0 gain";
            // 
            // textBoxGainCH0
            // 
            this.textBoxGainCH0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxGainCH0.Location = new System.Drawing.Point(169, 427);
            this.textBoxGainCH0.Name = "textBoxGainCH0";
            this.textBoxGainCH0.Size = new System.Drawing.Size(100, 20);
            this.textBoxGainCH0.TabIndex = 22;
            this.textBoxGainCH0.Text = "1";
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCalculate.Location = new System.Drawing.Point(14, 425);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(75, 23);
            this.buttonCalculate.TabIndex = 21;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // chartCH0
            // 
            this.chartCH0.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartCH0.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartCH0.Legends.Add(legend1);
            this.chartCH0.Location = new System.Drawing.Point(3, 3);
            this.chartCH0.Name = "chartCH0";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "mV";
            this.chartCH0.Series.Add(series1);
            this.chartCH0.Size = new System.Drawing.Size(781, 197);
            this.chartCH0.TabIndex = 20;
            this.chartCH0.Text = "chartCH0";
            // 
            // dataGridViewCH0
            // 
            this.dataGridViewCH0.AllowUserToAddRows = false;
            this.dataGridViewCH0.AllowUserToDeleteRows = false;
            this.dataGridViewCH0.AllowUserToResizeColumns = false;
            this.dataGridViewCH0.AllowUserToResizeRows = false;
            this.dataGridViewCH0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCH0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCH0.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnIndex,
            this.ColumnValue,
            this.floatValue,
            this.floatValueWithGain});
            this.dataGridViewCH0.Location = new System.Drawing.Point(790, 3);
            this.dataGridViewCH0.Name = "dataGridViewCH0";
            this.dataGridViewCH0.RowHeadersVisible = false;
            this.dataGridViewCH0.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewCH0.Size = new System.Drawing.Size(342, 197);
            this.dataGridViewCH0.TabIndex = 19;
            // 
            // ColumnIndex
            // 
            this.ColumnIndex.HeaderText = "Index";
            this.ColumnIndex.Name = "ColumnIndex";
            this.ColumnIndex.Width = 60;
            // 
            // ColumnValue
            // 
            this.ColumnValue.HeaderText = "Raw Value";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.Width = 80;
            // 
            // floatValue
            // 
            this.floatValue.HeaderText = "Float Value";
            this.floatValue.Name = "floatValue";
            this.floatValue.Width = 80;
            // 
            // floatValueWithGain
            // 
            this.floatValueWithGain.HeaderText = "Float Value * Gain";
            this.floatValueWithGain.Name = "floatValueWithGain";
            this.floatValueWithGain.Width = 120;
            // 
            // chartCH1
            // 
            this.chartCH1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chartCH1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartCH1.Legends.Add(legend2);
            this.chartCH1.Location = new System.Drawing.Point(3, 206);
            this.chartCH1.Name = "chartCH1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "mV";
            this.chartCH1.Series.Add(series2);
            this.chartCH1.Size = new System.Drawing.Size(781, 198);
            this.chartCH1.TabIndex = 25;
            this.chartCH1.Text = "chartCH1";
            // 
            // dataGridViewCH1
            // 
            this.dataGridViewCH1.AllowUserToAddRows = false;
            this.dataGridViewCH1.AllowUserToDeleteRows = false;
            this.dataGridViewCH1.AllowUserToResizeColumns = false;
            this.dataGridViewCH1.AllowUserToResizeRows = false;
            this.dataGridViewCH1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCH1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCH1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dataGridViewCH1.Location = new System.Drawing.Point(790, 206);
            this.dataGridViewCH1.Name = "dataGridViewCH1";
            this.dataGridViewCH1.RowHeadersVisible = false;
            this.dataGridViewCH1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewCH1.Size = new System.Drawing.Size(342, 198);
            this.dataGridViewCH1.TabIndex = 24;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Index";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Raw Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Float Value";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Float Value * Gain";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 120;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 430);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Ch1 gain";
            // 
            // textBoxGainCH1
            // 
            this.textBoxGainCH1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxGainCH1.Location = new System.Drawing.Point(346, 427);
            this.textBoxGainCH1.Name = "textBoxGainCH1";
            this.textBoxGainCH1.Size = new System.Drawing.Size(100, 20);
            this.textBoxGainCH1.TabIndex = 26;
            this.textBoxGainCH1.Text = "1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 348F));
            this.tableLayoutPanel1.Controls.Add(this.chartCH0, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewCH0, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chartCH1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewCH1, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1135, 407);
            this.tableLayoutPanel1.TabIndex = 28;
            // 
            // buttonSaveData
            // 
            this.buttonSaveData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveData.Location = new System.Drawing.Point(1050, 424);
            this.buttonSaveData.Name = "buttonSaveData";
            this.buttonSaveData.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveData.TabIndex = 29;
            this.buttonSaveData.Text = "Save Data";
            this.buttonSaveData.UseVisualStyleBackColor = true;
            this.buttonSaveData.Click += new System.EventHandler(this.buttonSaveData_Click);
            // 
            // FormMeasurementDualChannelPresenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 461);
            this.Controls.Add(this.buttonSaveData);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxGainCH1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxGainCH0);
            this.Controls.Add(this.buttonCalculate);
            this.Name = "FormMeasurementDualChannelPresenter";
            this.Text = "FormMeasurementDualChannelPresenter";
            ((System.ComponentModel.ISupportInitialize)(this.chartCH0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCH0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCH1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCH1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGainCH0;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCH0;
        private System.Windows.Forms.DataGridView dataGridViewCH0;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn floatValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn floatValueWithGain;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCH1;
        private System.Windows.Forms.DataGridView dataGridViewCH1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxGainCH1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonSaveData;
    }
}