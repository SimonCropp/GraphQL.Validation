using GraphQL.FluentValidation;

public static class ValidatorCacheBuilder
{
    public static ValidatorTypeCache Instance;

    public static ValidatorTypeCache InstanceDI;

    static ValidatorCacheBuilder()
    {
        Instance = new ValidatorTypeCache(useDependencyInjection: false);
        Instance.AddValidatorsFromAssembly(typeof(Startup).Assembly);

        InstanceDI = new ValidatorTypeCache(useDependencyInjection: true);
        InstanceDI.AddValidatorsFromAssembly(typeof(Startup).Assembly);
    }
}