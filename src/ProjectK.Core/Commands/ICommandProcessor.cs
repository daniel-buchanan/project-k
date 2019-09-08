namespace ProjectK.Core.Commands
{
    public interface ICommandProcessor
    {
        ValidationResult Validate(ICommand command);

        void Process(ICommand command);
    }

    public interface ICommandProcessor<T> : ICommandProcessor where T: ICommand
    {
        ValidationResult Validate(T command);

        void Process(T command);
    }
}
