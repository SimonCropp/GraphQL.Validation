using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class OutgoingValidationBehavior : Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        Validate(context);
        return next();
    }

    static void Validate(IOutgoingLogicalMessageContext context)
    {
        MessageValidator.Validate(context.Message.Instance, context.Builder, context.Headers, context.Extensions);
    }
}