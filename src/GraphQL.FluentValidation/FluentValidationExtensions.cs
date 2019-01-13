using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using NServiceBus.Extensibility;

namespace NServiceBus
{
    /// <summary>
    /// Extensions to FluentValidation.
    /// </summary>
    public static class FluentValidationExtensions
    {
        public static IReadOnlyDictionary<string, string> Headers(this CustomContext customContext)
        {
            Guard.AgainstNull(customContext, nameof(customContext));
            return customContext.ParentContext.Headers();
        }

        public static ContextBag ContextBag(this CustomContext customContext)
        {
            Guard.AgainstNull(customContext, nameof(customContext));
            return customContext.ParentContext.ContextBag();
        }

        public static IReadOnlyDictionary<string, string> Headers(this ValidationContext validationContext)
        {
            Guard.AgainstNull(validationContext, nameof(validationContext));
            return (IReadOnlyDictionary<string, string>)validationContext.RootContextData["Headers"];
        }

        public static ContextBag ContextBag(this ValidationContext validationContext)
        {
            Guard.AgainstNull(validationContext, nameof(validationContext));
            return (ContextBag)validationContext.RootContextData["ContextBag"];
        }
    }
}