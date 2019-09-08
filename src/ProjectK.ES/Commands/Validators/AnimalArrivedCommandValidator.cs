using System;
using ProjectK.Core;
using ProjectK.Core.Commands;

namespace ProjectK.ES.Commands.Validators
{
    public class AnimalArrivedCommandValidator : ICommandValidator<AnimalArrivedCommand>
    {
        public ValidationResult Validate(AnimalArrivedCommand command)
        {
            var result = new ValidationResult();

            if (command == null) return result;
            if (string.IsNullOrEmpty(command.Observation?.From)) return result;
            if (command.Observation.Animals.Count == 0) return result;

            result.MarkSuccess();

            return result;
        }

        public ValidationResult Validate(ICommand command)
        {
            return Validate((AnimalArrivedCommand) command);
        }
    }
}
