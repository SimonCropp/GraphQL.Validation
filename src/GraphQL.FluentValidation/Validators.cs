using System.Linq;
using FluentValidation;

namespace GraphQL.FluentValidation
{
    public static class Validators
    {
        public static void NotWhiteSpace<T>(this IRuleBuilderInitial<T, string> builder)
        {
            Guard.AgainstNull(builder, nameof(builder));
            builder
                .Must(u =>
                {
                    if (u == null)
                    {
                        return true;
                    }

                    return u.Any(char.IsWhiteSpace);
                })
                .WithMessage((type, member) => $"'{member}' must not be only whitespace.");
        }
    }
}