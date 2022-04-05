using FluentValidation;
using GraphiQl;
using GraphQL;
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
        services.AddSingleton<ISchema, Schema>();
        services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
        services.AddSingleton<IGraphQLSerializer, GraphQLSerializer>();
        var mvc = services.AddMvc(option => option.EnableEndpointRouting = false);
        mvc.AddNewtonsoftJson();
    }

    static IEnumerable<Type> GetGraphQLTypes() =>
        typeof(Startup).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract &&
                        (typeof(IObjectGraphType).IsAssignableFrom(x) ||
                         typeof(IInputObjectGraphType).IsAssignableFrom(x)));

    public void Configure(IApplicationBuilder builder)
    {
        builder.UseGraphiQl("/graphiql", "/graphql");
        builder.UseMvc();
    }
}