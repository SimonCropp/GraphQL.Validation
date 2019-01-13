using FluentValidation;
using NServiceBus.FluentValidation;

namespace NServiceBus
{
    /// <summary>
    /// Extensions to control message validation with FluentValidation.
    /// </summary>
    public static class FluentValidationConfigurationExtensions
    {
        public static FluentValidationConfig UseFluentValidation(
            this EndpointConfiguration endpoint,
            ValidatorLifecycle lifecycle = ValidatorLifecycle.Endpoint,
            bool incoming = true,
            bool outgoing = true)
        {
            Guard.AgainstNull(endpoint, nameof(endpoint));
            var recoverability = endpoint.Recoverability();
            recoverability.AddUnrecoverableException<ValidationException>();
            var config = new FluentValidationConfig(endpoint, lifecycle);
            var pipeline = endpoint.Pipeline;
            if (incoming)
            {
                pipeline.Register(new IncomingValidationStep(config));
            }
            if (outgoing)
            {
                pipeline.Register(new OutgoingValidationStep(config));
            }
            return config;
        }
    }
}