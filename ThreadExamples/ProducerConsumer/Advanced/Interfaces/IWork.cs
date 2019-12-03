using System;

namespace ThreadExamples.ProducerConsumer.Advanced.Interfaces
{
    public interface IWork<P>
    {
        Action<P> Action { get; }
        P Paramenter { get; }
        void Run();
    }
}
