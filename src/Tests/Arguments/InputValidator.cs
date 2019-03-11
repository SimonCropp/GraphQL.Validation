using FluentValidation;

public class InputValidator : AbstractValidator<Input>
{
    public InputValidator()
    {
        RuleFor(_ => _.Content)
            .NotEmpty();
    }
}