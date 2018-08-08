using DebugTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupercapController
{
    /// <summary>
    /// Send commands to all selected devices in dataGridView, with selected time period
    /// </summary>
    class CapSequencer
    {
        int isActive = 0;
        /// <summary>
        /// When CapSequencer is active this value will show which capacitor array is being worked with
        /// </summary>
        public int IsActive
        {
            get { return isActive; }
        }
        DataGridView _dgv;

        public CapSequencer(DataGridView dgv)
        {
            _dgv = dgv;
        }

        /// <summary>
        /// Send commands based on textfields to selected cells in dataGridView
        /// </summary>
        public async void SendCommandsToSelectedCells()
        {
            // Get selected devices
            var list = ExtractSelectedCheckboxesFromDataGridView(_dgv);
            if (list.Count == 0)
            {
                MessageBox.Show("Select one or more devices first!");
                return;
            }
            // Return cell painting to default

            foreach (var item in list)
            {
                ConfigClass.UpdateWorkingDeviceAddress(item);
                // Send Commands
                // Paint the cell

                FormCustomConsole.WriteLineWithConsole("Sending to ID:" + item);
                // Wait 
                await Task.Delay(1000);
                FormCustomConsole.WriteLineWithConsole("ID:" + item + " finished");

            }
        }

        public async void ReadMeasurementsFromSelectedCells()
        {
            // Get selected devices
            var list = ExtractSelectedCheckboxesFromDataGridView(_dgv);
            if (list.Count == 0)
            {
                MessageBox.Show("Select one or more devices first!");
                return;
            }
            foreach (var item in list)
            {
                ConfigClass.UpdateWorkingDeviceAddress(item);
                
                Console.WriteLine("Sending to ID:" + item);
                // Wait 
                await Task.Delay(1000);
                Console.WriteLine("ID:" + item + " finished");

            }
        }




        private List<int> ExtractSelectedCheckboxesFromDataGridView(DataGridView dgv)
        {
            List<int> list = new List<int>();
            int rowIndex = 0;
            int colIndex = 0;

            foreach (DataGridViewRow item in dgv.Rows)
            {
                colIndex = 0;
                foreach (DataGridViewCell item2 in item.Cells)
                {
                    if (item2 is DataGridViewCheckBoxCell)
                    {
                        if (item2.Value != null && item2.Value is bool)
                        {
                            if ((bool)item2.Value)
                            {
                                list.Add((int)dgv.Rows[rowIndex].Cells[colIndex - 1].Value);
                            }
                        }
                    }
                    colIndex++;
                }
                rowIndex++;
            }

            return list;
        }
        
    }
}
