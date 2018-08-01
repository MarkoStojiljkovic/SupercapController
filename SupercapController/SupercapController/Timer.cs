using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SupercapController
{
    /// <summary>
    /// Timer for one-time callback mechanism with configurable delay. 
    /// Callback execution can be aborted.
    /// Runs on separate thread.
    /// </summary>
    class Timer
    {
        int waitInterval;
        Action action;
        Thread t;

        public Timer(int waitInt, Action act)
        {
            waitInterval = waitInt;
            action = act;
            t = new Thread(TickTask);
            t.Name = "TimerThread";
            t.Start();
        }

        private void TickTask()
        {
            try
            {
                Thread.Sleep(waitInterval); // waitInterval ms
                action();
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Thread '{0}' aborted.", Thread.CurrentThread.Name);
            }
            // This wont execute if thread is aborted
            Console.WriteLine("Thread '{0}' finished.", Thread.CurrentThread.Name);
        }

        public void CancelTimer()
        {
            t.Abort();
        }
    }
}
