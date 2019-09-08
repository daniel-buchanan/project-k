using ProjectK.Core.Events;
using ProjectK.Core.Extrapolations;
using ProjectK.Model;

namespace ProjectK.ES.Events
{
    public class AnimalIntoMobEventProcessor : EventProcessor
    {
        private readonly IExtrapolator<AnimalMobHistory> _animalMobHistoryExtrapolator;

        public AnimalIntoMobEventProcessor(IExtrapolator<AnimalMobHistory> animalMobHistoryExtrapolator)
        {
            _animalMobHistoryExtrapolator = animalMobHistoryExtrapolator;
        }

        public override string Kind => EventKinds.AnimalIntoMob;

        public override void Process(IEvent @event)
        {
            //_animalMobHistoryExtrapolator.Extrapolate(@event);
        }
    }
}
