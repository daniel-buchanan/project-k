using System;
using Newtonsoft.Json;
using ProjectK.Core.Commands;
using ProjectK.Core.Events;
using ProjectK.ES.Events;
using ProjectK.Model;
using ProjectK.Model.Events;
using ProjectK.Model.Observations;

namespace ProjectK.ES.Commands.Processors
{
    public class AnimalArrivedCommandProcessor : AbstractCommandProcessor<AnimalArrivedCommand>
    {
        public AnimalArrivedCommandProcessor(IEventStream eventStream,
            ICommandValidator<AnimalArrivedCommand> validator) : base(eventStream, validator)
        {
        }

        public override void Process(AnimalArrivedCommand command)
        {
            foreach (var a in command.Observation.Animals)
            {
                var data = JsonConvert.SerializeObject(a);
                EventStream.Add(a.Id, nameof(Animal), EventKinds.AnimalCreated, data, DateTimeOffset.Now);

                var arrived = new AnimalArrived()
                {
                    From = command.Observation.From,
                    AnimalId = a.Id
                };

                var arrivedData = JsonConvert.SerializeObject(arrived);
                EventStream.Add(Guid.NewGuid(), nameof(Observation), EventKinds.AnimalArrived, arrivedData, DateTimeOffset.Now);

                var intoMob = new AnimalIntoMobObservation()
                {
                    AnimalId = a.Id,
                    MobId = command.Observation.MobId
                };

                var intoMobData = JsonConvert.SerializeObject(intoMob);
                EventStream.Add(Guid.NewGuid(), nameof(Observation), EventKinds.AnimalIntoMob, intoMobData, DateTimeOffset.Now);
            }
        }
    }
}
