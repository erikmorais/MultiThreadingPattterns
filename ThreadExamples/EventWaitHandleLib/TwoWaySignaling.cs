using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadExamples.EventWaitHandleLib
{
    class TwoWaySignaling
    {
        EventWaitHandle _ready = new AutoResetEvent(false);
        EventWaitHandle _go = new AutoResetEvent(false);
        string _msg = "";

        public void Run()
        {
            var thread = new Thread(DoWork);
            thread.Start();
            Thread.Sleep(10000);
            _ready.WaitOne();
            _msg = "hello!";
            _go.Set();

            _ready.WaitOne();
            _msg = " world!";
            _go.Set();

            _ready.WaitOne();
            _msg = " last message";
            _go.Set();

            Console.ReadKey();


        }

        private void DoWork()
        {
            while (true)
            {
                _ready.Set();
                _go.WaitOne();
                if (_msg == null) return;

                Console.WriteLine(_msg);

            }
        }
    }
}
