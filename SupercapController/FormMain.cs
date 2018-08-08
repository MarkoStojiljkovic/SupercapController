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
        List<MeasurementHeaderClass> headers = new List<MeasurementHeaderClass>(); // Used to store received measurements headers
        int currentHeader = 0;
        
        public FormMain()
        {
            InitializeComponent();

            // Configure serial driver
            //serial.ReadTimeout = 500; // 500ms
            //serial.WriteTimeout = 500; // 500ms
            
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
    }
}
