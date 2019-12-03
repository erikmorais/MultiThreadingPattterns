using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ThreadExamples.ProducerConsumer.Advanced.Interfaces;

namespace ThreadExamples.ProducerConsumer.Advanced.Implementation
{
    public class ProducerConsumerWorkOfString : IProducerConsumer<IWork<string>, string>
    {
        private readonly Thread[] threads;
        private readonly object _locker = new object();
        public readonly Queue<IWork<string>> Queue = new Queue<IWork<string>>();

        public ProducerConsumerWorkOfString(int numberOfWorkers)
        {
            NumberOfWorkers = numberOfWorkers;
            threads = new Thread[numberOfWorkers];
            for (int i = 0; i < numberOfWorkers; i++)
            {
                threads[i] = new Thread(Consumer);
                threads[i].Start();
            }

        }
        private void Consumer()
        {
            while (true)
            {
                IWork<string> work = null;
                lock(_locker)
                {
                    while (Queue.Count == 0) Monitor.Wait(_locker);
                    work = Queue.Dequeue();

                    if (work == null) return;
                    work.Run();
                }
            }
        }
        public int NumberOfWorkers { get; }

        public IProducerConsumer<IWork<string>,string> Enqueue(IWork<string> Item)
        {
            lock (_locker)
            {
                this.Queue.Enqueue(Item);
                Monitor.Pulse(_locker);
            }
           
            return this;
        }

        public IProducerConsumer<IWork<string>, string> ShutDown(bool wait)
        {
            foreach( var thread in threads)
            {
                Enqueue(null);
            }
            
            if (wait)
            {
                foreach( var thread in threads)
                {
                    thread.Join();
                }
            }
            return this;
        }
    }
}
