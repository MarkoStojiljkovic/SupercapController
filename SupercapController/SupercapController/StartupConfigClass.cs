using DebugTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace SupercapController
{
    class StartupConfigClass
    {
        static string path = AppDomain.CurrentDomain.BaseDirectory;

        public static void Init()
        {
            // Initialize gain
            GainConfInit();
            DeviceAvailabilityInit();
        }

        private static bool CreateFile(string filename)
        {
            string fullPath = path + filename;
            if (!File.Exists(fullPath))
            {
                using (File.Create(fullPath))
                {
                    return true;
                }
            }
            return false;
        }

        private static void GainConfInit()
        {
            string fullPath = path + DeviceGainStorageSerializableClass.fileName;
            if (!File.Exists(fullPath))
            {
                // File does not exist, create default file and store it in application folder
                CreateFile(DeviceGainStorageSerializableClass.fileName);
                using (var stream = new FileStream(DeviceGainStorageSerializableClass.fileName, FileMode.Create))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(DeviceGainStorageSerializableClass));
                    xml.Serialize(stream, new DeviceGainStorageSerializableClass());
                }
            }
            else
            {
                // File already exists, load it in memory
                try
                {
                    using (var stream = new FileStream(DeviceGainStorageSerializableClass.fileName, FileMode.Open))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(DeviceGainStorageSerializableClass));
                        // No need to asign result, static reference will be updated in constructor
                        xml.Deserialize(stream);
                    }
                }
                catch (Exception)
                {
                    new DeviceGainStorageSerializableClass(); // Force default values (static reference will be updated)
                    FormCustomConsole.WriteLine("Problem occurred while loading gain configuration file, default values are applied");
                    Console.WriteLine("Problem occurred while loading gain configuration file, default values are applied");
                    System.Windows.Forms.MessageBox.Show("Problem occurred while loading gain configuration file, default values are applied");
                }
            }
            return;
        }

        private static void DeviceAvailabilityInit()
        {
            // Init configurations for both capacitors 
            // Capacitor 1 init
            string fullPath = path + ConfigClass.devPoolCap1Filename;
            if (!File.Exists(fullPath))
            {
                // File does not exist, create default file and store it in application folder
                CreateFile(ConfigClass.devPoolCap1Filename);
                using (var stream = new FileStream(ConfigClass.devPoolCap1Filename, FileMode.Create))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(DevicePoolSerializableClass));
                    ConfigClass.DevPoolCap1 = new DevicePoolSerializableClass(ConfigClass.cap1AddrOffset);
                    xml.Serialize(stream, ConfigClass.DevPoolCap1);
                }
            }
            else
            {
                // File already exists, load it in memory
                try
                {
                    using (var stream = new FileStream(ConfigClass.devPoolCap1Filename, FileMode.Open))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(DevicePoolSerializableClass));
                        // No need to asign result, static reference will be updated in constructor
                        ConfigClass.DevPoolCap1 = (DevicePoolSerializableClass)xml.Deserialize(stream);
                    }
                }
                catch (Exception)
                {
                    ConfigClass.DevPoolCap1 = new DevicePoolSerializableClass(ConfigClass.cap1AddrOffset); // Force default values
                    FormCustomConsole.WriteLine("Problem occurred while initializing availability for Capacitor 1, default values are applied");
                    Console.WriteLine("Problem occurred while initializing availability for Capacitor 1, default values are applied");
                    System.Windows.Forms.MessageBox.Show("Problem occurred while initializing availability for Capacitor 1, default values are applied");
                }

            }

            // Capacitor 2 init
            fullPath = path + ConfigClass.devPoolCap2Filename;
            if (!File.Exists(fullPath))
            {
                // File does not exist, create default file and store it in application folder
                CreateFile(ConfigClass.devPoolCap2Filename);
                using (var stream = new FileStream(ConfigClass.devPoolCap2Filename, FileMode.Create))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(DevicePoolSerializableClass));
                    ConfigClass.DevPoolCap2 = new DevicePoolSerializableClass(ConfigClass.cap2AddrOffset);
                    xml.Serialize(stream, ConfigClass.DevPoolCap2);
                }
            }
            else
            {
                // File already exists, load it in memory
                try
                {
                    using (var stream = new FileStream(ConfigClass.devPoolCap2Filename, FileMode.Open))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(DevicePoolSerializableClass));
                        // No need to asign result, static reference will be updated in constructor
                        ConfigClass.DevPoolCap2 = (DevicePoolSerializableClass)xml.Deserialize(stream);

                    }
                }
                catch (Exception)
                {
                    ConfigClass.DevPoolCap2 = new DevicePoolSerializableClass(ConfigClass.cap2AddrOffset); // Force default values
                    FormCustomConsole.WriteLine("Problem occurred while initializing availability for Capacitor 2, default values are applied");
                    Console.WriteLine("Problem occurred while initializing availability for Capacitor 2, default values are applied");
                    System.Windows.Forms.MessageBox.Show("Problem occurred while initializing availability for Capacitor 2, default values are applied");
                }

            }
            return;
        }
        

    }

    




}
