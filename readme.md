<!--
This file was generate by MarkdownSnippets.
Source File: \readme.source.md
To change this file edit the source file and then re-run the generation using either the dotnet global tool (https://github.com/SimonCropp/MarkdownSnippets#githubmarkdownsnippets) or using the api (https://github.com/SimonCropp/MarkdownSnippets#running-as-a-unit-test).
-->
# <img src="https://raw.githubusercontent.com/SimonCropp/GraphQL.Validation/master/src/icon.png" height="40px"> GraphQL.FluentValidation

Add [FluentValidation](https://fluentvalidation.net/) support to [GraphQL.net](https://github.com/graphql-dotnet/graphql-dotnet)


## NuGet [![NuGet Status](https://badgen.net/nuget/v/GraphQL.FluentValidation/)](https://www.nuget.org/packages/GraphQL.FluentValidation/)

https://nuget.org/packages/GraphQL.FluentValidation/

    PM> Install-Package GraphQL.FluentValidation


## Usage


### Define validators

Given the following input:

<!-- snippet: input -->
```cs
public class MyInput
{
    public string Content { get; set; }
}
```
<sup>[snippet source](/src/Tests/Snippets/MyInput.cs#L2-L7)</sup>
<!-- endsnippet -->

And graph:

<!-- snippet: graph -->
```cs
public class MyInputGraph : InputObjectGraphType
{
    public MyInputGraph()
    {
        Field<StringGraphType>("content");
    }
}
```
<sup>[snippet source](/src/Tests/Snippets/MyInputGraph.cs#L3-L11)</sup>
<!-- endsnippet -->

A custom validator can be defined as follows:

<!-- snippet: validator -->
```cs
public class MyInputValidator :
    AbstractValidator<MyInput>
{
    public MyInputValidator()
    {
        RuleFor(_ => _.Content)
            .NotEmpty();
    }
}
```
<sup>[snippet source](/src/Tests/Snippets/MyInputValidator.cs#L3-L13)</sup>
<!-- endsnippet -->


### Setup Validators

Validators need to be added to the `ValidatorTypeCache`. This should be done once at application startup.

<!-- snippet: StartConfig -->
```cs
var validatorTypeCache = new ValidatorTypeCache();
validatorTypeCache.AddValidatorsFromAssembly(assemblyContainingValidators);
var schema = new Schema();
var executer = new DocumentExecuter();
```
<sup>[snippet source](/src/Tests/Snippets/QueryExecution.cs#L11-L18)</sup>
<!-- endsnippet -->

Generally `ValidatorTypeCache` is scoped per app and can be collocated with `Schema`, `DocumentExecuter` initialization.


### Add to ExecutionOptions

Validation needs to be added to any instance of `ExecutionOptions`.

<!-- snippet: UseFluentValidation -->
```cs
var options = new ExecutionOptions
{
    Schema = schema,
    Query = queryString,
    Inputs = inputs
};
options.UseFluentValidation(validatorTypeCache);

var executionResult = await executer.ExecuteAsync(options);
```
<sup>[snippet source](/src/Tests/Snippets/QueryExecution.cs#L23-L35)</sup>
<!-- endsnippet -->


### UserContext must be a dictionary

This library needs to be able to pass the list of validators, in the form of `ValidatorTypeCache`, through the graphql context. The only way of achieving this is to use the `ExecutionOptions.UserContext`. To facilitate this, the type passed to `ExecutionOptions.UserContext` has to implement `IDictionary<string, object>`. There are two approaches to achieving this:


#### 1. Have the user context class implement IDictionary

Given a user context class of the following form:

<!-- snippet: ContextImplementingDictionary -->
```cs
public class MyUserContext :
    Dictionary<string, object>
{
    public string MyProperty { get; set; }
}
```
<sup>[snippet source](/src/Tests/Snippets/QueryExecution.cs#L38-L46)</sup>
<!-- endsnippet -->

The `ExecutionOptions.UserContext` can then be set as follows:

<!-- snippet: ExecuteQueryWithContextImplementingDictionary -->
```cs
var options = new ExecutionOptions
{
    Schema = schema,
    Query = queryString,
    Inputs = inputs,
    UserContext = new MyUserContext
    {
        MyProperty = "the value"
    }
};
options.UseFluentValidation(validatorTypeCache);
```
<sup>[snippet source](/src/Tests/Snippets/QueryExecution.cs#L50-L64)</sup>
<!-- endsnippet -->


#### 2. Have the user context class exist inside a IDictionary

<!-- snippet: ExecuteQueryWithContextInsideDictionary -->
```cs
var options = new ExecutionOptions
{
    Schema = schema,
    Query = queryString,
    Inputs = inputs,
    UserContext = new Dictionary<string, object>
    {
        {
            "MyUserContext",
            new MyUserContext
            {
                MyProperty = "the value"
            }
        }
    }
};
options.UseFluentValidation(validatorTypeCache);
```
<sup>[snippet source](/src/Tests/Snippets/QueryExecution.cs#L69-L89)</sup>
<!-- endsnippet -->


#### No UserContext

If no instance is passed to `ExecutionOptions.UserContext`:

<!-- snippet: NoContext -->
```cs
var options = new ExecutionOptions
{
    Schema = schema,
    Query = queryString,
    Inputs = inputs
};
options.UseFluentValidation(validatorTypeCache);
```
<sup>[snippet source](/src/Tests/Snippets/QueryExecution.cs#L94-L104)</sup>
<!-- endsnippet -->

Then the `UseFluentValidation` method will instantiate it to a new `Dictionary<string, object>`.



### Validate when reading arguments

When reading arguments use `GetValidatedArgument` instead of `GetArgument`:

<!-- snippet: GetValidatedArgument -->
```cs
public class MyQuery :
    ObjectGraphType
{
    public MyQuery()
    {
        Field<ResultGraph>(
            "inputQuery",
            arguments: new QueryArguments(
                new QueryArgument<MyInputGraph>
                {
                    Name = "input"
                }
            ),
            resolve: context =>
            {
                var input = context.GetValidatedArgument<MyInput>("input");
                return new Result
                {
                    Data = input.Content
                };
            }
        );
    }
}
```
<sup>[snippet source](/src/Tests/Snippets/Query.cs#L4-L31)</sup>
<!-- endsnippet -->


## Icon

<a href="https://thenounproject.com/term/shield/1893182/" target="_blank">Shield</a> designed by [Maxim Kulikov](https://thenounproject.com/maxim221/) from [The Noun Project](https://thenounproject.com)
