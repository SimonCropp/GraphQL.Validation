# <img src="/src/icon.png" height="30px"> GraphQL.Validation

[![Build status](https://ci.appveyor.com/api/projects/status/wvk8wm3n227b2b3q/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/graphql-validation)
[![NuGet Status](https://img.shields.io/nuget/v/GraphQL.FluentValidation.svg)](https://www.nuget.org/packages/GraphQL.FluentValidation/)

Add [FluentValidation](https://fluentvalidation.net/) support to [GraphQL.net](https://github.com/graphql-dotnet/graphql-dotnet)

toc


## NuGet

https://nuget.org/packages/GraphQL.FluentValidation/


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


### Trigger validation

To trigger the validation, when reading arguments use `GetValidatedArgument` instead of `GetArgument`:

snippet: GetValidatedArgument


## Testing

### Integration

A full end-to-en test can be run against the GraphQl controller:

snippet: GraphQlControllerTests


### Unit

Unit tests can be run a specific field of a query:

snippet: QueryTests


## Release Notes

See [closed milestones](../../milestones?state=closed).


## Icon

[Shield](https://thenounproject.com/term/shield/1893182/) designed by [Maxim Kulikov](https://thenounproject.com/maxim221/) from [The Noun Project](https://thenounproject.com)