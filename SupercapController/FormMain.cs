﻿using System;
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
            
            DataGridHelperClass.PopulateCapDataGrid(ConfigClass.DevPoolCap1, dataGridViewCap1);
            DataGridHelperClass.PopulateCapDataGrid(ConfigClass.DevPoolCap2, dataGridViewCap2);
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
