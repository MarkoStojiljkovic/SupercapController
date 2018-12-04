using DebugTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupercapController
{
    public partial class FormMultiVoltageViewer : Form
    {
        List<int> deviceList;
        int index = 0;
        bool forceStop = false;
        bool busy = false;
        FormMain form;

        public FormMultiVoltageViewer(List<int> _list, FormMain _form)
        {
            form = _form;
            deviceList = _list;
            InitializeComponent();
            this.Text = "Voltage Viewer";
            InitDatagrid();
        }


        void InitDatagrid()
        {
            foreach (var item in deviceList)
            {
                dataGridView1.Rows.Add(item.ToString());
            }
        }

        private void buttonGetVoltage_Click(object sender, EventArgs e)
        {
            if (busy == true)
            {
                FormCustomConsole.WriteLineWithConsole("Multi sender busy \r\n");
                return;
            }
            index = 0;
            busy = true;
            // First add column

            dataGridView1.Columns.Add("Mes" + dataGridView1.Columns.Count.ToString(), DateTime.Now.ToString());
            labelStatus.Text = "Pending";
            labelStatus.ForeColor = Color.Red;
            GetVoltage(deviceList[index++], 10);
            
        }


        private void GetVoltage(int devId, int delayMs)
        {
            ConfigClass.UpdateWorkingDeviceAddress(devId);
            form.Text = "Charger Controller   DEV_ADDR=" + ConfigClass.deviceAddr.ToString() + "     GainCH0=" + ConfigClass.deviceGainCH0 + "  GainCH1=" + ConfigClass.deviceGainCH1;

            // Send command to get last ADC sample
            FormCustomConsole.WriteLineWithConsole("\r\n ------------------------");
            // form test sequence
            var com = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            com.GetLastADCSample(1);
            var data = com.GetFinalCommandList();
            try
            {
                Thread.Sleep(delayMs);
                if (forceStop)
                {
                    FormCustomConsole.WriteLineWithConsole("Multi sending aborted\r\n");
                    busy = false;
                    return;
                }
                FormCustomConsole.WriteLineWithConsole("\r\nSending commands to ID:" + devId + "\r\n");
                if (!SerialDriver.Send(data, SuccessCallback, FailCallback))
                {
                    Console.WriteLine("Serial Driver busy!!");
                    busy = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Problem occured while trying to send data to serial port!");
                FormCustomConsole.WriteLine("------- Commands not sent --------\r\n");
            }
        }

        private void SuccessCallback(byte[] obj)
        {
            FormCustomConsole.WriteLineWithConsole("COMMANDS SENT SUCCESSFULLY TO DEVICE:" + ConfigClass.deviceAddr + "!!!");
            this.Invoke((MethodInvoker)delegate
            {
                //ChangeColorOnDatagrid(ConfigClass.deviceAddr, System.Drawing.Color.Green);
                // Calculate result
                ByteArrayDecoderClass decoder = new ByteArrayDecoderClass(obj);

                decoder.Get2BytesAsInt(); // Ignore first 2 values (message length)
                                          // Get raw value and convert to human readable values, with and without gain
                var rawValue = decoder.Get2BytesAsInt16();
                float fValueGain = SupercapHelperClass.ConvertToFloatInMiliVolts(rawValue, ConfigClass.deviceGainCH1);

                if (index < deviceList.Count) // Prevent stupid things
                {
                    dataGridView1.Rows[index - 1].Cells[dataGridView1.Columns.Count - 1].Value = fValueGain;
                    GetVoltage(deviceList[index++], 10);
                }
                else
                {
                    busy = false;
                    dataGridView1.Rows[index - 1].Cells[dataGridView1.Columns.Count - 1].Value = fValueGain;
                    labelStatus.Text = "Ready";
                    labelStatus.ForeColor = Color.Green;
                    FormCustomConsole.WriteLineWithConsole("\r\n------------------All commands sent!!! ---------------\r\n");
                    //NotifyEnd();
                }
            }
            );
        }

        private void FailCallback()
        {
            FormCustomConsole.WriteLineWithConsole("COMMANDS NOT RECEIVED BY DEVICE WITH ID:" + ConfigClass.deviceAddr + "!!!");
            this.Invoke((MethodInvoker)delegate
            {
                //ChangeColorOnDatagrid(ConfigClass.deviceAddr, System.Drawing.Color.Green);
                
                if (index < deviceList.Count) // Prevent stupid things
                {
                    dataGridView1.Rows[index - 1].Cells[dataGridView1.Columns.Count - 1].Value = 0;
                    dataGridView1.Rows[index - 1].Cells[dataGridView1.Columns.Count - 1].Style.BackColor = Color.Red;
                    GetVoltage(deviceList[index++], 10);
                }
                else
                {
                    busy = false;
                    dataGridView1.Rows[index - 1].Cells[dataGridView1.Columns.Count - 1].Value = 0;
                    dataGridView1.Rows[index - 1].Cells[dataGridView1.Columns.Count - 1].Style.BackColor = Color.Red;
                    labelStatus.Text = "Ready";
                    labelStatus.ForeColor = Color.Green;
                    FormCustomConsole.WriteLineWithConsole("\r\n------------------All commands sent!!! ---------------\r\n");
                    //NotifyEnd();
                }
            }
            );
        }
    }
}
