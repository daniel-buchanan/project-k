using System;
using System.Collections.Generic;
using ProjectK.Core.Aggregates;
using ProjectK.Core.Events;

namespace ProjectK.ES.Aggregates
{
    public class AggregatePlayer<T> : IAggregatePlayer<T> where T: IAggregate, new()
    {
        private readonly IAggregateMutator<T> _mutator;
        private readonly IEventStream _stream;

        public AggregatePlayer(IAggregateMutator<T> mutator,
            IEventStream stream)
        {
            _mutator = mutator;
            _stream = stream;
        }

        public T PlayTo(Guid subjectId, DateTime timestamp)
        {
            var events = _stream.Fetch<T>(subjectId, timestamp);
            return Play(events);
        }

        public T PlayTo(Guid subjectId, int sequence)
        {
            var events = _stream.Fetch<T>(subjectId, sequence);
            return Play(events);
        }

        public T PlayToNow(Guid subjectId)
        {

            var events = _stream.Fetch<T>(subjectId);
            return Play(events);
        }

        private T Play(IEnumerable<IEvent> events)
        {
            var aggregate = new T();
            foreach (var e in events)
                aggregate = _mutator.Mutate(aggregate, e);

            return aggregate;
        }
    }
}
