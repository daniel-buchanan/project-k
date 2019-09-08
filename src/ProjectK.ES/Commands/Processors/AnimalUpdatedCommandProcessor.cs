using System;
using Newtonsoft.Json;
using ProjectK.Core.Commands;
using ProjectK.Core.Events;
using ProjectK.ES.Events;
using ProjectK.Model;

namespace ProjectK.ES.Commands.Processors
{
    class AnimalUpdatedCommandProcessor : AbstractCommandProcessor<AnimalUpdatedCommand>
    {
        public AnimalUpdatedCommandProcessor(IEventStream eventStream,
            ICommandValidator<AnimalUpdatedCommand> validator) : base(eventStream, validator)
        {
        }

        public override void Process(AnimalUpdatedCommand command)
        {
            var data = JsonConvert.SerializeObject(command.Animal);
            EventStream.Add(command.Animal.Id, nameof(Animal), EventKinds.AnimalUpdated, data, DateTimeOffset.Now);
        }
    }
}
