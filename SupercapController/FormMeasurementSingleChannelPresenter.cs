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
    public partial class FormMeasurementSingleChannelPresenter : Form
    {
        int[] values;
        MeasurementHeaderClass selectedHeader;

        public FormMeasurementSingleChannelPresenter(byte[] raw)
        {
            InitializeComponent();

            ByteArrayDecoderClass dec = new ByteArrayDecoderClass(raw);
            // Extract data len without header
            int dataLen = (dec.Get2BytesAsInt() - ConfigClass.HEADER_LENGTH) / 2;

            // Remove header from raw
            selectedHeader = new MeasurementHeaderClass(0); // Dummy address
            selectedHeader.timestamp = dec.Get6BytesAsTimestamp();
            selectedHeader.prescaler = dec.Get4BytesAsInt(); 
            selectedHeader.numOfPoints = dec.Get2BytesAsInt();
            selectedHeader.operatingMode = dec.Get1ByteAsInt();
            selectedHeader.channel = dec.Get1ByteAsInt();
            
            //int[] ts = dec.Get6BytesAsTimestamp(); // Remove timestamp
            //dec.Get4BytesAsInt(); // Remove prescaler
            //dec.Get2BytesAsInt(); // Remove num of points
            //dec.Get1ByteAsInt(); // Remove operating mode
            //int ch = dec.Get1ByteAsInt(); // Remove channel

            // Form timestamp string
            string time = (selectedHeader.timestamp[0] + 2000).ToString() + "/" + selectedHeader.timestamp[1].ToString() + "/" + 
                selectedHeader.timestamp[2].ToString() + " " +
                selectedHeader.timestamp[3].ToString() + ":" + selectedHeader.timestamp[4].ToString() + ":" + selectedHeader.timestamp[5].ToString();
            string channel = SupercapHelperClass.ConvertChannelToSymbolicString(selectedHeader.channel);

            // Change name of this form
            this.Text = channel + "  " + time;


            values = new int[dataLen];
            //int value = dec.Get2BytesAsInt16();

            for (int i = 0; i < dataLen; i++)
            {
                values[i] = dec.Get2BytesAsInt16();
            }
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            float gainCH;
            try
            {
                //string fvalueCh0 = textBoxGainCH0.Text.Replace('.', ',');
                //string fvalueCh1 = textBoxGainCH1.Text.Replace('.', ',');
                string fvalueCh0 = textBoxGain.Text;
                gainCH = float.Parse(fvalueCh0);
            }
            catch (Exception)
            {
                MessageBox.Show("Enter valid float number for gain");
                return;
            }

            // First clear all graphs and dataGrids
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            chart1.Series["mV"].Points.Clear();
            
            // Calculate dual channel graph and fill its dataGrid values
            for (int i = 0; i < values.Length; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = i + 1; // Index
                //int tmpInt = values[i];
                //float tmpFloat = tmpInt;
                //// Represent in mV
                //float fValue = (tmpFloat * 3300) / 32768;
                float fValueGain = SupercapHelperClass.ConvertToFloatInMiliVolts(values[i], gainCH);
                dataGridView1.Rows[i].Cells[1].Value = values[i].ToString(); // Raw
                dataGridView1.Rows[i].Cells[2].Value = SupercapHelperClass.ConvertToFloatInMiliVolts(values[i]).ToString(); // float
                dataGridView1.Rows[i].Cells[3].Value = fValueGain.ToString(); // float with gain
                chart1.Series["mV"].Points.AddXY(i, fValueGain);
            }
        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            SaveMeasurementsToCSVClass.Save(selectedHeader, values, selectedHeader.channel);
        }
    }
}
