# <img src="/src/icon.png" height="30px"> GraphQL.Validation

[![Build status](https://ci.appveyor.com/api/projects/status/wvk8wm3n227b2b3q/branch/main?svg=true)](https://ci.appveyor.com/project/SimonCropp/graphql-validation)
[![NuGet Status](https://img.shields.io/nuget/v/GraphQL.FluentValidation.svg)](https://www.nuget.org/packages/GraphQL.FluentValidation/)

Add [FluentValidation](https://fluentvalidation.net/) support to [GraphQL.net](https://github.com/graphql-dotnet/graphql-dotnet)

**See [Milestones](../../milestones?state=closed) for release notes.**


## NuGet package

https://nuget.org/packages/GraphQL.FluentValidation/


## Usage


### Define validators

Given the following input:

<!-- snippet: input -->
<a id='snippet-input'></a>
```cs
public class MyInput
{
    public string Content { get; set; } = null!;
}
```
<sup><a href='/src/SampleWeb/Graphs/MyInput.cs#L1-L8' title='Snippet source file'>snippet source</a> | <a href='#snippet-input' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

And graph:

<!-- snippet: graph -->
<a id='snippet-graph'></a>
```cs
public class MyInputGraph :
    InputObjectGraphType
{
    public MyInputGraph() =>
        Field<StringGraphType>("content");
}
```
<sup><a href='/src/SampleWeb/Graphs/MyInputGraph.cs#L3-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-graph' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

A custom validator can be defined as follows:

<!-- snippet: validator -->
<a id='snippet-validator'></a>
```cs
public class MyInputValidator :
    AbstractValidator<MyInput>
{
    public MyInputValidator() =>
        RuleFor(_ => _.Content)
            .NotEmpty();
}
```
<sup><a href='/src/SampleWeb/Graphs/MyInputValidator.cs#L3-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-validator' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Setup Validators

Validators need to be added to the `ValidatorTypeCache`. This should be done once at application startup.

<!-- snippet: StartConfig -->
<a id='snippet-startconfig'></a>
```cs
var validatorCache = new ValidatorInstanceCache();
validatorCache.AddValidatorsFromAssembly(assemblyContainingValidators);
var schema = new Schema();
schema.UseFluentValidation();
var executer = new DocumentExecuter();
```
<sup><a href='/src/Tests/Snippets/QueryExecution.cs#L16-L24' title='Snippet source file'>snippet source</a> | <a href='#snippet-startconfig' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Generally `ValidatorTypeCache` is scoped per app and can be collocated with `Schema`, `DocumentExecuter` initialization.

Dependency Injection can be used for validators. Create a `ValidatorTypeCache` with the
`useDependencyInjection: true` parameter and call one of the `AddValidatorsFrom*` methods from
[FluentValidation.DependencyInjectionExtensions](https://www.nuget.org/packages/FluentValidation.DependencyInjectionExtensions/)
package in the `Startup`. By default, validators are added to the DI container with a transient lifetime.


### Add to ExecutionOptions

Validation needs to be added to any instance of `ExecutionOptions`.

<!-- snippet: UseFluentValidation -->
<a id='snippet-usefluentvalidation'></a>
```cs
var options = new ExecutionOptions
{
    Schema = schema,
    Query = queryString,
    Variables = inputs
};
options.UseFluentValidation(validatorCache);

var executionResult = await executer.ExecuteAsync(options);
```
<sup><a href='/src/Tests/Snippets/QueryExecution.cs#L29-L41' title='Snippet source file'>snippet source</a> | <a href='#snippet-usefluentvalidation' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### UserContext must be a dictionary

This library needs to be able to pass the list of validators, in the form of `ValidatorTypeCache`, through the graphql context. The only way of achieving this is to use the `ExecutionOptions.UserContext`. To facilitate this, the type passed to `ExecutionOptions.UserContext` has to implement `IDictionary<string, object>`. There are two approaches to achieving this:


#### 1. Have the user context class implement IDictionary

Given a user context class of the following form:

<!-- snippet: ContextImplementingDictionary -->
<a id='snippet-contextimplementingdictionary'></a>
```cs
public class MyUserContext(string myProperty) :
    Dictionary<string, object?>
{
    public string MyProperty { get; } = myProperty;
}
```
<sup><a href='/src/Tests/Snippets/QueryExecution.cs#L44-L52' title='Snippet source file'>snippet source</a> | <a href='#snippet-contextimplementingdictionary' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The `ExecutionOptions.UserContext` can then be set as follows:

<!-- snippet: ExecuteQueryWithContextImplementingDictionary -->
<a id='snippet-executequerywithcontextimplementingdictionary'></a>
```cs
var options = new ExecutionOptions
{
    Schema = schema,
    Query = queryString,
    Variables = inputs,
    UserContext = new MyUserContext
    (
        myProperty: "the value"
    )
};
options.UseFluentValidation(validatorCache);
```
<sup><a href='/src/Tests/Snippets/QueryExecution.cs#L56-L70' title='Snippet source file'>snippet source</a> | <a href='#snippet-executequerywithcontextimplementingdictionary' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### 2. Have the user context class exist inside a IDictionary

<!-- snippet: ExecuteQueryWithContextInsideDictionary -->
<a id='snippet-executequerywithcontextinsidedictionary'></a>
```cs
var options = new ExecutionOptions
{
    Schema = schema,
    Query = queryString,
    Variables = inputs,
    UserContext = new Dictionary<string, object?>
    {
        {
            "MyUserContext",
            new MyUserContext
            (
                myProperty: "the value"
            )
        }
    }
};
options.UseFluentValidation(validatorCache);
```
<sup><a href='/src/Tests/Snippets/QueryExecution.cs#L75-L95' title='Snippet source file'>snippet source</a> | <a href='#snippet-executequerywithcontextinsidedictionary' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### No UserContext

If no instance is passed to `ExecutionOptions.UserContext`:

<!-- snippet: NoContext -->
<a id='snippet-nocontext'></a>
```cs
var options = new ExecutionOptions
{
    Schema = schema,
    Query = queryString,
    Variables = inputs
};
options.UseFluentValidation(validatorCache);
```
<sup><a href='/src/Tests/Snippets/QueryExecution.cs#L100-L110' title='Snippet source file'>snippet source</a> | <a href='#snippet-nocontext' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Then the `UseFluentValidation` method will instantiate it to a new `Dictionary<string, object>`.


### Trigger validation

To trigger the validation, when reading arguments use `GetValidatedArgument` instead of `GetArgument`:

<!-- snippet: GetValidatedArgument -->
<a id='snippet-getvalidatedargument'></a>
```cs
public class Query :
    ObjectGraphType
{
    public Query() =>
        Field<ResultGraph>("inputQuery")
            .Argument<MyInputGraph>("input")
            .Resolve(context =>
                {
                    var input = context.GetValidatedArgument<MyInput>("input");
                    return new Result
                    {
                        Data = input.Content
                    };
                }
            );
}
```
<sup><a href='/src/SampleWeb/Query.cs#L4-L23' title='Snippet source file'>snippet source</a> | <a href='#snippet-getvalidatedargument' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Difference from IValidationRule

The validation implemented in this project has nothing to do with the validation of the incoming GraphQL
request, which is described in the [official specification](http://spec.graphql.org/June2018/#sec-Validation).
[GraphQL.NET](https://github.com/graphql-dotnet/graphql-dotnet) has a concept of [validation rules](https://github.com/graphql-dotnet/graphql-dotnet/blob/master/src/GraphQL/Validation/IValidationRule.cs)
that would work **before** request execution stage. In this project validation occurs for input arguments
**at the request execution stage**. This additional validation complements but does not replace the standard
set of validation rules.


## Testing

### Integration

A full end-to-en test can be run against the GraphQL controller:

<!-- snippet: GraphQlControllerTests -->
<a id='snippet-graphqlcontrollertests'></a>
```cs
[UsesVerify]
public class GraphQLControllerTests
{
    [Fact]
    public async Task RunQuery()
    {
        using var server = GetTestServer();
        using var client = server.CreateClient();
        var query = """
                    {
                      inputQuery(input: {content: "TheContent"}) {
                        data
                      }
                    }
                    """;
        var body = new
        {
            query
        };
        var serialized = JsonConvert.SerializeObject(body);
        using var content = new StringContent(
            serialized,
            Encoding.UTF8,
            "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, "graphql")
        {
            Content = content
        };
        using var response = await client.SendAsync(request);
        await Verify(response);
    }

    static TestServer GetTestServer()
    {
        var builder = new WebHostBuilder();
        builder.UseStartup<Startup>();
        return new(builder);
    }
}
```
<sup><a href='/src/SampleWeb.Tests/GraphQLControllerTests.cs#L5-L47' title='Snippet source file'>snippet source</a> | <a href='#snippet-graphqlcontrollertests' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Unit

Unit tests can be run a specific field of a query:

<!-- snippet: QueryTests -->
<a id='snippet-querytests'></a>
```cs
[UsesVerify]
public class QueryTests
{
    [Fact]
    public async Task RunInputQuery()
    {
        var field = new Query().GetField("inputQuery")!;

        var userContext = new GraphQLUserContext();
        FluentValidationExtensions.AddCacheToContext(
            userContext,
            ValidatorCacheBuilder.Instance);

        var input = new MyInput
        {
            Content = "TheContent"
        };
        var fieldContext = new ResolveFieldContext
        {
            Arguments = new Dictionary<string, ArgumentValue>
            {
                {
                    "input", new ArgumentValue(input, ArgumentSource.Variable)
                }
            },
            UserContext = userContext
        };
        var result = await field.Resolver!.ResolveAsync(fieldContext);
        await Verify(result);
    }

    [Fact]
    public Task RunInvalidInputQuery()
    {
        Thread.CurrentThread.CurrentUICulture = new("en-US");
        var field = new Query().GetField("inputQuery")!;

        var userContext = new GraphQLUserContext();
        FluentValidationExtensions.AddCacheToContext(
            userContext,
            ValidatorCacheBuilder.Instance);

        var value = new Dictionary<string, object>();
        var fieldContext = new ResolveFieldContext
        {
            Arguments = new Dictionary<string, ArgumentValue>
            {
                {
                    "input", new ArgumentValue(value, ArgumentSource.Variable)
                }
            },
            UserContext = userContext
        };
        var exception = Assert.Throws<ValidationException>(
            () => field.Resolver!.ResolveAsync(fieldContext));
        return Verify(exception.Message);
    }
}
```
<sup><a href='/src/SampleWeb.Tests/QueryTests.cs#L5-L66' title='Snippet source file'>snippet source</a> | <a href='#snippet-querytests' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Shield](https://thenounproject.com/term/shield/1893182/) designed by [Maxim Kulikov](https://thenounproject.com/maxim221/) from [The Noun Project](https://thenounproject.com)
