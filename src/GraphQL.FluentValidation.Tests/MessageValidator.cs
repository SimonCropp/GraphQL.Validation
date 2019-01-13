using FluentValidation;
using FluentValidation.Results;
using NServiceBus;
using Xunit;

public class MessageValidator : AbstractValidator<MessageWithValidator>
{
    public MessageValidator()
    {
        RuleFor(_ => _.Content).NotEmpty();
    }

    public override ValidationResult Validate(ValidationContext<MessageWithValidator> context)
    {
        Assert.NotNull(context.Headers());
        Assert.NotNull(context.ContextBag());
        return base.Validate(context);
    }
}