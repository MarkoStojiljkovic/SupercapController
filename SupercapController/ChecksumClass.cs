using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    class ChecksumClass
    {
        UInt16 cs = 0;


        /// <summary>
        /// Calculate checksum for whole array except last 2 bytes, which should match with calculation
        /// </summary>
        /// <param name="arr">True if calculated checksum matches sent checksum </param>
        public bool VerifyChecksumFromReceivedMessage(byte[] arr)
        {
            for (int i = 0; i < arr.Length - 2; i++)
            {
                cs += arr[i];
            }

            UInt16 givenCs = (UInt16)(arr[arr.Length - 1] << 8);
            givenCs |= arr[arr.Length - 2];

            return (givenCs == cs);
       }

        static public UInt16 CalculateChecksum(byte[] arr)
        {
            UInt16 csum = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                csum += arr[i];
            }

            return csum;
        }
    
    }
}
