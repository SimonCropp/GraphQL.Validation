using Microsoft.Extensions.DependencyInjection;
using System;

public class Schema : GraphQL.Types.Schema
{
    public Schema(IServiceProvider serviceProvider, Query query) :
        base(serviceProvider)
    {
        Query = query;
    }
}