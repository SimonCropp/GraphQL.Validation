using FluentValidation;

public class ComplexInputInnerValidator :
    AbstractValidator<ComplexInputInner>
{
    public ComplexInputInnerValidator()
    {
        RuleFor(_ => _.Content)
            .NotEmpty();
    }
}