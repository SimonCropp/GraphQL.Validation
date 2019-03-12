# <img src="https://raw.githubusercontent.com/SimonCropp/GraphQL.Validation/master/src/icon.png" height="40px"> GraphQL.FluentValidation

Add [FluentValidation](https://fluentvalidation.net/) support to [GraphQL.net](https://github.com/graphql-dotnet/graphql-dotnet)


## NuGet [![NuGet Status](https://badgen.net/nuget/v/GraphQL.FluentValidation/)](https://www.nuget.org/packages/GraphQL.FluentValidation/)

https://nuget.org/packages/GraphQL.FluentValidation/

    PM> Install-Package GraphQL.FluentValidation


## Usage


### Define validators

Given the following input:

snippet: input

And graph:

snippet: graph

A custom validator can be defined as follows:

snippet: validator


### Setup Validators

Validators need to be added to the `ValidatorTypeCache`. This should be done once at application startup.

snippet: StartConfig

Generally `ValidatorTypeCache` is scoped per app and can be collocated with `Schema`, `DocumentExecuter` initialization.


### Add to ExecutionOptions

Validation needs to be added to any instance of `ExecutionOptions`.

snippet: UseFluentValidation


### UserContext must be a dictionary

This library needs to be able to pass the list of validators, in the form of `ValidatorTypeCache`, through the graphql context. The only way of achieving this is to use the `ExecutionOptions.UserContext`. To facilitate this, the type passed to `ExecutionOptions.UserContext` has to implement `IDictionary<string, object>`. There are two approaches to achieving this:


#### 1. Have the user context class implement IDictionary

Given a user context class of the following form:

snippet: ContextImplementingDictionary

The `ExecutionOptions.UserContext` can then be set as follows:

snippet: ExecuteQueryWithContextImplementingDictionary


#### 2. Have the user context class exist inside a IDictionary

snippet: ExecuteQueryWithContextInsideDictionary


#### No UserContext

If no instance is passed to `ExecutionOptions.UserContext`:

snippet: NoContext

Then the `UseFluentValidation` method will instantiate it to a new `Dictionary<string, object>`.



### Validate when reading arguments

When reading arguments use `GetValidatedArgument` instead of `GetArgument`:

snippet: GetValidatedArgument


## Icon

<a href="https://thenounproject.com/term/shield/1893182/" target="_blank">Shield</a> designed by [Maxim Kulikov](https://thenounproject.com/maxim221/) from [The Noun Project](https://thenounproject.com)
