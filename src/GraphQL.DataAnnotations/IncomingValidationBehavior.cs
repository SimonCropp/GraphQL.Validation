using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class IncomingValidationBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        Validate(context);
        return next();
    }

    static void Validate(IIncomingLogicalMessageContext context)
    {
        MessageValidator.Validate(context.Message.Instance, context.Builder, context.Headers, context.Extensions);
    }
}