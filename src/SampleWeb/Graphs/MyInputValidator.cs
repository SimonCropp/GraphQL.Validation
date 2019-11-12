using FluentValidation;

public class MyInputValidator :
    AbstractValidator<MyInput>
{
    public MyInputValidator()
    {
        RuleFor(_ => _.Content)
            .NotEmpty();
    }
}
