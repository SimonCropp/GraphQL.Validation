using Microsoft.Extensions.DependencyInjection;
using System;

public class Schema : GraphQL.Types.Schema
{
    public Schema(IServiceProvider serviceProvider) :
        base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<Query>();
    }
}