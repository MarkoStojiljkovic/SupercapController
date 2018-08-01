using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace SupercapController
{
    enum UARTResult { Done, Error, WaitMoreData };

    /// <summary>
    /// Class used for extracting data from custom protocol messages, needs implementation of auto reset after some timeout timeout
    /// </summary>
    class UARTDataReceiverClass
    {
        

        int dataLen = 0;
        List<byte> fragmentedData = new List<byte>();
        int state = 0;
        public byte[] bData = null;

        /// <summary>
        /// Collects all raw data from serial port and store them in internal buffer until whole message is received (data need to be sent in custom protocol)
        /// </summary>
        /// <param name="data">Received data fragment</param>
        /// <returns>Result</returns>
        public UARTResult CollectData(byte[] data)
        {
            switch (state)
            {
                case 0:
                    // First time, try to parse data len that will be received
                    fragmentedData.AddRange(data);
                    if (fragmentedData.Count > 2)
                    {
                        // We can extract data length
                        dataLen = CustomConvertorClass.Decode2BytesToInt(data, 0);
                        // Check if we received all data at once
                        if (dataLen + 2 <= fragmentedData.Count)
                        {
                            // Everything arrived at once
                            state = 0; //DEBUG
                            bData = fragmentedData.ToArray();
                            return UARTResult.Done;
                        }

                        state = 2;
                    }
                    // We received just first char, we need one more to form dataLen
                    state = 1;
                    return UARTResult.WaitMoreData;
                case 1:
                    // We received enough data to infer total length, check if we received all data too
                    fragmentedData.AddRange(data);
                    dataLen = CustomConvertorClass.Decode2BytesToInt(fragmentedData.ToArray(), 0);
                    // Check if we received all data at once
                    if (dataLen + 2 <= fragmentedData.Count)
                    {
                        // Everything arrived at once
                        state = 0;
                        bData = fragmentedData.ToArray();
                        return UARTResult.Done; ;
                    }
                    state = 2;
                    return UARTResult.WaitMoreData;
                case 2:
#warning MAKE IT PRECISE NOT "<="
                    fragmentedData.AddRange(data);
                    if (dataLen + 2 <= fragmentedData.Count)
                    {
                        // Everything arrived at once
                        state = 0;
                        bData = fragmentedData.ToArray();
                        return UARTResult.Done;
                    }
                    return UARTResult.WaitMoreData;
                default:
                    break;
            }
            return UARTResult.Error;
        }


        public void Reset()
        {
            dataLen = 0;
            fragmentedData = new List<byte>();
            state = 0;
            bData = null;
        }
    }
}
