using FluentValidation;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        foreach (var type in GetGraphQLTypes())
        {
            services.AddSingleton(type);
        }

        services.AddValidatorsFromAssemblyContaining<Startup>();
        services.AddGraphQL(builder => builder
            .AddHttpMiddleware<Schema>()
            .AddSchema<Schema>()
            .ConfigureExecutionOptions(options =>
            {
                options.ThrowOnUnhandledException = true;
                options.UseFluentValidation(ValidatorCacheBuilder.InstanceDI);
            })
            .AddSystemTextJson()
            .AddGraphTypes(typeof(Schema).Assembly));
    }

    static IEnumerable<Type> GetGraphQLTypes() =>
        typeof(Startup).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract &&
                        (typeof(IObjectGraphType).IsAssignableFrom(x) ||
                         typeof(IInputObjectGraphType).IsAssignableFrom(x)));

    public void Configure(IApplicationBuilder builder)
    {
        builder.UseGraphQL<Schema>();
        builder.UseGraphQLGraphiQL();
    }
}