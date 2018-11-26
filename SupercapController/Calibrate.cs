using DebugTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupercapController
{
    public partial class Calibrate : Form
    {
        List<CalibratePoint> calibValues = new List<CalibratePoint>();
        int numberOfSamples = 0;

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        public Calibrate()
        {
            InitializeComponent();
            dataGridView1.Rows.Add("0", "0");
            dataGridView1.Rows.Add("0", "0");
            //dataGridView1.Rows.Add("0", "0");
            //dataGridView1.Rows.Add("0", "0");
        }

        private void buttonTakeSample_Click(object sender, EventArgs e)
        {
            // Send command to get last ADC sample
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.GetLastADCSample(1);
            var data = cm.GetFinalCommandList();
            SerialDriver.Send(data, SuccessCallback, FailCallback);
        }

        private void button1_Click(object sender, EventArgs e) // Clear button clicked
        {
            calibValues = new List<CalibratePoint>();
            numberOfSamples = 0;
            textBoxDisplay.Text = "";
            textBoxGain.Text = "Average";
        }

        void FailCallback()
        {
            FormCustomConsole.WriteLineWithConsole("Fail To Calibrate!!");
        }

        private void SuccessCallback(byte[] b)
        {
            Invoke((MethodInvoker)delegate
            {
                ByteArrayDecoderClass decoder = new ByteArrayDecoderClass(b);

                decoder.Get2BytesAsInt(); // Ignore first 2 values (message length)
                                          // Get raw value and convert to human readable values, with and without gain
                var rawValue = decoder.Get2BytesAsInt16();
                float fValue = SupercapHelperClass.ConvertToFloatInMiliVolts(rawValue);
                float givenValue;
                if (!float.TryParse(textBoxFixedVoltage.Text, out givenValue))
                {
                    FormCustomConsole.WriteLineWithConsole("Wrong float value for calibration!");
                    return;
                }
                // Add to list
                var calPt = new CalibratePoint(rawValue, fValue, givenValue);
                calibValues.Add(calPt);
                numberOfSamples++;
                // Append to display
                AppendToDisplay(calPt);
                // Calculate average
                CalculateAverage();
            }
            );
        }

        private void CalculateAverage()
        {
            float sum = 0;
            foreach (var item in calibValues)
            {
                sum += item.CalculateGain();
            }
            // Calculate average gain
            sum /= numberOfSamples;
            textBoxGain.Text = "Average gain: " + sum.ToString();
        }

        private void AppendToDisplay(CalibratePoint cp)
        {
            string newLine = "Raw: " + cp.rawValue +  "    sampled: " + cp.sampledValue + "   given: " + cp.givenValue + "   gain: " + cp.CalculateGain();
            textBoxDisplay.Text += "\r\n" + newLine;
        }

        private void buttonCopyGain_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in calibValues)
            {
                sb.Append(item.CalculateGain() + "\r\n");
            }
            Clipboard.SetText(sb.ToString());
        }

        private void textBoxFixedVoltage_Enter(object sender, EventArgs e)
        {
            if (textBoxFixedVoltage.Text == "") return;
            int tmp, final;
            final = Convert.ToInt32(textBoxFixedVoltage.Text);
            //Check if auto decrement is enabled
            if (checkBox1.Checked)
            {
                tmp = Convert.ToInt32(textBoxAutoDecrement.Text);
                tmp = final - tmp;
                if (tmp >= 0) // Dont allow negative values
                {
                    final = tmp;
                }
            }

            textBoxFixedVoltage.Text = final.ToString();
            textBoxFixedVoltage.SelectAll();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            Point pt = new Point();
            GetCursorPos(ref pt);
            labelMousePosX.Text = "PosX: " + pt.X.ToString();
            labelMousePosY.Text = "PosY: " + pt.Y.ToString();
        }

        private void Calibrate_KeyPress(object sender, KeyPressEventArgs e)
        {
            Point pt = new Point();
            GetCursorPos(ref pt);

            switch (e.KeyChar)
            {
                case '/':
                    dataGridView1.Rows[0].SetValues(pt.X.ToString(), pt.Y.ToString());
                    break;
                case '*':
                    dataGridView1.Rows[1].SetValues(pt.X.ToString(), pt.Y.ToString());
                    break;
                //case '-':
                //    dataGridView1.Rows[2].SetValues(pt.X.ToString(), pt.Y.ToString());
                //    break;
                //case '+':
                //    dataGridView1.Rows[3].SetValues(pt.X.ToString(), pt.Y.ToString());
                //    break;
                default:
                    break;
            }
        }

        private void buttonCalib100A_Click(object sender, EventArgs e)
        {
            // Send commands for calibration
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.ReturnACK();
            cm.AppendDischarger100AOn();
            cm.AppendWaitForMs(2000);
            cm.AppendDataRecorderTask(0, 2, 0, 150, DateTime.Now.AddSeconds(2)); // Take into consideration 2 sec wait above
            cm.AppendWaitForMs(1000);
            cm.AppendDischarger100AOffS1();
            cm.AppendWaitForMs(200);
            cm.AppendDischarger100AOffS2();
            var data = cm.GetFinalCommandList();
            SerialDriver.Send(data, SuccessCallbackCurrent100A, FailCallback);

            // Now enable GW INSTEC recording

            Point pt = new Point(Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value), Convert.ToInt32(dataGridView1.Rows[0].Cells[1].Value));
            Cursor.Position = pt;
            mouse_event(MOUSEEVENTF_LEFTDOWN, pt.X, pt.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, pt.X, pt.Y, 0, 0);
            Thread.Sleep(100);
            // Configrm recording
            pt = new Point(Convert.ToInt32(dataGridView1.Rows[1].Cells[0].Value), Convert.ToInt32(dataGridView1.Rows[1].Cells[1].Value));
            Cursor.Position = pt;
            mouse_event(MOUSEEVENTF_LEFTDOWN, pt.X, pt.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, pt.X, pt.Y, 0, 0);
            Thread.Sleep(2900);
            // Disable measures
            pt = new Point(Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value), Convert.ToInt32(dataGridView1.Rows[0].Cells[1].Value));
            Cursor.Position = pt;
            mouse_event(MOUSEEVENTF_LEFTDOWN, pt.X, pt.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, pt.X, pt.Y, 0, 0);
        }

        private void SuccessCallbackCurrent100A(byte[] obj)
        {
            Console.WriteLine("Finished 100A");
        }

        private void buttonCalib10A_Click(object sender, EventArgs e)
        {
            // Send commands for calibration
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.ReturnACK();
            cm.AppendDischarger10AOn();
            cm.AppendWaitForMs(2000);
            cm.AppendDataRecorderTask(0, 2, 0, 300, DateTime.Now.AddSeconds(2)); // Take into consideration 2 sec wait above
            cm.AppendWaitForMs(2000);
            cm.AppendDischarger10AOffS1();
            cm.AppendWaitForMs(200);
            cm.AppendDischarger10AOffS2();
            var data = cm.GetFinalCommandList();
            SerialDriver.Send(data, SuccessCallbackCurrent100A, FailCallback);

            // Now enable GW INSTEC recording
            Point pt = new Point(Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value), Convert.ToInt32(dataGridView1.Rows[0].Cells[1].Value));
            Cursor.Position = pt;
            mouse_event(MOUSEEVENTF_LEFTDOWN, pt.X, pt.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, pt.X, pt.Y, 0, 0);
            Thread.Sleep(100);
            // Configrm recording
            pt = new Point(Convert.ToInt32(dataGridView1.Rows[1].Cells[0].Value), Convert.ToInt32(dataGridView1.Rows[1].Cells[1].Value));
            Cursor.Position = pt;
            mouse_event(MOUSEEVENTF_LEFTDOWN, pt.X, pt.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, pt.X, pt.Y, 0, 0);
            Thread.Sleep(3900);
            // Disable measures
            pt = new Point(Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value), Convert.ToInt32(dataGridView1.Rows[0].Cells[1].Value));
            Cursor.Position = pt;
            mouse_event(MOUSEEVENTF_LEFTDOWN, pt.X, pt.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, pt.X, pt.Y, 0, 0);
        }
    }

    public class CalibratePoint
    {
        public int rawValue;
        public float sampledValue;
        public float givenValue;


        public CalibratePoint(int raw, float sampled, float given)
        {
            sampledValue = sampled;
            givenValue = given;
            rawValue = raw;
        }

        public float CalculateGain()
        {
            return givenValue / sampledValue;
        }

    }
}
