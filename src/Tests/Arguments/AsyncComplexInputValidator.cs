using FluentValidation;

public class AsyncComplexInputValidator :
    AbstractValidator<AsyncComplexInput>
{
    public AsyncComplexInputValidator() =>
        RuleFor(_ => _.Inner!)
            .NotEmpty()
            .MustAsync((o, _) => Task.FromResult(o != null && !string.IsNullOrWhiteSpace(o.Content)))
            .WithMessage("Inner async test failed msg.")
            .SetValidator(new ComplexInputInnerValidator());
}