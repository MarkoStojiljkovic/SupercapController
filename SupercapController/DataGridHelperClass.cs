using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupercapController
{
    class DataGridHelperClass
    {
        /// <summary>
        /// Populate datagrid based on config file
        /// </summary>
        /// <param name="devPool"></param>
        /// <param name="datagrid"></param>
        public static void PopulateCapDataGrid(DevicePoolSerializableClass devPool, DataGridView datagrid)
        {
            // Clear datagrid before populating with new values
            datagrid.Rows.Clear();
            datagrid.Refresh();

            Tuple<bool, bool, int> tup;
            for (int i = 0; i < ConfigClass.NUM_OF_CONTAINERS; i++)
            {
                datagrid.Rows.Add("kom" + (i + 1), 1, 0, 2, 0, 3, 0, 4, 0, 5, 0, 6, 0, 7, 0, 8, 0, 9, 0, 10, 0);
                for (int y = 0; y < ConfigClass.NUM_OF_DEVICES_IN_CONTAINER; y++)
                {
                    tup = devPool.GetNext();
                    CorrectDevicePoolGrid(i, y, tup, datagrid);

                }
            }
        }

        public static void ClearStatusColorsFromDataGrid(DataGridView dg)
        {
            foreach (DataGridViewRow row in dg.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //If cell is different from gray or white restore to white
                    if (cell.Style.BackColor != Color.LightGray)
                    {
                        cell.Style.BackColor = Color.White;
                    }
                    
                }
            }
        }


        
        /// <summary>
        /// Correct datagrid based on conifiguration file (disable or skip some cells)
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <param name="tup"></param>
        /// <param name="datagrid"></param>
        private static void CorrectDevicePoolGrid(int rowIndex, int colIndex, Tuple<bool, bool, int> tup, DataGridView datagrid)
        {
            // Adjust column index
            colIndex = (colIndex * 2) + 1;
            // Check what needs to be done with cell
            if (tup.Item2 == true) // skip ?
            {
                // Disable, fill blank and paint darker
                datagrid.Rows[rowIndex].Cells[colIndex].Value = "";
                datagrid.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                datagrid.Rows[rowIndex].Cells[colIndex++].Style.BackColor = Color.LightGray;
                datagrid.Rows[rowIndex].Cells[colIndex].Value = 0; // Deselect checkbox
                datagrid.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                datagrid.Rows[rowIndex].Cells[colIndex].Style.BackColor = Color.LightGray;
                return;
            }
            else if (tup.Item1 != true) // Enabled ?
            {
                // Disable and paint darker
                datagrid.Rows[rowIndex].Cells[colIndex].Value = tup.Item3;
                datagrid.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                datagrid.Rows[rowIndex].Cells[colIndex++].Style.BackColor = Color.LightGray;
                datagrid.Rows[rowIndex].Cells[colIndex].Value = 0; // Deselect checkbox
                datagrid.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
                datagrid.Rows[rowIndex].Cells[colIndex].Style.BackColor = Color.LightGray;
                return;
            }
            // Not skipped and enabled, fill with values
            datagrid.Rows[rowIndex].Cells[colIndex].Value = tup.Item3;
            datagrid.Rows[rowIndex].Cells[colIndex++].ReadOnly = true;
            datagrid.Rows[rowIndex].Cells[colIndex].Value = 0; // Deselect checkbox
        }


        /// <summary>
        /// Get all selected device addresses from datagrid
        /// </summary>
        /// <param name="dg"></param>
        /// <returns></returns>
        public static List<int> GetSelectedIndexes(DataGridView dg)
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

        public static DataGridViewCell FindCellWithID(DataGridView dg, int index)
        {
            foreach (DataGridViewRow row in dg.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //if (!cell.ReadOnly) // Skip disabled cells
                    //{

                        int temp;
                        if (int.TryParse(cell.Value.ToString(), out temp))
                        {
                            if (temp == index)
                            {
                                // We found cell that matches that ID
                                return cell;
                            }
                        }
                    //}
                }
            }
            return null;
        }
    }
}
