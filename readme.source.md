# <img src="https://raw.githubusercontent.com/SimonCropp/GraphQL.EntityFramework/master/src/icon.png" height="40px"> GraphQL.EntityFramework

Add [FluentValidation](https://fluentvalidation.net/) support to [GraphQL.net](https://github.com/graphql-dotnet/graphql-dotnet)


## NuGet [![NuGet Status](http://img.shields.io/nuget/v/GraphQL.FluentValidation.svg?longCache=true&style=flat)](https://www.nuget.org/packages/GraphQL.FluentValidation/)

https://nuget.org/packages/GraphQL.FluentValidation/

    PM> Install-Package GraphQL.FluentValidation


## Usage


### Define validators

Given the following input and graph:

snippet: graph

snippet: input

A custom validator can be defined as follows:

snippet: validator


### Add Validators

Validators need to be added to the `ValidatorTypeCache`. This should be done once at application startup.

snippet: AddValidators


### Add to ExecutionOptions

Validation needs to be added to any instance of `ExecutionOptions`.

snippet: UseFluentValidation


## Icon

<a href="https://thenounproject.com/term/shield/1893182/" target="_blank">Shield</a> designed by [Maxim Kulikov](https://thenounproject.com/maxim221/) from [The Noun Project](https://thenounproject.com)
