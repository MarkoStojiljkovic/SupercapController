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


        // Empty constructor for serialization
        public DevicePoolSerializableClass()
        { }

        private void FillPool(int offset)
        {
            int containerIndex = 0;
            // Create first container
            DevUnit[] c1 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 3; i++)
            {
                c1[i] = new DevUnit(offset++, true, false);
            }
            for (int i = 3; i < 10; i++)
            {
                c1[i] = new DevUnit(0, false, true);
            }
            pool[containerIndex] = new DeviceContainer(c1, ++containerIndex);

            // 2 container
            DevUnit[] c2 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 3; i++)
            {
                c2[i] = new DevUnit(offset++, true, false);
            }
            for (int i = 3; i < 10; i++)
            {
                c2[i] = new DevUnit(0, false, true);
            }
            pool[containerIndex] = new DeviceContainer(c2, ++containerIndex);

            // 3 container
            DevUnit[] c3 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 3; i++)
            {
                c3[i] = new DevUnit(offset++, true, false);
            }
            for (int i = 3; i < 10; i++)
            {
                c3[i] = new DevUnit(0, false, true);
            }
            pool[containerIndex] = new DeviceContainer(c3, ++containerIndex);

            // 4 container
            DevUnit[] c4 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 3; i++)
            {
                c4[i] = new DevUnit(offset++, true, false);
            }
            for (int i = 3; i < 10; i++)
            {
                c4[i] = new DevUnit(0, false, true);
            }
            pool[containerIndex] = new DeviceContainer(c4, ++containerIndex);

            // 5 container
            DevUnit[] c5 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 6; i++)
            {
                c5[i] = new DevUnit(offset++, true, false);
            }
            for (int i = 6; i < 10; i++)
            {
                c5[i] = new DevUnit(0, false, true);
            }
            pool[containerIndex] = new DeviceContainer(c5, ++containerIndex);

            //  6 container
            DevUnit[] c6 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 6; i++)
            {
                c6[i] = new DevUnit(offset++, true, false);
            }
            for (int i = 6; i < 10; i++)
            {
                c6[i] = new DevUnit(0, false, true);
            }
            pool[containerIndex] = new DeviceContainer(c6, ++containerIndex);

            // 7 Container
            DevUnit[] c7 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 6; i++)
            {
                c7[i] = new DevUnit(offset++, true, false);
            }
            for (int i = 6; i < 10; i++)
            {
                c7[i] = new DevUnit(0, false, true);
            }
            pool[containerIndex] = new DeviceContainer(c7, ++containerIndex);

            // 8 Container
            DevUnit[] c8 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 9; i++)
            {
                c8[i] = new DevUnit(offset++, true, false);
            }
            for (int i = 9; i < 10; i++)
            {
                c8[i] = new DevUnit(0, false, true);
            }
            pool[containerIndex] = new DeviceContainer(c8, ++containerIndex);

            // Container 9
            DevUnit[] c9 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 9; i++)
            {
                c9[i] = new DevUnit(offset++, true, false);
            }
            for (int i = 9; i < 10; i++)
            {
                c9[i] = new DevUnit(0, false, true);
            }
            pool[containerIndex] = new DeviceContainer(c9, ++containerIndex);

            // Container 10
            DevUnit[] c10 = new DevUnit[ConfigClass.NUM_OF_DEVICES_IN_CONTAINER];
            for (int i = 0; i < 10; i++)
            {
                c10[i] = new DevUnit(offset++, true, false);
            }
            pool[containerIndex] = new DeviceContainer(c10, ++containerIndex);
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
        const int NUM_OF_DEVICES = 10;
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
