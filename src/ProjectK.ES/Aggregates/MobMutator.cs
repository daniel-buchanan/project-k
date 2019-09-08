using System;
using Newtonsoft.Json;
using ProjectK.Core.Aggregates;
using ProjectK.Core.Events;
using ProjectK.Model;

namespace ProjectK.ES.Aggregates
{
    public class MobMutator : IAggregateMutator<Mob>
    {
        public Mob Mutate(Mob currentState, IEvent @event)
        {
            var incoming = JsonConvert.DeserializeObject<Mob>(@event.Data);

            var changed = false;

            if(currentState == null) currentState = new Mob();

            if (currentState.Id == Guid.Empty)
                currentState.Id = @event.Subject;

            changed |= currentState.SetProperty(p => p.Name, incoming);
            changed |= currentState.SetProperty(p => p.HeadCount, incoming);

            if (changed) currentState.Version = @event.Sequence;

            return currentState;
        }
    }
}
