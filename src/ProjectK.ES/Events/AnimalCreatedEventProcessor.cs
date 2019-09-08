using ProjectK.Core.Aggregates;
using ProjectK.Core.Events;
using ProjectK.Model;

namespace ProjectK.ES.Events
{
    public class AnimalCreatedEventProcessor : EventProcessor
    {
        private readonly IAggregateMutator<Animal> _animalMutator;
        private readonly IAggregateStore<Animal> _animalStore;

        public AnimalCreatedEventProcessor(IAggregateMutator<Animal> animalMutator,
            IAggregateStore<Animal> animalStore)
        {
            _animalMutator = animalMutator;
            _animalStore = animalStore;
        }

        public override string Kind => EventKinds.AnimalCreated;
        public override void Process(IEvent @event)
        {
            var current = _animalStore.Get(@event.Subject);
            var mutated = _animalMutator.Mutate(current, @event);
            _animalStore.Persist(mutated);
        }
    }
}
