using FluentValidation;

public static class ModuleInitializer
{
    public static void Initialize()
    {
        ValidatorOptions.DisplayNameResolver = ValidatorOptions.PropertyNameResolver;
    }
}