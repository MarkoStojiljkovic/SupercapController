using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    class UARTHelperClass
    {
        /// <summary>
        /// Remove checksum from data packet, and updates data length accordingly
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] RemoveChecksumFromMessage(byte[] msg)
        {
            // Copy whole array except last two bytes
            byte[] newMsg = new byte[msg.Length - 2];
            
            for (int i = 0; i < newMsg.Length; i++)
            {
                newMsg[i] = msg[i];
            }

            // Remove checksum count from message header
            int len = newMsg.Length - 2; // number of data bytes
            byte[] temp = CustomConvertorClass.ConvertIntTo2Bytes(len);
            newMsg[0] = temp[1];
            newMsg[1] = temp[0];

            return newMsg;
        }
    }
}
