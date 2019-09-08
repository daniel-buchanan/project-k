using ProjectK.Core.Aggregates;
using ProjectK.Core.Events;
using ProjectK.Model;

namespace ProjectK.ES.Events
{
    public class AnimalUpdatedEventProcessor : EventProcessor
    {
        private readonly IAggregateMutator<Animal> _animalMutator;
        private readonly IAggregateStore<Animal> _animalStore;

        public AnimalUpdatedEventProcessor(IAggregateMutator<Animal> animalMutator,
            IAggregateStore<Animal> animalStore)
        {
            _animalMutator = animalMutator;
            _animalStore = animalStore;
        }

        public override string Kind => EventKinds.AnimalUpdated;
        public override void Process(IEvent @event)
        {
            var current = _animalStore.Get(@event.Subject);
            var mutated = _animalMutator.Mutate(current, @event);
            _animalStore.Persist(mutated);
        }
    }
}
