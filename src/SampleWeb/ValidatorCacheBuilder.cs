using GraphQL.FluentValidation;

public static class ValidatorCacheBuilder
{
    public static ValidatorTypeCache Instance;

    static ValidatorCacheBuilder()
    {
        Instance = new ValidatorTypeCache(useDependencyInjection: true);
        Instance.AddValidatorsFromAssembly(typeof(Startup).Assembly);
    }
}