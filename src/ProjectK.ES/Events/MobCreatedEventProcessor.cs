using ProjectK.Core.Aggregates;
using ProjectK.Core.Events;
using ProjectK.Model;

namespace ProjectK.ES.Events
{
    public class MobCreatedEventProcessor : EventProcessor
    {
        private readonly IAggregateMutator<Mob> _mobMutator;
        private readonly IAggregateStore<Mob> _mobStore;

        public MobCreatedEventProcessor(IAggregateMutator<Mob> mobMutator,
            IAggregateStore<Mob> mobStore)
        {
            _mobMutator = mobMutator;
            _mobStore = mobStore;
        }

        public override string Kind => EventKinds.AnimalCreated;
        public override void Process(IEvent @event)
        {
            var current = _mobStore.Get(@event.Subject);
            var mutated = _mobMutator.Mutate(current, @event);
            _mobStore.Persist(mutated);
        }
    }
}
