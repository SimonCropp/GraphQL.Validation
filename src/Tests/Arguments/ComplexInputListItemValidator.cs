using FluentValidation;

public class ComplexInputListItemValidator :
    AbstractValidator<ComplexInputListItem>
{
    public ComplexInputListItemValidator()
    {
        RuleFor(_ => _.Id)
            .NotEmpty();

        RuleFor(_ => _.Content)
            .NotEmpty();
    }
}