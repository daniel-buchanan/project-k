using System;
using Newtonsoft.Json;
using ProjectK.Core.Commands;
using ProjectK.Core.Events;
using ProjectK.ES.Events;
using ProjectK.Model;

namespace ProjectK.ES.Commands.Processors
{
    class MobCreatedCommandProcessor : AbstractCommandProcessor<MobCreatedCommand>
    {
        public MobCreatedCommandProcessor(IEventStream eventStream,
            ICommandValidator<MobCreatedCommand> validator) : base(eventStream, validator)
        {
        }

        public override void Process(MobCreatedCommand command)
        {
            var data = JsonConvert.SerializeObject(command.Mob);
            EventStream.Add(Guid.NewGuid(), nameof(Mob), EventKinds.MobCreated, data, DateTimeOffset.Now);
        }
    }
}
