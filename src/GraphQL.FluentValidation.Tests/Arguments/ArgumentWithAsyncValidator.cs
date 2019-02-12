using System.Threading.Tasks;
using FluentValidation;

public class ArgumentWithAsyncValidator : AbstractValidator<ArgumentWithAsync>
{
    public ArgumentWithAsyncValidator()
    {
        RuleFor(_ => _.Content)
            .MustAsync((s, token) => Task.FromResult(s != null));
    }
}