<!--
This file was generate by the MarkdownSnippets.
Source File: \readme.source.md
To change this file edit the source file and then re-run the generation using either the dotnet global tool (https://github.com/SimonCropp/MarkdownSnippets#githubmarkdownsnippets) or using the api (https://github.com/SimonCropp/MarkdownSnippets#running-as-a-unit-test).
-->

# <img src="https://raw.githubusercontent.com/SimonCropp/GraphQL.Validation/master/src/icon.png" height="40px"> GraphQL.FluentValidation

Add [FluentValidation](https://fluentvalidation.net/) support to [GraphQL.net](https://github.com/graphql-dotnet/graphql-dotnet)


## NuGet [![NuGet Status](https://badgen.net/nuget/v/GraphQL.FluentValidation/pre)](https://www.nuget.org/packages/GraphQL.FluentValidation/)

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
<sup>[snippet source](/src/Tests/Snippets/QueryExecution.cs#L10-L17)</sup>
<!-- endsnippet -->

Generally ValidatorTypeCache is scoped per app and can be collocated with `Schema`, `DocumentExecuter` initialization.


### Add to ExecutionOptions

Validation needs to be added to any instance of `ExecutionOptions`.

<!-- snippet: UseFluentValidation -->
```cs
var options = new ExecutionOptions
{
    Schema = schema,
    Query = queryString,
    UserContext = new MyUserContext(),
    Inputs = inputs
};
options.UseFluentValidation(validatorTypeCache);

var executionResult = await executer.ExecuteAsync(options)
    .ConfigureAwait(false);
```
<sup>[snippet source](/src/Tests/Snippets/QueryExecution.cs#L22-L36)</sup>
<!-- endsnippet -->


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
