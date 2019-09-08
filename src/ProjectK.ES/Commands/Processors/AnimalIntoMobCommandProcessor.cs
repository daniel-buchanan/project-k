using System;
using Newtonsoft.Json;
using ProjectK.Core.Commands;
using ProjectK.Core.Events;
using ProjectK.ES.Events;
using ProjectK.Model;

namespace ProjectK.ES.Commands.Processors
{
    class AnimalIntoMobCommandProcessor : AbstractCommandProcessor<AnimalIntoMobCommand>
    {
        public AnimalIntoMobCommandProcessor(IEventStream eventStream, 
            ICommandValidator<AnimalIntoMobCommand> validator) : base(eventStream, validator)
        {
        }

        public override void Process(AnimalIntoMobCommand command)
        {
            var data = JsonConvert.SerializeObject(command.Observation);
            EventStream.Add(Guid.NewGuid(), nameof(Observation), EventKinds.AnimalIntoMob, data, DateTimeOffset.Now);
        }
    }
}
