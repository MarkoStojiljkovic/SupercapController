using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SupercapController
{
    class InputValidatorHelperClass
    {

        public static byte GetChModeFromComboBox(ComboBox cb)
        {
            switch ((string)cb.SelectedItem)
            {
                case "CH0":
                    return 0;
                case "CH1":
                    return 1;
                case "DUAL":
                    return 2;
                default:
                    throw new Exception("Select channel");
            }
        }

        public static byte GetOperationModeFromComboBox(ComboBox cb)
        {
            switch ((string)cb.SelectedItem)
            {
                case "Continious":
                    return 1;
                case "Target Points":
                    return 2;
                default:
                    throw new Exception("Select Operation Mode");
            }
        }
    }
}