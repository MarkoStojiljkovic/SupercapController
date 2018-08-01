using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    /// <summary>
    /// Class that stores configurations for all devices, it has static reference which is always updated to point at last instance (like singleton class)
    /// </summary>
    [Serializable]
    public class DeviceGainStorageSerializableClass
    {
        public const string fileName = "DeviceConfig.xml";
        
        public static DeviceGainStorageSerializableClass lastInstance;
        // Fill with default values and overwrite if config file exists
        public DeviceGainInfo[] dev = new DeviceGainInfo[ConfigClass.NUM_OF_DEVICES];
        public DeviceGainStorageSerializableClass()
        {
            for (int i = 0; i < ConfigClass.NUM_OF_DEVICES; i++)
            {
                dev[i] = new DeviceGainInfo(i + 1);
            }
            lastInstance = this;
        }
    }

    /// <summary>
    /// Simple class that stores device data which should be serialized/deserialized
    /// </summary>
    [Serializable]
    public class DeviceGainInfo
    {
        public int devAddr;
        // Maybe add more gain values
        public float gainCH0 = 1.0f;
        public float gainCH1 = 1.0f;
        
        public DeviceGainInfo(int i)
        {
            devAddr = i;
        }

        // Needs a default concstructor for serialization
        public DeviceGainInfo()
        { }
    }
}
