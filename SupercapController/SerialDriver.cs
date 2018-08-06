using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupercapController
{
    class SerialDriver
    {
        private static readonly object lockObj = new object();
        static int running = 0;
        public static int Running
        {
            get
            {
                lock (lockObj)
                {
                    return running;
                }
            }
            set
            {
                lock (lockObj)
                {
                    running = value;
                }
            }
        }
        const int MAX_TIMEOUTS = 10;
        //bool useRetry = false;
        //int retryCount = 0;
        //int totalRetry = 4;
        //int sleepBeforeRetry = 1000;

        static Ticker ticker = new Ticker(100);

        static SerialPort serial = new SerialPort();

        static Action<byte[]> _successCallback;
        static Action _failCallback;
        static bool busy = false; // Busy is active when data is expected to arrive from device
        static UARTDataReceiverClass uartReceiver = new UARTDataReceiverClass(); // Collects and decodes bytes sent via custom protocol 
        static byte[] tempSerialReceiveBuff;
        
        static int taskState = 0; // State that controls SM in TickTask



        // Static constructor
        static SerialDriver()
        {
            serial.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived); // Subscribe to fucking handler
            ticker.TickEvent += TickTask;
        }

        private static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            tempSerialReceiveBuff = new byte[serial.BytesToRead];
            Console.WriteLine("Data Received: " + tempSerialReceiveBuff.Length);
            serial.Read(tempSerialReceiveBuff, 0, tempSerialReceiveBuff.Length);
            if (!busy)
            {
                // Just ignore read data that we didnt asked for
                return;
            }

            switch (uartReceiver.CollectData(tempSerialReceiveBuff))
            {
                case UARTResult.Done:
                    taskState = 0;
                    Running = 0;
                    busy = false;
                    // All data collected, process it in a given success delegate if checksum matches
                    ChecksumClass cs = new ChecksumClass();
                    if (cs.VerifyChecksumFromReceivedMessage(uartReceiver.bData))
                    {
                        // Checksum match
                        var dataWithoutChecksum = UARTHelperClass.RemoveChecksumFromMessage(uartReceiver.bData);
                        uartReceiver.Reset();
                        Console.WriteLine("Finished from DataReceived method");
                        _successCallback(dataWithoutChecksum);
                    }
                    else
                    {
                        // Checksum not valid
                        _failCallback();
                    }
                    break;
                case UARTResult.WaitMoreData:
                    Running = 1;
                    break;
                case UARTResult.Error:
                default:
                    uartReceiver.Reset();
                    Running = 0;
                    throw new Exception("Error with UARTResult in SerialDriver.serialPort_DataReceived");
            }
        }

        
        private static void TickTask()
        {
            
            switch (taskState)
            {
                case 0: // IDLE
                    //Console.WriteLine("Tick IDLE");
                    break;
                case 1:
                    Console.WriteLine("Tick RUNNING " + Running.ToString());


                    if (Running == 0)
                    {
                        Console.WriteLine("Tick Finished");
                        taskState = 0;
                        return;
                    }

                    if (busy == false)
                    {
                        // This shouldnt happen
                        throw new Exception("Wrong busy flag in SerialDriver.TickTask");
                    }


                    if (Running++ >= MAX_TIMEOUTS)
                    {
                        // Timeout, restart everything and call fail delegate
                        Console.WriteLine("Tick timeout");
                        uartReceiver.Reset();
                        taskState = 0;
                        _failCallback();
                        busy = false;
                        return;
                    }
                    break;
                default:
                    throw new Exception("Corrupted state in SerialDriver.TickTask");
            }
        }

        public static bool Send(byte[] data)
        {
            if (busy)
            {
                return false;
            }
            serial.Write(data, 0, data.Length);
            return true;
        }

        public static bool Send(byte[] data, Action<byte[]> sucCb, Action fCb)
        {
            if (busy)
            {
                return false;
            }
            busy = true;
            //BytesRead = 0;
            _successCallback = sucCb;
            _failCallback = fCb;
            if (serial.IsOpen)
            {
                serial.Write(data, 0, data.Length);
                taskState = 1; // Put SM where it belongs
                Running = 1;
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Port is not open!");
                return false;
            }
            
            
        }

        public static string Open(string portName)
        {
            try
            {
                if (serial.IsOpen)
                {
                    return "Port is already open";
                }
                serial.PortName = portName;
                serial.Open();
                return  "Ready " + portName;

            }
            catch (Exception)
            {
                return "COM port error";
            }
        }


        public static void Close()
        {
            if (serial.IsOpen)
            {
                serial.Close();
            }
        }

    }
}
