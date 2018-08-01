using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    class CustomConvertorClass
    {
        public static byte[] ConvertIntTo4Bytes(int val)
        {
            byte[] intBytes = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            return intBytes;
        }

        public static byte[] ConvertIntTo4Bytes(UInt32 val)
        {
            byte[] intBytes = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            return intBytes;
        }

        // Inverts result, should be big endian
        public static byte[] ConvertIntTo2Bytes(int temp)
        {
            byte[] varb = new byte[2];

            
            varb[0] = (byte)(temp >> 8);
            varb[1] = (byte)temp;
            return varb;
        }

        // Big endian
        public static byte[] ConvertIntTo2BytesBE(int temp)
        {
            byte[] varb = new byte[2];


            varb[0] = (byte)(temp);
            varb[1] = (byte)(temp >> 8);
            return varb;
        }

        public static byte ConvertIntTo1Byte(int temp)
        {
            return (byte)temp;
        }
        
        public static int ConvertRawStringToInt(string s)
        {
            if (s.Length != 2) throw new Exception("String must be of length 2");

            int temp = (byte)s[0];
            temp += (byte)s[1] * 16;

            return temp;
        }

        public static int GetMessageLength(byte[] b) // Remove if all goes well
        {
            int temp = b[0];
            temp |= b[1] << 8;
            return temp;
        }


        public static int Decode2BytesToInt(byte[] ba, int startingIndex)
        {
            int temp = ba[startingIndex++];
            return temp |= ba[startingIndex] << 8;

        }
    }
}
