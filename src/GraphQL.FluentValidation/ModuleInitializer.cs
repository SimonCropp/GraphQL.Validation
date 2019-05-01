using FluentValidation;

static class ModuleInitializer
{
    public static void Initialize()
    {
        ValidatorOptions.DisplayNameResolver = ValidatorOptions.PropertyNameResolver;
    }
}