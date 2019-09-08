using System;

namespace ProjectK.Core.Events
{
    public abstract class EventProcessor : IEventProcessor
    {
        public abstract string Kind { get; }

        public abstract void Process(IEvent @event);
    }
}
