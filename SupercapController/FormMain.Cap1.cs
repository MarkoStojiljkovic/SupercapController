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
        

        /// <summary>
        /// Fetch all selected addresses and send command sequence to all of them with given delay in seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonCap1SendTestSeq_Click(object sender, EventArgs e)
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

        private void buttonCap1SelectAll_Click(object sender, EventArgs e)
        {
            DataGridHelperClass.SelectAll(dataGridViewCap1);
        }

        private void buttonCap1DeselectAll_Click(object sender, EventArgs e)
        {
            DataGridHelperClass.DeselectAll(dataGridViewCap1);
        }

        private void buttonCap1ForceStop_Click(object sender, EventArgs e)
        {
            MultiCommandSender.forceStop = true;
        }



        private void buttonCap1ActivateFanox_Click(object sender, EventArgs e)
        {
            MultiCommandSender.SendMulti(dataGridViewCap1, this, Cap1FanoxOnTestSeq);
        }

        private void buttonCap1DeactivateFanox_Click(object sender, EventArgs e)
        {
            MultiCommandSender.SendMulti(dataGridViewCap1, this, Cap1FanoxOffTestSeq);
        }

        private void buttonCap1Ping_Click(object sender, EventArgs e)
        {
            MultiCommandSender.SendMulti(dataGridViewCap1, this, Cap1PingTestSeq);
        }

        private void buttonCap1GetVoltage_Click(object sender, EventArgs e)
        {
            var list = DataGridHelperClass.GetSelectedIndexes(dataGridViewCap1);
            if (list.Count == 0) return; // Ignore if none device is selected
            FormMultiVoltageViewer f = new FormMultiVoltageViewer(list, this);
            f.Show();
        }




        #region TEST SEQUENCES
        private void Cap1FanoxOnTestSeq(CommandFormerClass co)
        {
            // Request return ACK
            co.ReturnACK();
            // Fanox on
            co.AppendFanoxOn();
        }

        private void Cap1FanoxOffTestSeq(CommandFormerClass co)
        {
            // Request return ACK
            co.ReturnACK();
            // Fanox off
            co.AppendFanoxOff();
        }

        private void Cap1PingTestSeq(CommandFormerClass co)
        {
            // Request return ACK
            co.ReturnACK();
        }
        #endregion TEST SEQUENCE
    }
}
