using GraphQL.FluentValidation;

public static class ValidatorCacheBuilder
{
    public static ValidatorTypeCache Instance = new ValidatorTypeCache().AddValidatorsFromAssembly(typeof(Startup).Assembly);
}