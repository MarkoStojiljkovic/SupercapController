using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    /// <summary>
    /// Is used for easy creating list of commands and then converting everything to message with protocol that charger can decode (message length and checksum are calculated and appended automatically after GetFinalCommandList() )
    /// </summary>
    class CommandFormerClass
    {
        List<byte> bl; // List of command bytes
        const float delta = 10f; // If some values are exceding 16bit signed value correct them and include delta

        public CommandFormerClass(byte startSeq, byte devAddress)
        {
            bl = new List<byte>();
            bl.Add(startSeq);
            bl.Add(devAddress);
        }

        /// <summary>
        ///  Data recorder task
        /// </summary>
        /// <param name="channel"> CH0 = 0 , CH1 = 1, CH0 & CH1 = 2</param>
        /// <param name="opMode"> 1- continuous , 2 - target points </param>
        /// <param name="prescaler"> (prescaler - 1) * 10ms</param>
        /// <param name="targetPoints"> How much points should be recorder (not used in continuous mode)</param>
        public void AppendDataRecorderTask(byte channel, byte opMode, UInt32 prescaler, UInt32 targetPoints, DateTime time)
        {
            bl.Add(4);
            bl.Add(channel);
            bl.Add(opMode);
            bl.AddRange(CustomConvertorClass.ConvertIntTo4Bytes(prescaler));
            bl.AddRange(CustomConvertorClass.ConvertIntTo4Bytes(targetPoints));
            // Now append transformed time
            bl.Add((byte)(time.Year - 2000));
            bl.Add((byte)(time.Month));
            bl.Add((byte)(time.Day));
            bl.Add((byte)(time.Hour));
            bl.Add((byte)(time.Minute));
            bl.Add((byte)(time.Second));

        }

        

        public void AppendWaitForMs(UInt32 ms)
        {
            bl.Add(3);
            bl.AddRange(CustomConvertorClass.ConvertIntTo4Bytes(ms));
        }

        public void AppendDataRecFinish()
        {
            bl.Add(5);
        }

        public void AppendReadFromAddress(int address, int numOfBytes)
        {
            bl.Add(6);
            // Append address
            byte[] bAddress = CustomConvertorClass.ConvertIntTo4Bytes(address);
            bl.AddRange(bAddress);
            // Get and append length
            byte[] bLen = CustomConvertorClass.ConvertIntTo2Bytes(numOfBytes); 
            bl.AddRange(bLen);
        }

        public void AppendEraseMeasurements()
        {
            bl.Add(7);
        }

        public void AppendWaitForDataRecorderToFinish()
        {
            bl.Add(8);
        }

        public void AppendWaitForValueRising(byte channel, UInt16 latency, UInt16 targetValue)
        {
            bl.Add(9);
            bl.Add(channel);
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(latency));
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(targetValue));
        }

        public void AppendWaitForValueRising(byte channel, UInt16 latency, float targetValue)
        {
            // targt value is given in mV
            if (targetValue < 0)
            {
                targetValue = 0;
            }

            if (targetValue > 3300)
            {
                targetValue = 3300 - delta;
            }

            /*     
             maxValue = 32767


            signed int offsetCorrectedValue = (signed int)rawData - SDOffset;
            signed int offsetCorrectedValue = (signed int)rawData;
            Now gain correction
            float offsetAndGainValue = (float)offsetCorrectedValue * SDGain;
            ADC is 15bits plus sign bit which are 32767 different values
            currentValue/maxPossibleValue = x/3300  in mV
            float fValue = offsetAndGainValue*3300/32768;
            
            offsetAndGainValue = fValue * 32768 / 3300
             */
            // Now convert to uint16
            UInt16 uValue = (UInt16)(targetValue * 32768 / 3300);

            bl.Add(9);
            bl.Add(channel);
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(latency));
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(uValue));
        }

        public void AppendWaitForValueFalling(byte channel, UInt16 latency, UInt16 targetValue)
        {
            bl.Add(10);
            bl.Add(channel);
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(latency));
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(targetValue));
        }

        public void AppendWaitForValueFalling(byte channel, UInt16 latency, float targetValue)
        {
            // targt value is given in mV
            if (targetValue < 0)
            {
                targetValue = 0;
            }

            if (targetValue > 3300)
            {
                targetValue = 3300 - delta;
            }
            
            // Now convert to uint16
            UInt16 uValue = (UInt16)(targetValue * 32768 / 3300);

            bl.Add(10);
            bl.Add(channel);
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(latency));
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(uValue));
        }

        public void AppendSetCriticalLow(UInt16 targetValue, byte channel)
        {
            bl.Add(11);
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(targetValue));
            bl.Add(channel);
            
        }
        public void AppendSetCriticalLow(float targetValue, byte channel)
        {
#warning IMPROVE THIS
            // targt value is given in mV

            if (targetValue > 3300)
            {
                targetValue = 3300 - delta;
            }
            else if (targetValue < 0)
            {
                targetValue = 0;
            }
            
            // Now convert to uint16
            UInt16 uValue = (UInt16)(targetValue * 32768 / 3300);


            bl.Add(11);
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(uValue));
            bl.Add(channel);

        }

        public void AppendSetCriticalHigh(UInt16 targetValue, byte channel)
        {
            bl.Add(12);
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(targetValue));
            bl.Add(channel);
        }

        public void AppendSetCriticalHigh(float targetValue, byte channel)
        {
            // targt value is given in mV

            if (targetValue > 3300)
            {
                targetValue = 3300 - delta;
            }
            else if (targetValue < 0)
            {
                targetValue = 0;
            }
            
            // Now convert to uint16
            UInt16 uValue = (UInt16)(targetValue * 32768 / 3300);


            bl.Add(12);
            bl.AddRange(CustomConvertorClass.ConvertIntTo2Bytes(uValue));
            bl.Add(channel);
        }


        public void AppendLedOn(byte ledID)
        {
            bl.Add(0);
            bl.Add(ledID); // RED = 1, GREEN = 2, BLUE = 3
        }

        public void AppendLedOff(byte ledID)
        {
            bl.Add(1);
            bl.Add(ledID); // RED = 1, GREEN = 2, BLUE = 3
        }

        public void AppendPinSetHigh(byte pinId)
        {
            bl.Add(13);
            bl.Add(pinId); // Pins are selectable by adressing MCU pin layout
        }

        public void AppendPinSetLow(byte pinId)
        {
            bl.Add(14);
            bl.Add(pinId); // Pins are selectable by adressing MCU pin layout
        }

        public void AppendChargerOn()
        {
            bl.Add(15);
        }

        public void AppendChargerOff()
        {
            bl.Add(16);
        }

        public void AppendDischarger100AOn()
        {
            bl.Add(17);
        }

        public void AppendDischarger100AOffS1()
        {
            bl.Add(18);
        }

        public void AppendDischarger100AOffS2()
        {
            bl.Add(19);
        }

        public void AppendDischarger10AOn()
        {
            bl.Add(20);
        }

        public void AppendDischarger10AOffS1()
        {
            bl.Add(21);
        }

        public void AppendDischarger10AOffS2()
        {
            bl.Add(22);
        }

        public void AppendFanoxOn()
        {
            bl.Add(23);
        }

        public void AppendFanoxOff()
        {
            bl.Add(24);
        }

        public void AppendResOn()
        {
            bl.Add(25);
        }

        public void AppendResOff()
        {
            bl.Add(26);
        }

        public void GetGain()
        {
            bl.Add(27);
        }

        /// <summary>
        /// Calculate checksum, and append it at the end of message
        /// </summary>
        /// <returns></returns>
        public byte[] GetFinalCommandList()
        {
            var num = bl.Count;
            // First 2 bytes are skipped but checksum should be counted so overall length is unchanged, +2 - 2 = 0
            bl.InsertRange(2, CustomConvertorClass.ConvertIntTo2Bytes(num));

            UInt16 csum = ChecksumClass.CalculateChecksum(bl.ToArray());
            bl.AddRange(CustomConvertorClass.ConvertIntTo2BytesBE(csum));
            
            return bl.ToArray();
        }

    }
}
