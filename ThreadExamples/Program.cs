using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThreadExamples.EventWaitHandleLib;
using ThreadExamples.ProducerConsumer.Advanced.Implementation;
using ThreadExamples.ProducerConsumerLib;
using static System.Console;

namespace ThreadExamples.EventWaitHandleLib
{
    class Program
    {


        static void Main(string[] args)
        {


            Console.ReadKey();
            Example_1(); 
            // Example_2();
            // Example_3();
            Example_4();
            Console.ReadKey();
        }

        public static void Example_4()
        {
            ProducerConsumerWorkOfString pc = new ProducerConsumerWorkOfString(4);
            for (int i = 0; i < 100; i++)
            {
                string tmp = i.ToString();
                WorktringParameter wrk = new WorktringParameter(tmp, PrintConsole);
                pc.Enqueue(wrk);
            }
            Thread.Sleep(1000);

            for (int i = 0; i < 100; i++)
            {
                string tmp = "oi " + i.ToString();
                WorktringParameter wrk = new WorktringParameter(tmp, PrintConsole);
                pc.Enqueue(wrk);
            }
        }
        public static void Example_1()
        {
            TwoWaySignaling t = new TwoWaySignaling();
            t.Run();

            /***  example 2  ****/
            TwoWaySignalingOne t2 = new TwoWaySignalingOne();
            t2.Run();
            Console.ReadKey();
        }
        public static void Example_2()
        {
            ProducerConsumer pc = new ProducerConsumer();
            pc.Initialise();

            for (int i = 0; i < 100; i++)
            {
                int tmp = i;
                MyAction action = new MyAction(PrintConsole, i.ToString());
                pc.EnqueTask(action);
            }
            Thread.Sleep(1000);

            for (int i = 0; i < 100; i++)
            {
                int tmp = i;
                MyAction action = new MyAction(PrintConsole, "oi " + i.ToString());
                pc.EnqueTask(action);
            }
        }
        public static void Example_3()
        {
            ProducerConsumerQueue q = new ProducerConsumerQueue(10);

            Console.WriteLine("Enqueuing 10 items...");

            for (int i = 0; i < 10; i++)
            {
                int itemNumber = i;      // To avoid the captured variable trap
                q.EnqueueItem(() =>
                {
                    Thread.Sleep(1000);          // Simulate time-consuming work
                    Console.Write(" Task" + itemNumber);
                });
            }

            q.Shutdown(true);
            Console.WriteLine();
            Console.WriteLine("Workers complete!");
        }


        public static void PrintConsole(string msg)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + " >> " + msg);
        }
    }
}
