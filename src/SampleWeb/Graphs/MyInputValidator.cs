using FluentValidation;

#region validator
public class MyInputValidator :
    AbstractValidator<MyInput>
{
    public MyInputValidator()
    {
        RuleFor(_ => _.Content)
            .NotEmpty();
    }
}
#endregion