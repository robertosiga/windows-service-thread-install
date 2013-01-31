using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace ExampleService
{
    public static class Process
    {
        static Process() { }

        //Start the thread and call the service
        public static void Start()
        {
            Trace.WriteLine("Processo Start()");
            Thread thread = new Thread(new ThreadStart(doWork));
            thread.Start();
            Thread thread2 = new Thread(new ThreadStart(doWorks));
            thread2.Start();
        }

        //stop the thread and stop the service
        public static void Stop()
        {
        }

        //action called by the thread and debugmode
        public static void doWork()
        {
            for (int x = 0; x <= 10000; x++)
            {
                Trace.WriteLine("x = " + x);
            }
        }

        //action called by the thread and debugmode
        public static void doWorks()
        {
            for (int x = 10000; x > 1; x--)
            {
                Trace.WriteLine("Y = " + x);
            }
        }
    }
}
