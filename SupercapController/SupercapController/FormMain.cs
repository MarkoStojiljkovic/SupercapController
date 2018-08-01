using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using DebugTools;
using System.Globalization;
using System.Threading;

namespace SupercapController
{
    public partial class FormMain : Form
    {
        byte[] tempSerialReceiveBuff = new byte[1]; // Copy to this buffer on every data received event
        SerialPort serial = new SerialPort();
        UARTDataReceiverClass uartReceiver = new UARTDataReceiverClass();
        // Store received headers
        List<MeasurementHeaderClass> headers = new List<MeasurementHeaderClass>();
        int currentHeader = 0;

        delegate void ProcessUARTData(byte[] data); // Declare type for fucntion pointer
        ProcessUARTData processFunction = Dummy; // Default processing method

        public FormMain()
        {
            InitializeComponent();

            // Configure serial driver
            serial.ReadTimeout = 500; // 500ms
            serial.WriteTimeout = 500; // 500ms
            serial.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived); // Subscribe to fucking handler

            SetThreadCultureInfo(); // Select CultureInfo for this thread

            StartupConfigClass.Init();
            ConfigClass.UpdateWorkingDeviceAddress(0x31); // Select addr 0x31 as default
            this.Text = "Charger Controller   DEV_ADDR=" + ConfigClass.deviceAddr.ToString() + "     GainCH0=" + ConfigClass.deviceGainCH0 + "  GainCH1=" + ConfigClass.deviceGainCH1;

            #region Data Download
            // Initialize all controlls from DataDownload tab
            comboBoxDataDownloadSerial.Items.AddRange(SerialPort.GetPortNames());


            #endregion

            #region Debug
            com = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            #endregion
#warning DEBUGGING DATAGRIDVIEW
            
            PopulateCapDataGrid(ConfigClass.DevPoolCap1, dataGridViewCap1);
            PopulateCapDataGrid(ConfigClass.DevPoolCap2, dataGridViewCap2);
        }

        private void PopulateCapDataGrid(DevicePoolSerializableClass devPool, DataGridView datagrid )
        {
            Tuple<bool, bool, int> tup;
            for (int i = 0; i < ConfigClass.NUM_OF_CONTAINERS; i++)
            {
                datagrid.Rows.Add("kom" + (i+1), 1, 0, 2, 0, 3, 0, 4, 0, 5, 0, 6, 0, 7, 0, 8, 0, 9, 0, 10, 0);
                for (int y = 0; y < ConfigClass.NUM_OF_DEVICES_IN_CONTAINER; y++)
                {
                    tup = devPool.GetNext();
                    CorrectDevicePoolGrid(i,y, tup, datagrid);

                }
            }
        }

        /// <summary>
        /// Correct datagrid based on conifiguration file (disable or skip some cells)
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <param name="tup"></param>
        /// <param name="datagrid"></param>
        private void CorrectDevicePoolGrid(int rowIndex,int colIndex, Tuple<bool, bool, int> tup, DataGridView datagrid)
        {
            // Adjust column index
            colIndex = (colIndex * 2) + 1;
            // Check what needs to be done with cell
            if (tup.Item2 == true) // skip ?
            {
                // Disable, fill blank and paint darker
                datagrid.Rows[rowIndex].Cells[colIndex].Value = "";
                datagrid.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                datagrid.Rows[rowIndex].Cells[colIndex++].Style.BackColor = Color.LightGray;
                datagrid.Rows[rowIndex].Cells[colIndex].Value = 0; // Deselect checkbox
                datagrid.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                datagrid.Rows[rowIndex].Cells[colIndex].Style.BackColor = Color.LightGray;
                return;
            }
            else if (tup.Item1 != true) // Enabled ?
            {
                // Disable and paint darker
                datagrid.Rows[rowIndex].Cells[colIndex].Value = tup.Item3;
                datagrid.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                datagrid.Rows[rowIndex].Cells[colIndex++].Style.BackColor = Color.LightGray;
                datagrid.Rows[rowIndex].Cells[colIndex].Value = 0; // Deselect checkbox
                datagrid.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                datagrid.Rows[rowIndex].Cells[colIndex].Style.BackColor = Color.LightGray;
                return;
            }
            // Not skipped and enabled, fill with values
            datagrid.Rows[rowIndex].Cells[colIndex].Value = tup.Item3;
            datagrid.Rows[rowIndex].Cells[colIndex++].ReadOnly = true;
            datagrid.Rows[rowIndex].Cells[colIndex].Value = 0; // Deselect checkbox
        }

        /// <summary>
        /// Keep all culture settings except NumberDecimalSeparator (which will be '.')
        /// </summary>
        private void SetThreadCultureInfo()
        {
            //NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat.Clone() as NumberFormatInfo;
            CultureInfo nfi = CultureInfo.CurrentCulture.Clone() as CultureInfo;
            nfi.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = nfi;
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            tempSerialReceiveBuff = new byte[serial.BytesToRead]; // Maybe not needed anymore
            serial.Read(tempSerialReceiveBuff, 0, tempSerialReceiveBuff.Length);
            if (uartReceiver.CollectData(tempSerialReceiveBuff))
            {
                // All data collected, process it based on pressed button
                processFunction(uartReceiver.bData);
            }
        }

        /// <summary>
        /// Default method for processing UART received data
        /// </summary>
        /// <param name="b"></param>
        private static void Dummy(byte[] b)
        {
            Console.WriteLine("Dummy");
            FormCustomConsole.WriteLine("Dummy function called, this could be some kind of error");
        }

        /// <summary>
        /// Measurement info is received, decode data and send requests to read measurement headers
        /// </summary>
        /// <param name="b"></param>
        private void ProcessGetMeasurementInfo(byte[] b)
        {
            // Check checksum and remove from message
            ChecksumClass cs = new ChecksumClass();
            if (cs.VerifyChecksumFromReceivedMessage(b))
                FormCustomConsole.WriteLine("CHECKSUM MATCH");
            else
            {
                FormCustomConsole.WriteLine("CHECKSUM NOT VALID");
                return;
            }

            var newArr = UARTHelperClass.RemoveChecksumFromMessage(b);


            ByteArrayDecoderClass decoder = new ByteArrayDecoderClass(newArr);

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
                processFunction = Dummy;
                uartReceiver.Reset(); // Just to be sure
                return;
            }
            headers = new List<MeasurementHeaderClass>(); // Reset headers list
            // For now only header addresses are received, which can be used to fetch headers
            for (int i = 0; i < numOfMeasurements; i++)
            {
                headers.Add(new MeasurementHeaderClass(decoder.Get4BytesAsInt()));
            }

            // Reset UART receiver and set new method for data processing
            uartReceiver.Reset();
            processFunction = FetchHeaders;
            currentHeader = 0; // Reset header index


            // Bootstrap
            // Send command to get measurement header
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendReadFromAddress(headers[0].headerAddress, ConfigClass.HEADER_LENGTH);
            var data = cm.GetFinalCommandList();
            serial.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Message which contains header is received, extract header from it, populate DataGridView and isue a new header request if needed
        /// </summary>
        /// <param name="b"> Message which contains header </param>
        private void FetchHeaders(byte[] b)
        {
            // Check checksum and remove from message
            ChecksumClass cs = new ChecksumClass();
            if (cs.VerifyChecksumFromReceivedMessage(b))
            {
                FormCustomConsole.WriteLine("Fetch headers CHECKSUM MATCH");
                Console.WriteLine("Fetch headers CHECKSUM MATCH");
            }
            else
            {
                // Header is corrupted, abort everything
                FormCustomConsole.WriteLine("Fetch headers CHECKSUM FAILED");
                Console.WriteLine("Fetch headers CHECKSUM FAILED");
                processFunction = Dummy;
                uartReceiver.Reset();
                return;
            }

            var newArr = UARTHelperClass.RemoveChecksumFromMessage(b);


            ByteArrayDecoderClass decoder = new ByteArrayDecoderClass(newArr);
            decoder.Get2BytesAsInt(); // Remove first 2 values (number of data received)
            
            // Fill header with remaining data
            headers[currentHeader].timestamp = decoder.Get6BytesAsTimestamp();
            headers[currentHeader].prescaler = decoder.Get4BytesAsInt();
            headers[currentHeader].numOfPoints = decoder.Get2BytesAsInt();
            headers[currentHeader].operatingMode = decoder.Get1ByteAsInt();
            headers[currentHeader].channel = decoder.Get1ByteAsInt();

            PopulateDataGridWithHeader(headers[currentHeader], currentHeader);
            uartReceiver.Reset();

            currentHeader++;
            if (currentHeader == headers.Count) // Last time in here, reset everything
            {
                processFunction = Dummy;
                return;
            }
            
            // Send command to get measurement header
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendReadFromAddress(headers[currentHeader].headerAddress, ConfigClass.HEADER_LENGTH);
            var data = cm.GetFinalCommandList();
            serial.Write(data, 0, data.Length);

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
                head.operatingMode.ToString(), head.channel.ToString(), "KELP");
            }));
        }
        
        

        /// <summary>
        /// When measurement data is downloaded (measurement header included), this fucntion should be called for processing and display
        /// </summary>
        /// <param name="b">Message containing header + data</param>
        private void ProcessReceivedMeasurements(byte[] b)
        {
            uartReceiver.Reset();
            // Check checksum and remove from message
            ChecksumClass cs = new ChecksumClass();
            if (cs.VerifyChecksumFromReceivedMessage(b))
                FormCustomConsole.WriteLine("ProcessReceivedMeasurements CHECKSUM MATCH");
            else
            {
                FormCustomConsole.WriteLine("ProcessReceivedMeasurements CHECKSUM DONT MATCH");
                return;
            }

            var newArr = UARTHelperClass.RemoveChecksumFromMessage(b);

            FormCustomConsole.WriteLine("in ProcessReceivedMeasurements, read bytes: " + (newArr.Length - ConfigClass.HEADER_LENGTH - 2)); // -2 for data len

            int channel = b[15];
            if (channel == 0 || channel == 1)
            {
                this.Invoke(new Action(() =>
                {
                    FormMeasurementSingleChannelPresenter fdp = new FormMeasurementSingleChannelPresenter(newArr);
                    fdp.Show();
                }));
            }
            else if (channel == 2)
            {
                this.Invoke(new Action(() =>
                {
                    FormMeasurementDualChannelPresenter fdp = new FormMeasurementDualChannelPresenter(newArr);
                    fdp.Show();
                }));
            }
            else
            {
                FormCustomConsole.WriteLine("Channel not recognized");
            }
        }

        
    }
}
