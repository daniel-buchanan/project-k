using ProjectK.Core.Events;

namespace ProjectK.Core.Commands
{
    public abstract class AbstractCommandProcessor<T> : ICommandProcessor<T> where T: ICommand
    {
        private readonly ICommandValidator<T> _validator;
        private readonly IEventStream _eventStream;

        protected AbstractCommandProcessor(IEventStream eventStream,
            ICommandValidator<T> validator)
        {
            _eventStream = eventStream;
            _validator = validator;
        }

        protected IEventStream EventStream => _eventStream;

        public ValidationResult Validate(T command)
        {
            return _validator?.Validate(command) ?? ValidationResult.Success();
        }

        public abstract void Process(T command);

        public ValidationResult Validate(ICommand command) => Validate((T) command);

        public void Process(ICommand command) => Process((T) command);
    }
}
