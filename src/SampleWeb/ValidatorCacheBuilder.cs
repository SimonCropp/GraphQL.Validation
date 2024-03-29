﻿using GraphQL.FluentValidation;

public static class ValidatorCacheBuilder
{
    public static ValidatorInstanceCache Instance;

    public static ValidatorServiceCache InstanceDI;

    static ValidatorCacheBuilder()
    {
        Instance = new();
        Instance.AddValidatorsFromAssembly(typeof(Startup).Assembly);

        InstanceDI = new();
        InstanceDI.AddValidatorsFromAssembly(typeof(Startup).Assembly);
    }
}