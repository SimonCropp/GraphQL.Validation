using GraphQL.FluentValidation;

public static class ValidatorCacheBuilder
{
    public static ValidatorTypeCache ValidatorTypeCache;

    static ValidatorCacheBuilder()
    {
        ValidatorTypeCache = new ValidatorTypeCache();
        ValidatorTypeCache.AddValidatorsFromAssembly(typeof(Startup).Assembly);
    }
}