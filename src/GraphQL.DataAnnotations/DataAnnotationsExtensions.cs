using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NServiceBus.Extensibility;

namespace NServiceBus
{
    /// <summary>
    /// Extensions to DataAnnotations.
    /// </summary>
    public static class DataAnnotationsExtensions
    {
        public static IReadOnlyDictionary<string,string> Headers(this ValidationContext validationContext)
        {
            Guard.AgainstNull(validationContext, nameof(validationContext));
            return (IReadOnlyDictionary<string, string>) validationContext.Items["Headers"];
        }

        public static ContextBag ContextBag(this ValidationContext validationContext)
        {
            Guard.AgainstNull(validationContext, nameof(validationContext));
            return (ContextBag) validationContext.Items["ContextBag"];
        }
    }
}