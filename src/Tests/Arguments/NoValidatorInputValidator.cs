using FluentValidation;

// ReSharper disable once UnusedTypeParameter
public class NoValidatorInputValidator<T> :
    AbstractValidator<NoValidatorInput>
{
    public NoValidatorInputValidator() =>
        RuleFor(_ => _.Content)
            .NotEmpty();
}