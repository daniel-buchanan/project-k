using ProjectK.Core.Aggregates;
using ProjectK.Core.Events;
using ProjectK.Model;

namespace ProjectK.ES.Events
{
    public class AnimalArrivedEventProcessor : EventProcessor
    {
        private readonly IAggregateMutator<Observation> _observationMutator;
        private readonly IAggregateStore<Observation> _observationStore;

        public AnimalArrivedEventProcessor(IAggregateMutator<Observation> observationMutator,
            IAggregateStore<Observation> observationStore)
        {
            _observationMutator = observationMutator;
            _observationStore = observationStore;
        }

        public override string Kind => EventKinds.AnimalArrived;
        public override void Process(IEvent @event)
        {
            var current = _observationStore.Get(@event.Subject);
            var mutated = _observationMutator.Mutate(current, @event);
            _observationStore.Persist(mutated);
        }
    }
}
