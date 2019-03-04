using GraphQL.FluentValidation;
using GraphQL.Types;

static class ArgumentTypeCacheBag
{
    public static ValidatorTypeCache GetCache(this ResolveFieldContext context)
    {
        return (ValidatorTypeCache) context.Arguments["GraphQL.FluentValidation.ValidatorTypeCache"];
    }

    public static void SetCache(this ResolveFieldContext context, ValidatorTypeCache cache)
    {
        context.Arguments.Add("GraphQL.FluentValidation.ValidatorTypeCache", cache);
    }

    public static void RemoveCache(this ResolveFieldContext context)
    {
        context.Arguments.Remove("GraphQL.FluentValidation.ValidatorTypeCache");
    }

    public static ValidatorTypeCache GetCache<T>(ResolveFieldContext<T> context)
    {
        return (ValidatorTypeCache) context.Arguments["GraphQL.FluentValidation.ValidatorTypeCache"];
    }
}