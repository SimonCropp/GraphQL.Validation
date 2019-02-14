using FluentValidation;

namespace GraphQL.FluentValidation
{
    public static class Validators
    {
        public static void NotWhiteSpace<T>(this IRuleBuilderInitial<T, string> builder)
        {
            Guard.AgainstNull(builder, nameof(builder));
            builder
                .Must(value =>
                {
                    if (value == null)
                    {
                        return true;
                    }

                    return !string.IsNullOrWhiteSpace(value);
                })
                .WithMessage("{PropertyName} must not be only whitespace.");
        }
    }
}