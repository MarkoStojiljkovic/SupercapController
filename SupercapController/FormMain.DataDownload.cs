using DebugTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SupercapController
{
    // DataDownload Tab

    partial class FormMain 
    {
        private void buttonDataDownloadGetMeasures_Click(object sender, EventArgs e)
        {
            // Send command to get measurement header
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendReadFromAddress(ConfigClass.MEASURE_INFO_BASE, ConfigClass.MEASURE_INFO_LEN);
            var data = cm.GetFinalCommandList();
            //Console.WriteLine("ProcessGetMeasurementInfo from buttonDataDownloadGetMeasures_Click");
            SerialDriver.Send(data, ProcessGetMeasurementInfo, FailCallback);

        }

        private void buttonDataDownloadSerialConnect_Click(object sender, EventArgs e)
        {
            labelDataDownloadSerialConnectionStatus.Text = SerialDriver.Open((string)comboBoxDataDownloadSerial.SelectedItem);
        }

        private void buttonDataDownloadSerialDisconnect_Click(object sender, EventArgs e)
        {
            SerialDriver.Close();
            labelDataDownloadSerialConnectionStatus.Text = "Not connected!";
        }

        private void buttonDataDownloadErase_Click(object sender, EventArgs e)
        {
            // Send command to get measurement header
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendEraseMeasurements();
            cm.ReturnACK();
            var data = cm.GetFinalCommandList();
            SerialDriver.Send(data, SuccsessCallback, FailCallback);
        }

        /// <summary>
        /// Get current voltage from device (current ADC sample from CH1)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDataDownloadGetVoltage_Click(object sender, EventArgs e)
        {
            // Send command to get last ADC sample
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.GetLastADCSample(1);
            var data = cm.GetFinalCommandList();
            SerialDriver.Send(data, GetADCResultCallbackCH1, FailCallback);
        }
        
        void SuccsessCallback(byte[] b)
        {
            FormCustomConsole.WriteLineWithConsole("Success!!");
        }

        void FailCallback()
        {
            FormCustomConsole.WriteLineWithConsole("Fail!!");
        }


        void GetADCResultCallbackCH1(byte[] b)
        {
            ByteArrayDecoderClass decoder = new ByteArrayDecoderClass(b);

            decoder.Get2BytesAsInt(); // Ignore first 2 values (message length)
            // Get raw value and convert to human readable values, with and without gain
            var rawValue = decoder.Get2BytesAsInt16();
            float fValueGain = SupercapHelperClass.ConvertToFloatInMiliVolts(rawValue, ConfigClass.deviceGainCH1);
            float fValue = SupercapHelperClass.ConvertToFloatInMiliVolts(rawValue);
            // Show them in separate thread so you dont block current
            Thread t = new Thread(() =>
            {
                string s = "Raw ADC: " + rawValue + "\r\nValue in mV: " + fValue +  "\r\nValue in mV with gain: " + fValueGain;
                MessageBox.Show(s, DateTime.Now.ToString());
            });
            t.IsBackground = true;
            t.Start();
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
                buttonDebugResetInstructions_Click(this, EventArgs.Empty);
                // Reset DataGrid
                dataGridViewDataDownloadMesHeaders.DataSource = null;
                dataGridViewDataDownloadMesHeaders.Rows.Clear();
                dataGridViewDataDownloadMesHeaders.Refresh();
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

                //uartReceiver.Reset();
                //processFunction = ProcessReceivedMeasurements;

                // Send command to get measurement header + data
                CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
                cm.AppendReadFromAddress(header.headerAddress, header.numOfPoints * 2 + ConfigClass.HEADER_LENGTH); // 1 data point = 2 bytes
                var data = cm.GetFinalCommandList();
                SerialDriver.Send(data, ProcessReceivedMeasurements, FailCallback);
            }
        }


        /// <summary>
        /// When measurement data is downloaded (measurement header included), this fucntion should be called for processing and display
        /// </summary>
        /// <param name="b">Message containing header + data</param>
        private void ProcessReceivedMeasurements(byte[] b)
        {
            FormCustomConsole.WriteLine("in ProcessReceivedMeasurements, read bytes: " + (b.Length - ConfigClass.HEADER_LENGTH - 2)); // -2 for data len

            int channel = b[15];
            if (channel == 0 || channel == 1)
            {
                this.Invoke(new Action(() =>
                {
                    FormMeasurementSingleChannelPresenter fdp = new FormMeasurementSingleChannelPresenter(b);
                    fdp.Show();
                }));
            }
            else if (channel == 2)
            {
                this.Invoke(new Action(() =>
                {
                    FormMeasurementDualChannelPresenter fdp = new FormMeasurementDualChannelPresenter(b);
                    fdp.Show();
                }));
            }
            else
            {
                FormCustomConsole.WriteLine("Channel not recognized");
            }
        }
        
        /// <summary>
        /// Measurement info is received, decode data and send requests to read measurement headers
        /// </summary>
        /// <param name="b"></param>
        private void ProcessGetMeasurementInfo(byte[] b)
        {
            ByteArrayDecoderClass decoder = new ByteArrayDecoderClass(b);

            decoder.Get2BytesAsInt(); // Ignore first 2 values (message length)
            this.Invoke(new Action(() =>
            {
                dataGridViewDataDownloadMesHeaders.DataSource = null;
                dataGridViewDataDownloadMesHeaders.Rows.Clear();
                dataGridViewDataDownloadMesHeaders.Refresh();
            }));

            // Get number of measurements and their header starting addresses 
            int numOfMeasurements = decoder.Get2BytesAsInt();
            FormCustomConsole.WriteLine("Number of measurements: " + numOfMeasurements);
            Console.WriteLine("Number of measurements: " + numOfMeasurements);
            if (numOfMeasurements == 0)
            {
                // if no measurements leave it as it is
                MessageBox.Show("No saved measurements in device");
                return;
            }
            headers = new List<MeasurementHeaderClass>(); // Reset headers list
            // For now only header addresses are received, which can be used to fetch headers
            for (int i = 0; i < numOfMeasurements; i++)
            {
                headers.Add(new MeasurementHeaderClass(decoder.Get4BytesAsInt()));
            }
            currentHeader = 0; // Reset header index

            // Bootstrap
            // Send command to get measurement header
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendReadFromAddress(headers[0].headerAddress, ConfigClass.HEADER_LENGTH);
            var data = cm.GetFinalCommandList();
            //Console.WriteLine("Fetching headers from ProcessGetMeasurementInfo");
            SerialDriver.Send(data, FetchHeaders, FailCallback);
        }

        /// <summary>
        /// Message which contains header is received, extract header from it, populate DataGridView and isue a new header request if needed
        /// </summary>
        /// <param name="b"> Message which contains header </param>
        private void FetchHeaders(byte[] b)
        {
            ByteArrayDecoderClass decoder = new ByteArrayDecoderClass(b);
            decoder.Get2BytesAsInt(); // Remove first 2 values (number of data received)

            // Fill header with remaining data
            headers[currentHeader].timestamp = decoder.Get6BytesAsTimestamp();
            headers[currentHeader].prescaler = decoder.Get4BytesAsInt();
            headers[currentHeader].numOfPoints = decoder.Get2BytesAsInt();
            headers[currentHeader].operatingMode = decoder.Get1ByteAsInt();
            headers[currentHeader].channel = decoder.Get1ByteAsInt();

            PopulateDataGridWithHeader(headers[currentHeader], currentHeader);

            currentHeader++;
            if (currentHeader == headers.Count) // Last time in here, reset everything
            {
                //Console.WriteLine("Done fetching headers");
                return;
            }

            // Send command to get measurement header
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendReadFromAddress(headers[currentHeader].headerAddress, ConfigClass.HEADER_LENGTH);
            var data = cm.GetFinalCommandList();
            //Console.WriteLine("Fetching headers from Fetching");
            Thread.Sleep(10); // MCU cant handle very fast UART tasks
            SerialDriver.Send(data, FetchHeaders, FailCallback);
        }

        /// <summary>
        /// Populate DataGridView which is used for storing headers with current received header
        /// </summary>
        /// <param name="head"></param>
        /// <param name="currentHeader"></param>
        private void PopulateDataGridWithHeader(MeasurementHeaderClass head, int currentHeader)
        {
            // Form timestamp string first
            string time = (head.timestamp[0] + 2000).ToString() + "/" + head.timestamp[1].ToString() + "/" + head.timestamp[2].ToString() + " " +
                head.timestamp[3].ToString() + ":" + head.timestamp[4].ToString() + ":" + head.timestamp[5].ToString();


            this.Invoke(new Action(() =>
            {
                dataGridViewDataDownloadMesHeaders.Rows.Add(head.headerAddress.ToString(), time, head.prescaler.ToString(), head.numOfPoints.ToString(),
                head.operatingMode.ToString(), head.channel.ToString(), "FETCH");
            }));
        }


        
        /// <summary>
        /// Download 4 measurements from device, which are selected from datagrid and pack them in desired form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDataDownloadMultiDownload_Click(object sender, EventArgs e)
        {
            var cells = dataGridViewDataDownloadMesHeaders.SelectedCells;

            // Extract only row numbers 
            List<int> rows = new List<int>();
            foreach (DataGridViewCell item in cells)
            {
                rows.Add(item.RowIndex);
            }
            // Remove duplicates
            List<int> uniqueRows = rows.Distinct().ToList();
            // Sort
            uniqueRows.Sort();
            // Now check if there are 4 rows selected
            if (uniqueRows.Count != 4)
            {
                MessageBox.Show("Select exactly 4 rows!!!");
                return;
            }
            // Now form all data that needs to be sent

            // Update label
            labelMultiDownload.Text = "Pending!";
            labelMultiDownload.ForeColor = System.Drawing.Color.Black;
            
            // First select selected header
            MeasurementHeaderClass header;

            // Make key value pairs of addresses and data length that needs to be read
            var kvList = new List<Tuple<int, int>>();
            foreach (var index in uniqueRows)
            {
                header = headers[index];
                kvList.Add(new Tuple<int, int>(header.headerAddress, header.numOfPoints * 2 + ConfigClass.HEADER_LENGTH));

            }

            if (!MultiDownloader.Download(kvList, MultiSuccess, MultiFail))
            {
                MessageBox.Show("Request not sent!");
            }
        }


        static string defaultPath = AppDomain.CurrentDomain.BaseDirectory; // Used to remember state of last picked folder
        private void MultiSuccess(List<byte[]> list)
        {
            Invoke((MethodInvoker)delegate
            {
                labelMultiDownload.Text = "Success!";
                labelMultiDownload.ForeColor = System.Drawing.Color.Green;
                const string delimiter = "\r\n";
                Console.WriteLine("Success from MULTI");
                //string path = AppDomain.CurrentDomain.BaseDirectory;
                string filenameCH0;
                string filenameCH1;
                string midleFix = textBoxDataDownloadCapacitorDescription.Text;
                int[] valuesCH0, valuesCH1;
                MeasurementHeaderClass selectedHeader;
                string[] postFixes = { "", "_C1", "_C2", "_R" };
                int postFixIndex = 0;


                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.SelectedPath = defaultPath;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    defaultPath = dialog.SelectedPath + "\\";
                }
                else
                {
                    // If no folder is selected just abort everything :(
                    labelMultiDownload.Text = "Canceled!";
                    labelMultiDownload.ForeColor = System.Drawing.Color.Black;
                    return;
                }


                foreach (var item in list)
                {
                    ByteArrayDecoderClass dec = new ByteArrayDecoderClass(item);
                    // Extract data len without header (2 measurements are stored in raw thats why / 4)
                    int dataLen = (dec.Get2BytesAsInt() - ConfigClass.HEADER_LENGTH) / 4;

                    // Remove header from data packet
                    selectedHeader = new MeasurementHeaderClass(0); // Dummy address
                    selectedHeader.timestamp = dec.Get6BytesAsTimestamp();
                    selectedHeader.prescaler = dec.Get4BytesAsInt();
                    selectedHeader.numOfPoints = dec.Get2BytesAsInt();
                    selectedHeader.operatingMode = dec.Get1ByteAsInt();
                    selectedHeader.channel = dec.Get1ByteAsInt();

                    valuesCH0 = new int[dataLen];
                    valuesCH1 = new int[dataLen];

                    for (int i = 0; i < dataLen; i++)
                    {
                        valuesCH0[i] = dec.Get2BytesAsInt16();
                        valuesCH1[i] = dec.Get2BytesAsInt16();
                    }
                    // Values are extracted so far, now form filename

                    // Form timestamp string
                    string time = (selectedHeader.timestamp[0] + 2000).ToString() + "_" + selectedHeader.timestamp[1].ToString() + "_" +
                        selectedHeader.timestamp[2].ToString() + "__" + selectedHeader.timestamp[3].ToString() + "-" +
                        selectedHeader.timestamp[4].ToString() + "-" + selectedHeader.timestamp[5].ToString();
                    //string channel = SupercapHelperClass.ConvertChannelToSymbolicString(selectedHeader.channel);

                    filenameCH0 = "dev" + ConfigClass.deviceAddr + "_" + midleFix + "_" + time + "_" + "CH0" + postFixes[postFixIndex];
                    filenameCH1 = "dev" + ConfigClass.deviceAddr + "_" + midleFix + "_" + time + "_" + "CH1" + postFixes[postFixIndex];
                    SaveMeasurementsToCSVClass.Save(valuesCH0, 0, defaultPath + filenameCH0 + ".csv", delimiter);
                    SaveMeasurementsToCSVClass.Save(valuesCH1, 1, defaultPath + filenameCH1 + ".csv", delimiter);
                    postFixIndex++;
                }
            }
            );
            
            Console.WriteLine("All Done!!!!");
        }


        private void MultiFail()
        {
            Invoke((MethodInvoker)delegate
            {
                labelMultiDownload.Text = "Fail!";
                labelMultiDownload.ForeColor = System.Drawing.Color.Red;
            }
            );
        }

        private void buttonDataDownloadCalibrate_Click(object sender, EventArgs e)
        {
            Calibrate cf = new Calibrate();
            cf.Show();
        }

    } // End Class
}
