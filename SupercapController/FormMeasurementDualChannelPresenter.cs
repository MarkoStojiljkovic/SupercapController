using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SupercapController
{
    public partial class FormMeasurementDualChannelPresenter : Form
    {
        int[] valuesCH0, valuesCH1;
        MeasurementHeaderClass selectedHeader;

        public FormMeasurementDualChannelPresenter(byte[] raw)
        {
            InitializeComponent();
            ByteArrayDecoderClass dec = new ByteArrayDecoderClass(raw);
            // Extract data len without header (2 measurements are stored in raw thats why / 4)
            int dataLen = (dec.Get2BytesAsInt() - ConfigClass.HEADER_LENGTH) / 4;

            // Remove header from data packet
            selectedHeader = new MeasurementHeaderClass(0); // Dummy address
            selectedHeader.timestamp = dec.Get6BytesAsTimestamp();
            selectedHeader.prescaler = dec.Get4BytesAsInt();
            selectedHeader.numOfPoints = dec.Get2BytesAsInt();
            selectedHeader.operatingMode = dec.Get1ByteAsInt();
            selectedHeader.channel = dec.Get1ByteAsInt();

            // Set gain textboxes default values from config class
            textBoxGainCH0.Text = ConfigClass.deviceGainCH0.ToString();
            textBoxGainCH1.Text = ConfigClass.deviceGainCH1.ToString();

            // Form timestamp string
            string time = (selectedHeader.timestamp[0] + 2000).ToString() + "/" + selectedHeader.timestamp[1].ToString() + "/" + 
                selectedHeader.timestamp[2].ToString() + " " + selectedHeader.timestamp[3].ToString() + ":" + 
                selectedHeader.timestamp[4].ToString() + ":" + selectedHeader.timestamp[5].ToString();
            string channel = SupercapHelperClass.ConvertChannelToSymbolicString(selectedHeader.channel);

            // Change name of this form
            this.Text = channel + "  " + time;


            valuesCH0 = new int[dataLen];
            valuesCH1 = new int[dataLen];

            for (int i = 0; i < dataLen; i++)
            {
                valuesCH0[i] = dec.Get2BytesAsInt16();
                valuesCH1[i] = dec.Get2BytesAsInt16();
            }
        }
        
        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            float gainCH0, gainCH1;
            try
            {
                string fvalueCh0 = textBoxGainCH0.Text;
                string fvalueCh1 = textBoxGainCH1.Text;
                gainCH0 = float.Parse(fvalueCh0);
                gainCH1 = float.Parse(fvalueCh1);
            }
            catch (Exception)
            {
                MessageBox.Show("Enter valid float number for gain");
                return;
            }

            // First clear all graphs and dataGrids
            // Datagrid 1
            dataGridViewCH0.DataSource = null;
            dataGridViewCH0.Rows.Clear();
            chartCH0.Series["mV"].Points.Clear();
            // Datagrid 2
            dataGridViewCH1.DataSource = null;
            dataGridViewCH1.Rows.Clear();
            chartCH1.Series["mV"].Points.Clear();

            // Calculate CH0 channel graph and fill its dataGrid values
            for (int i = 0; i < valuesCH0.Length; i++)
            {
                dataGridViewCH0.Rows.Add();
                dataGridViewCH0.Rows[i].Cells[0].Value = i + 1; // Index
                float fValueGain = SupercapHelperClass.ConvertToFloatInMiliVolts(valuesCH0[i], gainCH0);
                dataGridViewCH0.Rows[i].Cells[1].Value = valuesCH0[i].ToString(); // Raw
                dataGridViewCH0.Rows[i].Cells[2].Value = SupercapHelperClass.ConvertToFloatInMiliVolts(valuesCH0[i]).ToString(); // float
                dataGridViewCH0.Rows[i].Cells[3].Value = fValueGain.ToString(); // float with gain
                chartCH0.Series["mV"].Points.AddXY(i, fValueGain);
            }

            // Calculate CH1 channel graph and fill its dataGrid values
            for (int i = 0; i < valuesCH1.Length; i++)
            {
                dataGridViewCH1.Rows.Add();
                dataGridViewCH1.Rows[i].Cells[0].Value = i + 1; // Index
                float fValueGain = SupercapHelperClass.ConvertToFloatInMiliVolts(valuesCH1[i], gainCH1);
                dataGridViewCH1.Rows[i].Cells[1].Value = valuesCH1[i].ToString(); // Raw
                dataGridViewCH1.Rows[i].Cells[2].Value = SupercapHelperClass.ConvertToFloatInMiliVolts(valuesCH1[i]).ToString(); // float
                dataGridViewCH1.Rows[i].Cells[3].Value = fValueGain.ToString(); // float with gain
                chartCH1.Series["mV"].Points.AddXY(i, fValueGain);
            }
        }

        private void buttonSaveDataRaw_Click(object sender, EventArgs e)
        {
            SaveMeasurementsToCSVClass.SaveRaw(selectedHeader, valuesCH0, 0);
            SaveMeasurementsToCSVClass.SaveRaw(selectedHeader, valuesCH1, 1);
        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            SaveMeasurementsToCSVClass.Save(selectedHeader, valuesCH0, 0);
            SaveMeasurementsToCSVClass.Save(selectedHeader, valuesCH1, 1);
        }
    }
}
