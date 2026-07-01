using FluentValidation;
using GraphQL;

public class BookingInput
{
    public DateTimeOffset Start { get; set; }
}

#region ServiceProviderInValidator
public class BookingInputValidator :
    AbstractValidator<BookingInput>
{
    public BookingInputValidator() =>
        RuleFor(_ => _.Start)
            .Must((_, start, context) =>
            {
                var clock = context.GetRequiredService<TimeProvider>();
                return start >= clock.GetUtcNow();
            })
            .WithMessage("Start cannot be in the past");
}
#endregion
