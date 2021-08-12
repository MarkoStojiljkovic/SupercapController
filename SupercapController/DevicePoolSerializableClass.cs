using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercapController
{
    [Serializable]
    public class DevicePoolSerializableClass
    {
        [NonSerialized]
        private int cntContainer, cntDev;


        public DeviceContainer[] pool = new DeviceContainer[ConfigClass.NUM_OF_CONTAINERS];
        
        public DevicePoolSerializableClass(int offset)
        {
            // Initialize with default values
            FillPool(offset);
            cntContainer = 0;
            cntDev = 0;
        }

        public void DevicePoolReset()
        {
            cntContainer = 0;
            cntDev = 0;
        }


        // Empty constructor for serialization
        public DevicePoolSerializableClass()
        { }

        private void FillPool(int offset)
        {
            int containerIndex = 0;

            for (int z = 0; z < ConfigClass.NUM_OF_CONTAINERS; z++)
            {
                DevUnit[] c = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
                for (int i = 0; i < ConfigClass.NUM_OF_DEVICES_IN_CONTAINER; i++)
                {
                    c[i] = new DevUnit(0, false, true);
                }
                pool[containerIndex] = new DeviceContainer(c, ++containerIndex);
            }
            return;
        }

        public Tuple<bool, bool, int> GetNext()
        {
            if (cntDev == ConfigClass.NUM_OF_DEVICES_IN_CONTAINER)
            {
                cntContainer++;
                cntDev = 0; // Start from start
                // Check did we used up all containers
                if (cntContainer == ConfigClass.NUM_OF_CONTAINERS)
                {
                    // This is overflow condition
                    throw new Exception("Overflow exception while fetching data from DevicePool");
                }
            }
            int adr = pool[cntContainer].container[cntDev].devAddr;
            bool en = pool[cntContainer].container[cntDev].enabled;
            bool skip = pool[cntContainer].container[cntDev++].skip;
            return new Tuple<bool, bool, int>(en, skip, adr);
        }
        
    }

    [Serializable]
    public class DeviceContainer
    {
        public int containerID;
        public DevUnit[] container;

        public DeviceContainer(DevUnit[] devList, int id)
        {
            container = devList;
            containerID = id;
        }

        public DeviceContainer()
        { }
    }

    [Serializable]
    public class DevUnit
    {
        public bool enabled = false;
        public bool skip = true;
        public int devAddr = 0;

        public DevUnit(int addr, bool en, bool sk)
        {
            devAddr = addr;
            enabled = en;
            skip = sk;
        }

        public DevUnit()
        { }
    }
}
