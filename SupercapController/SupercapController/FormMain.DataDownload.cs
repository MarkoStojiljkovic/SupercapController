using DebugTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SupercapController
{
    // DataDownload Tab

    partial class FormMain 
    {
        private void buttonDataDownloadGetMeasures_Click(object sender, EventArgs e)
        {
            uartReceiver.Reset();
            processFunction = ProcessGetMeasurementInfo;
            // Send command to get measurement header
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendReadFromAddress(ConfigClass.MEASURE_INFO_BASE, ConfigClass.MEASURE_INFO_LEN);
            var data = cm.GetFinalCommandList();
            serial.Write(data, 0, data.Length);
        }

        private void buttonDataDownloadSerialConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serial.IsOpen)
                {
                    MessageBox.Show("Port is already open");
                    return;
                }
                serial.PortName = (string)comboBoxDataDownloadSerial.SelectedItem;
                serial.Open();
                labelDataDownloadSerialConnectionStatus.Text = "Ready " + (string)comboBoxDataDownloadSerial.SelectedItem;

            }
            catch (Exception ex)
            {
                labelDataDownloadSerialConnectionStatus.Text = "COM port error";
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonDataDownloadSerialDisconnect_Click(object sender, EventArgs e)
        {
            if (serial.IsOpen)
            {
                serial.Close();
            }
            labelDataDownloadSerialConnectionStatus.Text = "Not connected!";
        }

        private void buttonDataDownloadErase_Click(object sender, EventArgs e)
        {
            // Send command to get measurement header
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendEraseMeasurements();
            var data = cm.GetFinalCommandList();
            serial.Write(data, 0, data.Length);
        }

        private void buttonDataDownloadConsole_Click(object sender, EventArgs e)
        {
            FormCustomConsole cc = new FormCustomConsole();
            cc.Show();
        }
        
        private void buttonDataDownloadAddrSelect_Click(object sender, EventArgs e)
        {
            // Addresses are ranging from 1 to 116
            int addr;
            try
            {
                addr = Convert.ToInt32(textBoxDataDownloadAddrSelect.Text);
                if (addr < 1 || addr > 116) throw new Exception();
                ConfigClass.UpdateWorkingDeviceAddress(addr);
                this.Text = "Charger Controller   DEV_ADDR=" + ConfigClass.deviceAddr.ToString() + "     GainCH0=" + ConfigClass.deviceGainCH0 + "  GainCH1=" + ConfigClass.deviceGainCH1;
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter number ranging from 1 to 116");
            }
        }

        private void dataGridViewDataDownloadMesHeaders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check is it button and if it is get that header
            var senderGrid = (DataGridView)sender;

            // Check if button is clicked
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                // First select selected header
                MeasurementHeaderClass header;
                try
                {
                    header = headers[e.RowIndex];
                }
                catch (Exception)
                {
                    MessageBox.Show("Header not present");
                    return;
                }

                uartReceiver.Reset();
                processFunction = ProcessReceivedMeasurements;

                // Send command to get measurement header + data
                CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
                cm.AppendReadFromAddress(header.headerAddress, header.numOfPoints * 2 + ConfigClass.HEADER_LENGTH); // 1 data point = 2 bytes
                var data = cm.GetFinalCommandList();
                serial.Write(data, 0, data.Length);
            }
        }

    }
}
