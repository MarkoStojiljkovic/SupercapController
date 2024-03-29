﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    class ConfigClass
    {
        // Last selected device (messages will be sent to this device)
        public static byte deviceAddr = 0x31;
        public static float deviceGainCH0 = 1.0f;
        public static float deviceGainCH1 = 1.0f;

        // Total number of used devices
        public const int NUM_OF_DEVICES = 116;

        // Device pool settings
        public const int NUM_OF_DEVICES_IN_CONTAINER = 12;
        public const int NUM_OF_CONTAINERS = 10;
        public static DevicePoolSerializableClass DevPoolCap1; // Init with values from conf file, or use default
        public static DeviceAvailability DEV_POOL_NEW_1; // TEST, NOT USED
        public static DevicePoolSerializableClass DevPoolCap2; // Init with values from conf file, or use default
        public const string devPoolCap1Filename = "AvailabilityConfCap1.xml";
        public const string devPoolCap2Filename = "AvailabilityConfCap2.xml";
        public const int cap1AddrOffset = 1;  // Offset for filling device addresses in dataGrdiView
        public const int cap2AddrOffset = 59; // Offset for filling device addresses in dataGrdiView


        // These settings should match settings in firmware
        public const byte startSeq = 254;
        public const int MEASURE_INFO_BASE = 512;
        public const int MEASURE_INFO_LEN = 82;
        public const int HEADER_LENGTH = 14;



        /// <summary>
        /// Updates current working addres for sending commands
        /// </summary>
        /// <param name="id"></param>
        public static void UpdateWorkingDeviceAddress(int id)
        {
            deviceAddr = (byte)id;
            deviceGainCH0 = DeviceGainStorageSerializableClass.lastInstance.dev[id - 1].gainCH0;
            deviceGainCH1 = DeviceGainStorageSerializableClass.lastInstance.dev[id - 1].gainCH1;
        }

    }
}
