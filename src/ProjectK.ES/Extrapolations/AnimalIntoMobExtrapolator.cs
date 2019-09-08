using System.Linq;
using Newtonsoft.Json;
using ProjectK.Core.Events;
using ProjectK.Core.Extrapolations;
using ProjectK.DataStore;
using ProjectK.ES.Events;
using ProjectK.Model;
using ProjectK.Model.Observations;

namespace ProjectK.ES.Extrapolations
{
    public class AnimalIntoMobExtrapolator : IExtrapolator<AnimalMobHistory>
    {
        private readonly IEventStream _eventStream;
        private readonly IDataRepository<AnimalMobHistory> _store;

        public AnimalIntoMobExtrapolator(IEventStream eventStream, IDataRepository<AnimalMobHistory> store)
        {
            _eventStream = eventStream;
            _store = store;
        }

        public void Extrapolate(IEvent @event)
        {
            var events = _eventStream.Where(e => e.Kind == EventKinds.AnimalIntoMob).ToList();
            var subjects = events.Select(e => e.Subject).Distinct();

            _store.Clear();

            foreach (var sub in subjects)
            {
                var subjectEvents = events.Where(e => e.Subject == sub)
                    .Select(e => JsonConvert.DeserializeObject<AnimalIntoMobObservation>(e.Data))
                    .OrderBy(o => o.Observed);

                AnimalMobHistory previous = null;
                foreach (var subEvent in subjectEvents)
                {
                    if (previous == null)
                    {
                        previous = new AnimalMobHistory()
                        {
                            AnimalId = subEvent.AnimalId,
                            MobId = subEvent.MobId,
                            DateIn = subEvent.Observed
                        };
                        _store.Add(previous);
                        continue;
                    }

                    var current = new AnimalMobHistory()
                    {
                        AnimalId = subEvent.AnimalId,
                        MobId = subEvent.MobId,
                        DateIn = subEvent.Observed
                    };

                    previous.DateOut = subEvent.Observed;

                    _store.Add(current);
                }

                var latest = DataStore.DataStore.AnimalMobHistories.Where(h => h.AnimalId == sub)
                    .OrderByDescending(h => h.DateIn).FirstOrDefault();

                if (latest == null) continue;

                var animal = DataStore.DataStore.Animals.First(a => a.Id == latest.AnimalId);
                animal.MobId = latest.MobId;
                DataStore.DataStore.Animals.Update(animal);
            }
        }
    }
}
