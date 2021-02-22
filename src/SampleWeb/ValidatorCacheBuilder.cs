using GraphQL.FluentValidation;

public static class ValidatorCacheBuilder
{
    public static ValidatorTypeCache Instance;

    public static ValidatorTypeCache InstanceDI;

    static ValidatorCacheBuilder()
    {
        Instance = new(useDependencyInjection: false);
        Instance.AddValidatorsFromAssembly(typeof(Startup).Assembly);

        InstanceDI = new(useDependencyInjection: true);
        InstanceDI.AddValidatorsFromAssembly(typeof(Startup).Assembly);
    }
}