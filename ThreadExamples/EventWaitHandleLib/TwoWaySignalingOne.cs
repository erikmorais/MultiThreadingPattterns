using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadExamples.EventWaitHandleLib
{
    public class TwoWaySignalingOne
    {
        EventWaitHandle _evenReady = new AutoResetEvent(false);
        EventWaitHandle _oddReady = new AutoResetEvent(false);

        object _lock = new object();

        IList<int> _list = new List<int>();
        public void Run()
        {

            var odd = new Thread(() => PrintOddNumber(_list, 100000));

            var even = new Thread(() => PrintEvenNumber(_list, 100000));

            odd.Start();
            even.Start();
            Thread.Sleep(3000);
            _evenReady.Set();
            odd.Join();
            even.Join();


            for (int i = 0; i < _list.Count - 1; i++)
            {
                if ((_list[i + 1] - _list[i] != 1))
                    Console.WriteLine(" failed at " + i.ToString());
            }

            Console.WriteLine("done " + _list.Count.ToString());

            Console.ReadKey();

        }

        public void PrintEvenNumber(IList<int> list, int max)
        {
            for (int i = 0; i <= max; i++)
            {
                _evenReady.WaitOne();
                lock (_lock)
                {
                    if (i % 2 == 0)
                    {
                        list.Add(i);
                    }
                }
                _oddReady.Set();

            }
        }
        public void PrintOddNumber(IList<int> list, int max)
        {
            for (int i = 1; i <= max; i++)
            {
                _oddReady.WaitOne();
                lock (_lock)
                {
                    if (i % 2 != 0)
                    {
                        list.Add(i);
                    }
                }
                _evenReady.Set();

            }
        }
    }
}
