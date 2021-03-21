using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using GraphiQl;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        foreach (var type in GetGraphQLTypes())
        {
            services.AddSingleton(type);
        }

        services.AddValidatorsFromAssemblyContaining<Startup>();
        services.AddSingleton<ISchema, Schema>();
        services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
        services.AddSingleton<IDocumentWriter, DocumentWriter>();
        var mvc = services.AddMvc(option => option.EnableEndpointRouting = false);
        mvc.SetCompatibilityVersion(CompatibilityVersion.Latest);
        mvc.AddNewtonsoftJson();
    }

    static IEnumerable<Type> GetGraphQLTypes()
    {
        return typeof(Startup).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract &&
                        (typeof(IObjectGraphType).IsAssignableFrom(x) ||
                         typeof(IInputObjectGraphType).IsAssignableFrom(x)));
    }

    public void Configure(IApplicationBuilder builder)
    {
        builder.UseGraphiQl("/graphiql", "/graphql");
        builder.UseMvc();
    }
}