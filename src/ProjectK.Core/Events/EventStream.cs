using System;
using System.Collections.Generic;
using System.Linq;
using ProjectK.Core.Aggregates;

namespace ProjectK.Core.Events
{
    public class EventStream : ImmutableList<IEvent>, IEventStream
    {
        private readonly IDictionary<string, IEventProcessor> _listeners =
            new Dictionary<string, IEventProcessor>();

        private int _nextSequence;

        public void Add(Guid subject, string aggregate, string kind, string data, DateTimeOffset timestamp)
        {
            var nextSequence = _nextSequence + 1;
            var @event = new Event(nextSequence, subject, aggregate, kind, data, timestamp);
            base.Add(@event);
            var listenerExists = _listeners.ContainsKey(kind);
            if (!listenerExists) return;
            _listeners[kind].Process(@event);
        }

        public void RegisterListener(IEventProcessor processor)
        {
            if (_listeners.ContainsKey(processor.Kind)) return;
            _listeners.Add(processor.Kind, processor);
        }

        public IEnumerable<IEvent> Fetch<T>(Guid subject) where T : IAggregate
        {
            return this.Where(e => e.Subject == subject).OrderBy(e => e.Sequence);
        }

        public IEnumerable<IEvent> Fetch<T>(Guid subject, DateTime timestampTo) where T : IAggregate
        {
            return Fetch<T>(subject).Where(e => e.Timestamp <= timestampTo);
        }

        public IEnumerable<IEvent> Fetch<T>(Guid subject, int sequenceTo) where T : IAggregate
        {
            return Fetch<T>(subject).Where(e => e.Sequence <= sequenceTo);
        }
    }
}
