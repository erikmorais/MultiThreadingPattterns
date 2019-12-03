using System;
using System.Collections.Generic;
using System.Text;
using ThreadExamples.ProducerConsumer.Advanced.Interfaces;

namespace ThreadExamples.ProducerConsumer.Advanced.Implementation
{
    public class WorktringParameter : IWork<String>
    {
        public WorktringParameter(string paramenter, Action<String> action)
        {
            Paramenter = paramenter;
            Action = action;
        }
        public Action<string> Action { get; }

        public string Paramenter { get; }

        public void Run()
        {
            this.Action(this.Paramenter);
        }
    }
}
