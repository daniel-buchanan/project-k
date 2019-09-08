using System;
using System.Collections.Generic;
using ProjectK.Core.Aggregates;

namespace ProjectK.Core.Events
{
    public interface IEventStream : IImmutableList<IEvent>
    {
        void Add(Guid subject, string aggregate, string kind, string data, DateTimeOffset timestamp);

        void RegisterListener(IEventProcessor processor);

        IEnumerable<IEvent> Fetch<T>(Guid subject) where T : IAggregate;
        IEnumerable<IEvent> Fetch<T>(Guid subject, DateTime timestampTo) where T : IAggregate;
        IEnumerable<IEvent> Fetch<T>(Guid subject, int sequenceTo) where T : IAggregate;
    }
}
