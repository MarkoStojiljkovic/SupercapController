using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SupercapController
{
    class Ticker
    {

        private bool running = true;
        private int waitInterval;

        public delegate void TickEventHandler();

        public event TickEventHandler TickEvent;

        /// <summary>
        ///  Create ticker object and subscribe to its event to be called periodically
        /// </summary>
        /// <param name="waitInt">Define loop period in miliseconds </param>
        public Ticker(int waitInt)
        {
            waitInterval = waitInt;
            Thread t = new Thread(TickTask);
            t.Start();
        }
        
        protected void OnTick()
        {
            // Prevent 1 more tick when disabled
            if (running)
            {
                if (TickEvent != null)
                {
                    TickEvent();
                }
            }
        }

        private void TickTask()
        {
            while (running)
            {
                Thread.Sleep(waitInterval);
                OnTick();
            }
        }
        
        public void StopTicker()
        {
            running = false;
        }
        
    }
}
