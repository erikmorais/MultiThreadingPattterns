using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadExamples.EventWaitHandleLib
{
    public class MyAction
    {

        public MyAction(Action<string> action, string param)
        {
            Action = action;
            Param = param;
        }

        public Action<string> Action { get; }
        public string Param { get; }
    }
    class ProducerConsumer : IDisposable
    {
        EventWaitHandle _wh = new AutoResetEvent(false);
        Thread _worker;
        readonly object _locker = new object();
        Queue<MyAction> _tasks = new Queue<MyAction>();
        public ProducerConsumer Initialise()
        {
            _worker = new Thread(Work);
            _worker.Start();
            return this;
        }

        public void EnqueTask(MyAction action)
        {
            lock (_locker)
            {
                _tasks.Enqueue(action);
            }
            _wh.Set();
        }

        void Work()
        {
            while (true)
            {
                MyAction task = null;
                lock (_locker)
                {
                    if (_tasks.Count > 0)
                    {
                        task = _tasks.Dequeue();
                        if (task == null)
                            return;
                    }
                }

                if (task != null)
                {
                    task.Action.Invoke(task.Param);
                }
                else
                {
                    _wh.WaitOne();
                }

            }
        }

        private void CallbackMethod(IAsyncResult ar)
        {
            Action example = (ar as IAsyncResult).AsyncState as Action;
            try
            {
                example.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            _wh.Close();
        }
    }
}
