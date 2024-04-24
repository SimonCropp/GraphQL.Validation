using FluentValidation;
using GraphQL;
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
            .Where(_ => !_.IsAbstract &&
                        (_.IsAssignableTo(typeof(IObjectGraphType)) ||
                         _.IsAssignableTo(typeof(IInputObjectGraphType))));

    public void Configure(IApplicationBuilder builder)
    {
        builder.UseGraphQL<Schema>();
        builder.UseGraphQLGraphiQL();
    }
}