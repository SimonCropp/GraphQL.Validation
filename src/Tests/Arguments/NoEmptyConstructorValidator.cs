using FluentValidation;
// ReSharper disable UnusedParameter.Local

public class NoEmptyConstructorValidator :
    AbstractValidator<Input>
{
    public NoEmptyConstructorValidator(string foo)
    {
    }
}