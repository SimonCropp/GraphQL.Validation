using System;
using System.Collections.Concurrent;
using GraphQL.Types;
using Type = System.Type;

static class ReturnTypeFinder
{
    static ConcurrentDictionary<Type, object?> typeCache = new ConcurrentDictionary<Type, object?>();

    public static object? Find(ResolveFieldContext context)
    {
        return typeCache.GetOrAdd(context.ReturnType.GetType(), type => GetDefault(GetReturnType(type)));
    }

    static Type GetReturnType(Type type)
    {
        while (type.BaseType != null)
        {
            type = type.BaseType;
            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(ObjectGraphType<>))
            {
                return type.GetGenericArguments()[0];
            }
        }
        throw new Exception($"Type is not a ObjectGraphType<>. Type: {type.FullName}");
    }

    static object? GetDefault(Type type)
    {
        if(type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }
        return null;
    }
}