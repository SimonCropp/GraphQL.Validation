using FluentValidation;
using FluentValidation.Results;
using GraphQL;
using Xunit;

public class InputValidator : AbstractValidator<Input>
{
    public InputValidator()
    {
        RuleFor(_ => _.Content)
            .NotEmpty();
    }

    public override ValidationResult Validate(ValidationContext<Input> context)
    {
        Assert.NotNull(context.UserContext<MyUserContext>());
        return base.Validate(context);
    }
}