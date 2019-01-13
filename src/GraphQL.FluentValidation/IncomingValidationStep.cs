using NServiceBus;
using NServiceBus.Pipeline;

class IncomingValidationStep : RegisterStep
{
    public IncomingValidationStep(FluentValidationConfig config) :
        base(
            stepId: "IncomingFluentValidation",
            behavior: typeof(IncomingValidationBehavior),
            description: "Validates incoming messages using FluentValidation",
            factoryMethod: builder => BuildBehavior(config))
    {
    }

    static IBehavior BuildBehavior(FluentValidationConfig config)
    {
        return new IncomingValidationBehavior(config.MessageValidator);
    }
}