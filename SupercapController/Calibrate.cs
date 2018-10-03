using DebugTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupercapController
{
    public partial class Calibrate : Form
    {
        List<CalibratePoint> calibValues = new List<CalibratePoint>();
        int numberOfSamples = 0;

        public Calibrate()
        {
            InitializeComponent();
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
