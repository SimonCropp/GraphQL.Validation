using System.Threading.Tasks;
using FluentValidation;

public class AsyncInputValidator : AbstractValidator<AsyncInput>
{
    public AsyncInputValidator()
    {
        RuleFor(_ => _.Content)
            .MustAsync((s, token) => Task.FromResult(!string.IsNullOrWhiteSpace(s)));
    }
}