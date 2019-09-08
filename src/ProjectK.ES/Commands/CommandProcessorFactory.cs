using ProjectK.Core.Commands;
using ProjectK.Core.Events;
using ProjectK.ES.Commands.Processors;

namespace ProjectK.ES.Commands
{
    public static class CommandProcessorFactory
    {
        public static ICommandProcessor GetForCommand<T>(IEventStream stream, T command) where T : ICommand
        {
            var name = command.GetType().Name;
            if (name == nameof(AnimalArrivedCommand)) return new AnimalArrivedCommandProcessor(stream, CommandValidatorFactory.Get<AnimalArrivedCommand>());
            if (name == nameof(AnimalIntoMobCommand)) return new AnimalIntoMobCommandProcessor(stream, CommandValidatorFactory.Get<AnimalIntoMobCommand>());
            if (name == nameof(MobCreatedCommand)) return new MobCreatedCommandProcessor(stream, CommandValidatorFactory.Get<MobCreatedCommand>());
            if (name == nameof(AnimalUpdatedCommand)) return new AnimalUpdatedCommandProcessor(stream, CommandValidatorFactory.Get<AnimalUpdatedCommand>());

            return null;
        }
    }
}
