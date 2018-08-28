using DebugTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupercapController
{
    partial class FormMain
    {

        List<int> list;

        private void buttonCap1ChargeToValue_Click(object sender, EventArgs e)
        {
            float value, gain, tValue;
            // First parse float value
            try
            {
                value = float.Parse(textBoxCap1ChargeToValue.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid float value!");
                return;
            }


            list = DataGridHelperClass.GetSelectedIndexes(dataGridViewCap1);
            
            foreach (var index in list)
            {
                //ConfigClass.UpdateWorkingDeviceAddress(addr); 
                gain = ConfigClass.deviceGainCH1;
                tValue = value / gain;
            }
            
        }
        

        private void buttonCap1Discharge10A_Click(object sender, EventArgs e)
        {

        }

        private void buttonCap1Discharge100A_Click(object sender, EventArgs e)
        {

        }

        private void buttonCap1SetCritHigh_Click(object sender, EventArgs e)
        {

        }

        private void buttonCap1SetCritLow_Click(object sender, EventArgs e)
        {

        }

        private void buttonCap1DisableCritHigh_Click(object sender, EventArgs e)
        {

        }

        private void buttonCap1DisableCritLow_Click(object sender, EventArgs e)
        {

        }
        

        /// <summary>
        /// Fetch all selected addresses and send command sequence to all of them with given delay in seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonCap1RunCommandsFromDebug_Click(object sender, EventArgs e)
        {
            int delay;
            // First parse float value
            try
            {
                delay = int.Parse(textBoxCap1DebugDelay.Text);
                delay *= 1000; // Delay is in ms and user input is in seconds
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid float value!");
                return;
            }
#warning UNTESTED CODE
            // Reset device pool and datagrid
            //ConfigClass.DevPoolCap1.DevicePoolReset();
            //DataGridHelperClass.PopulateCapDataGrid(ConfigClass.DevPoolCap1, dataGridViewCap1);
            DataGridHelperClass.ClearStatusColorsFromDataGrid(dataGridViewCap1);

            list = DataGridHelperClass.GetSelectedIndexes(dataGridViewCap1);

            foreach (var index in list)
            {
                ConfigClass.UpdateWorkingDeviceAddress(index);
                this.Text = "Charger Controller   DEV_ADDR=" + ConfigClass.deviceAddr.ToString() + "     GainCH0=" + ConfigClass.deviceGainCH0 + "  GainCH1=" + ConfigClass.deviceGainCH1;
                
                FormCustomConsole.WriteLineWithConsole("\r\n ------------------------");
                // form test sequence
                com = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
                AppendTestSequence();
                var data = com.GetFinalCommandList();
                labelDebugBytesUsed.Text = "Bytes Used : " + data.Length;
                try
                {
                    FormCustomConsole.WriteLineWithConsole("\r\nSending commands to ID:" + index + "\r\n");
                    SerialDriver.Send(data, Cap1ExecuteSuccessCallback, Cap1ExecuteFailCallback);
                }
                catch (Exception)
                {
                    MessageBox.Show("Problem occured while trying to send data to serial port!");
                    FormCustomConsole.WriteLine("------- Commands not sent --------\r\n");
                    return;

                }
                // Reset everything
                com = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
                textBoxDebugInstructionPool.Text = "";
                // Wait delay async

                await Task.Delay(delay);
            }
            Thread t = new Thread(() =>
            {
                MessageBox.Show("******All commands sent!*****", DateTime.Now.ToString());
            });
            t.IsBackground = true;
            t.Start();
            FormCustomConsole.WriteLineWithConsole("\r\n ******All commands sent!*****");
        }


        private void Cap1ExecuteSuccessCallback(byte[] b)
        {
            // Its ACK no need to use data
            //MessageBox.Show("Commands sent successfully!");
            Invoke((MethodInvoker)delegate
            {
                ChangeColorOnDatagrid(ConfigClass.deviceAddr, System.Drawing.Color.Green);
            }
            );
            FormCustomConsole.WriteLineWithConsole("COMMANDS SENT SUCCESSFULLY TO DEVICE:" + ConfigClass.deviceAddr + "!!!");
        }

        private void Cap1ExecuteFailCallback()
        {
            //MessageBox.Show("Commands not received by device!");
            Invoke((MethodInvoker)delegate
            {
                ChangeColorOnDatagrid(ConfigClass.deviceAddr, Color.Red);
            });
            FormCustomConsole.WriteLineWithConsole("COMMANDS NOT RECEIVED BY DEVICE WITH ID:" + ConfigClass.deviceAddr + "!!!");
        }

        public void ChangeColorOnDatagrid(int index, Color col)
        {
            DataGridViewCell cell;
            if (index < ConfigClass.cap2AddrOffset)
            {
                cell = DataGridHelperClass.FindCellWithID(dataGridViewCap1, index);
            }
            else
            {
                cell = DataGridHelperClass.FindCellWithID(dataGridViewCap2, index);
            }
            if (cell != null)
            {
                cell.Style.BackColor = col;
            }
            else
                throw new Exception("Cell not found in FormMain.ChangeColorOnDataGrid()");
        }
    }
}
