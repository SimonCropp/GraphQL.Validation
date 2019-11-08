using FluentValidation;
using System.Threading.Tasks;

public class AsyncComplexInputValidator :
    AbstractValidator<AsyncComplexInput>
{
    public AsyncComplexInputValidator()
    {
        RuleFor(_ => _.Inner!)
            .NotEmpty()
            .MustAsync((o, token) => {
                return Task.FromResult(o != null && !string.IsNullOrWhiteSpace(o.Content));
            }).WithMessage("Inner async test failed msg.")
            .SetValidator(new ComplexInputInnerValidator());
    }
}