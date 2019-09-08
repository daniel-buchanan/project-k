using ProjectK.Core.Commands;
using ProjectK.ES.Commands.Validators;

namespace ProjectK.ES.Commands
{
    public static class CommandValidatorFactory
    {
        public static ICommandValidator<T> Get<T>() where T: ICommand
        {
            var name = typeof(T).Name;
            if (name == nameof(AnimalArrivedCommand)) return (ICommandValidator<T>)new AnimalArrivedCommandValidator();

            return null;
        }
    }
}
