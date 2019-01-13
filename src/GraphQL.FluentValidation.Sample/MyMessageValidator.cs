using FluentValidation;
using NServiceBus;
// ReSharper disable UnusedVariable

public class MyMessageValidator : AbstractValidator<MyMessage>
{
    public MyMessageValidator()
    {
        RuleFor(_ => _.Content)
            .NotEmpty()
            .Custom((propertyValue, validationContext) =>
            {
                var pipelineContextBag = validationContext.ContextBag();
                var messageHeaders = validationContext.Headers();
                if (propertyValue == "User" &&
                    messageHeaders.ContainsKey("Auth"))
                {
                    validationContext.AddFailure("D");
                }
            });
    }
}