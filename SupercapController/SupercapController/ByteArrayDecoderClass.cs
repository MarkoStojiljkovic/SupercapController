using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    class ByteArrayDecoderClass
    {

        byte[] data;
        int index = 0;
         
        public ByteArrayDecoderClass(byte[] rawData)
        {
            data = rawData;
        }

        public int Get4BytesAsInt()
        {
            int value = 0;

            value = data[index++];
            value |= data[index++] << 8;
            value |= data[index++] << 16;
            value |= data[index++] << 24;

            return value;
        }

        public float Get4BytesAsFloat()
        {
            index += 4;
            float tmp = BitConverter.ToSingle(data, 2);
            return tmp;

        }

        public int Get2BytesAsInt()
        {
            int value = 0;

            value = data[index++];
            value |= data[index++] << 8;
            
            return value;
        }

        public int Get1ByteAsInt()
        {
            return data[index++];
        }

        public int[] Get6BytesAsTimestamp()
        {
            int[] timestamp = new int[6];

            for (int i = 0; i < 6; i++)
            {
                timestamp[i] = data[index++];
            }

            return timestamp;
        }


        public int Get2BytesAsInt16()
        {
            Int16 value = 0;

            value = data[index++];
            value |= (Int16)(data[index++] << 8);

            return value;
        }
    }
}
