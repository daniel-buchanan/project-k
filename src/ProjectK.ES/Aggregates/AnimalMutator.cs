using System;
using Newtonsoft.Json;
using ProjectK.Core.Aggregates;
using ProjectK.Core.Events;
using ProjectK.Model;

namespace ProjectK.ES.Aggregates
{
    public class AnimalMutator : IAggregateMutator<Animal>
    {
        public Animal Mutate(Animal currentState, IEvent @event)
        {
            var incoming = JsonConvert.DeserializeObject<Animal>(@event.Data);

            var changed = false;

            if(currentState == null) currentState = new Animal();

            if (currentState.Id == Guid.Empty)
                currentState.Id = @event.Subject;

            changed |= currentState.SetProperty(p => p.MobId, incoming);
            changed |= currentState.SetProperty(p => p.BirthDate, incoming);
            changed |= currentState.SetProperty(p => p.Eid, incoming);
            changed |= currentState.SetProperty(p => p.OfficialId, incoming);
            changed |= currentState.SetProperty(p => p.Sex, incoming);

            if (changed) currentState.Version = @event.Sequence;

            return currentState;
        }
    }
}
