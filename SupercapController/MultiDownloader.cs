using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SupercapController
{
    class MultiDownloader
    {
        public static bool busy = false; // When downloading is in progress set this flag

        private static int currentIndex = 0;
        private static int targetIndex = 0;
        static List<Tuple<int, int>> _adrList;

        static Action<List<byte[]>> _sucCb;
        static Action _fCb;
       


        static List<byte[]> finalDataList;

        public static bool Download(List<Tuple<int, int>> adrList, Action<List<byte[]>> sucCb, Action fCb)
        {
            if (busy)
            {
                return false; // Downloader is busy, abort request
            }
            busy = true;
            finalDataList = new List<byte[]>();
            _adrList = adrList;
            currentIndex = 0;
            targetIndex = adrList.Count;
            _sucCb = sucCb;
            _fCb = fCb;
            // Bootstrap
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendReadFromAddress(_adrList[currentIndex].Item1, _adrList[currentIndex].Item2);
            var data = cm.GetFinalCommandList();
            if (SerialDriver.Send(data, Fetcher, FailCallback))
            {
                currentIndex++;
                return true;
            }
            else
            {
                // Serial driver is busy
                return false;
            }
        }

        private static void FailCallback()
        {
            // Something gone wrong
            _fCb(); // Call failcallback
            busy = false;
            return;
        }

        private static void Fetcher(byte[] b)
        {
            finalDataList.Add(b); // First add received data to list (we had bootstrap)
            if (currentIndex >= targetIndex)
            {
                // All went well, call successful callback with results
                _sucCb(finalDataList);
                busy = false;
                return;
            }
            Thread.Sleep(10); // Device cant manage requests so fast
            // Send command to get measurement header + data
            CommandFormerClass cm = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            cm.AppendReadFromAddress(_adrList[currentIndex].Item1, _adrList[currentIndex].Item2);
            var data = cm.GetFinalCommandList();
            SerialDriver.Send(data, Fetcher, FailCallback);
            currentIndex++;
        }



    }
}
