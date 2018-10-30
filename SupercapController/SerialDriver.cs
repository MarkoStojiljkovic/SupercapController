using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugTools;
using System.Threading;

namespace SupercapController
{
    class SerialDriver
    {
        private static readonly object lockObj = new object();
        private static readonly object lockObj2 = new object();
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
        static bool busy = false; // Busy is active when data is expected to arrive from device
        private static bool Busy
        {
            get
            {
                lock (lockObj2)
                {
                    return busy;
                }
            }
            set
            {
                lock (lockObj2)
                {
                    busy = value;
                }
            }
        }

        const int MAX_TIMEOUTS = 10;

        static bool _useRetry = false;
        static int _retryCount = 0;
        const int _TOTAL_RETRY = 3;
        static int _sleepBeforeRetry = 0;
        const int _TOTAL_SLEEP_CNT = 15; // 15 * 100 ms

        static Ticker ticker = new Ticker(100);

        static SerialPort serial = new SerialPort();

        static Action<byte[]> _successCallback;
        static Action _failCallback;
        
        static UARTDataReceiverClass uartReceiver = new UARTDataReceiverClass(); // Collects and decodes bytes sent via custom protocol 
        static byte[] tempSerialReceiveBuff;
        
        static int taskState = 0; // State that controls SM in TickTask

        static byte[] dataPointer;


        // Static constructor
        static SerialDriver()
        {
            serial.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived); // Subscribe to fucking handler
            ticker.TickEvent += TickTask;
        }

        private static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            tempSerialReceiveBuff = new byte[serial.BytesToRead];
            
            serial.Read(tempSerialReceiveBuff, 0, tempSerialReceiveBuff.Length);
            if (!Busy)
            {
                // Just ignore read data that we didnt asked for
                return;
            }

            switch (uartReceiver.CollectData(tempSerialReceiveBuff))
            {
                case UARTResult.Done:
                    taskState = 0;
                    Running = 0;
                    Busy = false;
                    // All data collected, process it in a given success delegate if checksum matches
                    ChecksumClass cs = new ChecksumClass();
                    if (cs.VerifyChecksumFromReceivedMessage(uartReceiver.bData))
                    {
                        // Checksum match
                        var dataWithoutChecksum = UARTHelperClass.RemoveChecksumFromMessage(uartReceiver.bData);
                        uartReceiver.Reset();
                        FormCustomConsole.WriteLineWithConsole("Finished from DataReceived method");
                        _successCallback(dataWithoutChecksum);
                    }
                    else
                    {
                        if (_useRetry)
                        {
                            if (_retryCount >= _TOTAL_RETRY)
                            {
                                FormCustomConsole.WriteWithConsole("Aborting process (checksum doesnt match) at ");
                                FormCustomConsole.WriteLineWithConsole(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                                _failCallback();
                                Busy = false;
                            }
                            else
                            {
                                // Leave busy flag we are not finished yet
                                _retryCount++;
                                _sleepBeforeRetry = 1;
                                taskState = 2;
                            }
                        }
                        else
                        {
                            // Checksum not valid
                            _failCallback();
                            Busy = false;
                        }
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
                    //Console.WriteLine("Tick RUNNING " + Running.ToString());
                    if (Running == 0)
                    {
                        Console.WriteLine("Tick Finished");
                        taskState = 0;
                        return;
                    }

                    if (Busy == false)
                    {
                        // This shouldnt happen
                        throw new Exception("Wrong busy flag in SerialDriver.TickTask");
                    }


                    if (Running++ >= MAX_TIMEOUTS)
                    {
                        // Timeout, retry if possible or restart everything and call fail delegate
                        FormCustomConsole.WriteWithConsole("Tick timeout at ");
                        FormCustomConsole.WriteLineWithConsole(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                        uartReceiver.Reset();
                        taskState = 0;
                        if (_useRetry)
                        {
                            if (_retryCount >= _TOTAL_RETRY)
                            {
                                FormCustomConsole.WriteWithConsole("Aborting process at ");
                                FormCustomConsole.WriteLineWithConsole(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                                _failCallback();
                                Busy = false;
                            }
                            else
                            {
                                // Leave busy flag we are not finished yet
                                _retryCount++;
                                _sleepBeforeRetry = 1;
                                taskState = 2;
                            }
                        }
                        else
                        {
                            _failCallback();
                            Busy = false;
                        }
                        return;
                    }
                    break;
                case 2: // State for retry delay\
                    //Console.WriteLine("Retry delay state " + _sleepBeforeRetry);
                    if (_sleepBeforeRetry++ >= _TOTAL_SLEEP_CNT)
                    {
                        if (serial.IsOpen)
                        {
                            FormCustomConsole.WriteWithConsole("Sending again at ");
                            FormCustomConsole.WriteLineWithConsole(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                            serial.Write(dataPointer, 0, dataPointer.Length);
                            taskState = 1; // Put SM where it belongs
                            Running = 1;
                        }
                        else
                        {
                            // Abort everything
                            System.Windows.Forms.MessageBox.Show("Port is not open!");
                            _failCallback();
                            Busy = false;
                            taskState = 0;
                        }
                    }
                    break;
                default:
                    throw new Exception("Corrupted state in SerialDriver.TickTask");
            }
        }

        public static bool Send(byte[] data)
        {
            if (Busy)
            {
                return false;
            }
            dataPointer = data;
            serial.Write(data, 0, data.Length);
            return true;
        }

        public static bool Send(byte[] data, Action<byte[]> sucCb, Action fCb, bool useRet = true)
        {
            if (Busy)
            {
                return false;
            }
            Busy = true;
            _useRetry = useRet;
            if (useRet)
            {
                // Start from 1 so we dont need to take -1 into consideration
                _retryCount = 1; 
                _sleepBeforeRetry = 1;
            }
            
            _successCallback = sucCb;
            _failCallback = fCb;
            if (serial.IsOpen)
            {
                dataPointer = data;
                FormCustomConsole.WriteLineWithConsole(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                taskState = 1; // Put SM where it belongs
                Running = 1;
                serial.Write(data, 0, data.Length);
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Port is not open!");
                Busy = false;
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
