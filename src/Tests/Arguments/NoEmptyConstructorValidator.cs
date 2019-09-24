using FluentValidation;

public class NoEmptyConstructorValidator :
    AbstractValidator<Input>
{
    public NoEmptyConstructorValidator(string foo)
    {
    }
}