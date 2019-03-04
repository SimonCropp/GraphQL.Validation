# <img src="https://raw.githubusercontent.com/SimonCropp/GraphQL.Validation/master/src/icon.png" height="40px"> GraphQL.FluentValidation

Add [FluentValidation](https://fluentvalidation.net/) support to [GraphQL.net](https://github.com/graphql-dotnet/graphql-dotnet)


## NuGet [![NuGet Status](https://badgen.net/nuget/v/GraphQL.FluentValidation/pre)](https://www.nuget.org/packages/GraphQL.FluentValidation/)

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

Generally ValidatorTypeCache is scoped per app and can be collocated with `Schema`, `DocumentExecuter` initialization.


### Add to ExecutionOptions

Validation needs to be added to any instance of `ExecutionOptions`.

snippet: UseFluentValidation


### Validate when reading arguments

When reading arguments use `GetValidatedArgument` instead of `GetArgument`:

snippet: GetValidatedArgument


## Icon

<a href="https://thenounproject.com/term/shield/1893182/" target="_blank">Shield</a> designed by [Maxim Kulikov](https://thenounproject.com/maxim221/) from [The Noun Project](https://thenounproject.com)
