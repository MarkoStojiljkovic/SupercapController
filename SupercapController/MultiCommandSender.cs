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
    class MultiCommandSender
    {
        static List<int> cap1IndexList;
        static int cap1IndexListCount = 0;
        static DataGridView dg;
        static FormMain form;
        static Action<CommandFormerClass> testSeq;
        static bool busy = false;
        public static bool forceStop = false;

        public static bool SendMulti(DataGridView _dg, FormMain f, Action<CommandFormerClass> _testSeq)
        {
            if (busy == true)
            {
                FormCustomConsole.WriteLineWithConsole("Multi sender busy \r\n");
                return false;
            }
            busy = true;
            dg = _dg;
            form = f;
            testSeq = _testSeq;
            DataGridHelperClass.ClearStatusColorsFromDataGrid(dg);
            forceStop = false;
            cap1IndexList = DataGridHelperClass.GetSelectedIndexes(dg);
            cap1IndexListCount = 0;
            // Send first to start a sequence, take care that first time sender is main (GUI) thread other times is thread from serial driver module
            if (cap1IndexListCount < cap1IndexList.Count) // Prevent stupid things
            {
                Cap1SendToNextDev(cap1IndexList[cap1IndexListCount++], 10);
            }
            return true;
        }

        private static void Cap1SendToNextDev(int devId, int delayMs)
        {
            ConfigClass.UpdateWorkingDeviceAddress(devId);
            form.Text = "Charger Controller   DEV_ADDR=" + ConfigClass.deviceAddr.ToString() + "     GainCH0=" + ConfigClass.deviceGainCH0 + "  GainCH1=" + ConfigClass.deviceGainCH1;

            FormCustomConsole.WriteLineWithConsole("\r\n ------------------------");
            // form test sequence
            var comTest = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            testSeq(comTest);
            var data = comTest.GetFinalCommandList();
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

        private static void SuccessCallback(byte[] b)
        {
            // Its ACK no need to use data
            FormCustomConsole.WriteLineWithConsole("COMMANDS SENT SUCCESSFULLY TO DEVICE:" + ConfigClass.deviceAddr + "!!!");
            form.Invoke((MethodInvoker)delegate
            {
                ChangeColorOnDatagrid(ConfigClass.deviceAddr, System.Drawing.Color.Green);
                if (cap1IndexListCount < cap1IndexList.Count) // Prevent stupid things
                {
                    Cap1SendToNextDev(cap1IndexList[cap1IndexListCount++], 10);
                }
                else
                {
                    busy = false;
                    FormCustomConsole.WriteLineWithConsole("\r\n------------------All commands sent!!! ---------------\r\n");
                    //NotifyEnd();
                }
            }
            );

        }

        private static void FailCallback()
        {
            FormCustomConsole.WriteLineWithConsole("COMMANDS NOT RECEIVED BY DEVICE WITH ID:" + ConfigClass.deviceAddr + "!!!");
            form.Invoke((MethodInvoker)delegate
            {
                ChangeColorOnDatagrid(ConfigClass.deviceAddr, Color.Red);
                if (cap1IndexListCount < cap1IndexList.Count) // Prevent stupid things
                {
                    Cap1SendToNextDev(cap1IndexList[cap1IndexListCount++], 10);
                }
                else
                {
                    busy = false;
                    FormCustomConsole.WriteLineWithConsole("\r\n------------------All commands sent!!! ---------------\r\n");
                    //NotifyEnd();
                }
            });

        }
        
        public static void ChangeColorOnDatagrid(int index, Color col)
        {
            DataGridViewCell cell;
            if (index < ConfigClass.cap2AddrOffset)
            {
                cell = DataGridHelperClass.FindCellWithID(dg, index);
            }
            else
            {
                //cell = DataGridHelperClass.FindCellWithID(dataGridViewCap2, index);
                throw new Exception("Not implemented for index bigger than 59");
            }
            if (cell != null)
            {
                cell.Style.BackColor = col;
            }
            else
                throw new Exception("Cell not found in FormMain.ChangeColorOnDataGrid()");
        }

        private static void NotifyEnd()
        {
            Thread t = new Thread(() =>
            {
                MessageBox.Show("******All commands sent!*****", DateTime.Now.ToString());
            });
            t.IsBackground = true;
            t.Start();
        }
    }
}
