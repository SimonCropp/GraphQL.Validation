using FluentValidation;

public class AsyncInputValidator :
    AbstractValidator<AsyncInput>
{
    public AsyncInputValidator()
    {
        RuleFor(_ => _.Content)
            .MustAsync((s, _) => Task.FromResult(!string.IsNullOrWhiteSpace(s)));
    }
}