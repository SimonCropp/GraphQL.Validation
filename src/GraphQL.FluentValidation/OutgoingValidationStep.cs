using NServiceBus;
using NServiceBus.Pipeline;

class OutgoingValidationStep : RegisterStep
{
    public OutgoingValidationStep(FluentValidationConfig config) :
        base(
            stepId: "OutgoingFluentValidation",
            behavior: typeof(OutgoingValidationBehavior),
            description: "Validates outgoing messages using FluentValidation",
            factoryMethod: builder => BuildBehavior(config))
    {
    }

    static IBehavior BuildBehavior(FluentValidationConfig config)
    {
        return new OutgoingValidationBehavior(config.MessageValidator);
    }
}