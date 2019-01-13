using NServiceBus.Pipeline;

class IncomingValidationStep : RegisterStep
{
    public IncomingValidationStep() :
        base("IncomingDataAnnotations", typeof(IncomingValidationBehavior), "Validates incoming messages using DataAnnotations",
            builder => new IncomingValidationBehavior())
    {
    }
}