using FluentValidation;

static class ModuleInitializer
{
    public static void Initialize()
    {
        ValidatorOptions.Global.DisplayNameResolver = ValidatorOptions.Global.PropertyNameResolver;
    }
}