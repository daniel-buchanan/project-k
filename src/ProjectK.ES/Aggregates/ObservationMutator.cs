using System;
using Newtonsoft.Json;
using ProjectK.Core.Aggregates;
using ProjectK.Core.Events;
using ProjectK.Model;

namespace ProjectK.ES.Aggregates
{
    public class ObservationMutator : IAggregateMutator<Observation>
    {
        public void Mutate(IEvent @event)
        {
            var observation = JsonConvert.DeserializeObject<Observation>(@event.Data);
            DataStore.DataStore.Observations.Update(observation);
        }

        public Observation Mutate(Observation currentState, IEvent @event)
        {
            var incoming = JsonConvert.DeserializeObject<Observation>(@event.Data);

            var changed = false;

            if(currentState == null) currentState = new Observation();

            if (currentState.Id == Guid.Empty)
                currentState.Id = @event.Subject;

            changed |= currentState.SetProperty(p => p.Data, incoming);
            changed |= currentState.SetProperty(p => p.Kind, incoming);
            changed |= currentState.SetProperty(p => p.Observed, incoming);
            changed |= currentState.SetProperty(p => p.Occurred, incoming);
            changed |= currentState.SetProperty(p => p.Recorded, incoming);

            if (changed) currentState.Version = @event.Sequence;

            return currentState;
        }
    }
}
