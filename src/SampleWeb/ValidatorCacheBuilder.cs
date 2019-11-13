using GraphQL.FluentValidation;

public static class ValidatorCacheBuilder
{
    public static ValidatorTypeCache Instance;

    static ValidatorCacheBuilder()
    {
        Instance = new ValidatorTypeCache();
        Instance.AddValidatorsFromAssembly(typeof(Startup).Assembly);
    }
}