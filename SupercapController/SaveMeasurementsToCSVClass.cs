using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SupercapController
{
    class SaveMeasurementsToCSVClass
    {
        /// <summary>
        /// Save measurements in CSV file, named by dev address, date, channel etc... Gain from config file will be included
        /// </summary>
        /// <param name="head"></param>
        /// <param name="values"></param>
        /// <param name="ch"></param>
        public static void Save(MeasurementHeaderClass head, int[] values, int ch)
        {
            // Infer channel gain
            float chGain;
            if (ch == 0)
                chGain = ConfigClass.deviceGainCH0;
            else if (ch == 1)
                chGain = ConfigClass.deviceGainCH1;
            else
            {
                MessageBox.Show("Channel gain error");
                return;
            }

            // First convert to desired format
            StringBuilder sb = new StringBuilder();
            sb.Append("Device Addess: " + ConfigClass.deviceAddr + "\r\n");
            sb.Append("Channel: " + ch + "\r\n");
            sb.Append("Included gain value: " + chGain.ToString() + "\r\n");
            sb.Append("Prescaler: " + head.prescaler + "\r\n");
            sb.Append("Number of data points: " + head.numOfPoints + "\r\n");
            sb.Append("Start:\r\n");
            
            foreach (var item in values)
            {
                sb.Append(SupercapHelperClass.ConvertToFloatInMiliVolts(item, chGain).ToString() + ",");
            }
            // Remove last ','
            sb.Remove(sb.Length - 1, 1);

            // Create file name
            string time = (head.timestamp[0] + 2000).ToString() + "_" + head.timestamp[1].ToString() + "_" + head.timestamp[2].ToString() + "_" +
                head.timestamp[3].ToString() + "_" + head.timestamp[4].ToString() + "_" + head.timestamp[5].ToString();
            string filename = "dev" + ConfigClass.deviceAddr + "_" + time + "_" + SupercapHelperClass.ConvertChannelToSymbolicString(ch) + ".csv";



            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            
            saveFileDialog1.Filter = "CSV files |.csv";
            saveFileDialog1.RestoreDirectory = true; // Remember where we saved last file
            saveFileDialog1.FileName = filename; // Default name

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //if ((myStream = saveFileDialog1.OpenFile()) != null)
                //{
                //    // Code to write the stream goes here.
                //    myStream.Close();
                //}
                using (StreamWriter sw = new StreamWriter(File.Create(saveFileDialog1.FileName)))
                {
                    try
                    {
                        sw.Write(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }



            //DialogResult dialogResult = MessageBox.Show("Save as: " + filename, "Save File", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    //do something
            //    string path = AppDomain.CurrentDomain.BaseDirectory + filename;
            //    using (StreamWriter sw = new StreamWriter(File.Create(path)))
            //    {
            //        try
            //        {
            //            sw.Write(sb.ToString());
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }
            //    }

            //}
            //else if (dialogResult == DialogResult.No)
            //{
            //    //You dont want to save data :(
            //}

        }
    }
}
