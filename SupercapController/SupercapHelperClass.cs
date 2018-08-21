using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    class SupercapHelperClass
    {
        public static string ConvertChannelToSymbolicString(int ch)
        {
            switch (ch)
            {
                case 0:
                    return "CH0";
                case 1:
                    return "CH1";
                case 2:
                    return "DUAL CH";
                default:
                    return "Unkown channel";
            }
        }

        public static float ConvertToFloatInMiliVolts(int val, float gain)
        {
            float tmpFloat = val;
            // Represent in mV
            float fValue = (tmpFloat * 3300) / 32768;
            return fValue * gain;
        }
       
        public static float ConvertToFloatInMiliVolts(int val)
        {
            float tmpFloat = val;
            // Represent in mV
            float fValue = (tmpFloat * 3300) / 32768;
            return fValue;
        }
    }
}
