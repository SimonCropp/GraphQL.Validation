using FluentValidation;

static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        ValidatorOptions.Global.DisplayNameResolver = ValidatorOptions.Global.PropertyNameResolver;
    }
}