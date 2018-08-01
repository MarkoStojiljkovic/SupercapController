using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    /// <summary>
    /// Holds information about measurement
    /// </summary>
    class MeasurementHeaderClass
    {
        public int headerAddress;

        public int dataAddress;
        public int[] timestamp;
        public int prescaler;
        public int numOfPoints;
        public int operatingMode;
        public int channel;


        public MeasurementHeaderClass(int head)
        {
            headerAddress = head;
            dataAddress = headerAddress + 14; // Header is 14 bytes, data is placed just after header
        }
    }
}


// HEADER STRUCT IN C
//typedef struct measureHeader //14 bytes
//{
//    struct _timestamp
//    {
//        uint8_t year;
//        uint8_t month;
//        uint8_t day;
//        uint8_t hour;
//        uint8_t min;
//        uint8_t sec;
//    }
//    timestamp;
//  uint32_t prescaler;
//    uint16_t numOfPoints; // Number of saved points
//    uint8_t operatingMode;
//    uint8_t channelMode;
//}
//measureHeader_t;