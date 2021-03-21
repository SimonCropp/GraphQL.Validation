using System;

public class Schema : GraphQL.Types.Schema
{
    public Schema(IServiceProvider serviceProvider, Query query) :
        base(serviceProvider)
    {
        RegisterTypeMapping(typeof(MyInput), typeof(MyInputGraph));
        RegisterTypeMapping(typeof(Result), typeof(ResultGraph));
        Query = query;
    }
}