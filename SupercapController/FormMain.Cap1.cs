using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupercapController
{
    partial class FormMain
    {

        List<int> list;
        int currentIndex = 0;

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


            list = GetSelectedIndexes(dataGridViewCap1);
            
            foreach (var index in list)
            {
                ConfigClass.UpdateWorkingDeviceAddress(addr); 
                gain = ConfigClass.deviceGainCH1;
                tValue = value / gain;
            }
            
        }


        private int GetNext()
        {
            if (list.Count == currentIndex)
            {
                return -1;
            }

            return list[currentIndex++];
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

        private List<int> GetSelectedIndexes(DataGridView dg)
        {
            List<int> list = new List<int>();
            
            foreach (DataGridViewRow row in dg.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //do operations with cell
                    if (cell is DataGridViewCheckBoxCell)
                    {
                        if (Convert.ToBoolean(cell.Value) == true)
                        {
                            list.Add(Convert.ToInt32(row.Cells[cell.ColumnIndex - 1].Value));
                        }
                        
                    }
                }
            }
            return list;
        }
    }
}
