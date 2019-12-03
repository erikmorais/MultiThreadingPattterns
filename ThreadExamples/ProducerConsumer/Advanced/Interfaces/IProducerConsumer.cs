using System.Collections.Generic;
using System.Text;

namespace ThreadExamples.ProducerConsumer.Advanced.Interfaces
{
    public interface IProducerConsumer<T,P> where T: IWork<P>
    {
        IProducerConsumer<T,P> Enqueue(T Item);
        IProducerConsumer<T,P> ShutDown(bool wait);
    }
}
