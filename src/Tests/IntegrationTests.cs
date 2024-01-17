using GraphQL.FluentValidation;

public class IntegrationTests
{
    static IValidatorCache cache;

    static IntegrationTests()
    {
        cache = new ValidatorInstanceCache(
            type =>
            {
                if (type == typeof(NoValidatorInput))
                {
                    return new NoValidatorInputValidator<string>();
                }

                return null;
            });
        cache.AddValidatorsFromAssemblyContaining<IntegrationTests>();
    }

    [Fact]
    public async Task AsyncValid()
    {
        var query = """
                    {
                      asyncQuery
                        (
                          input: {
                            content: "TheContent"
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task AsyncInvalid()
    {
        var query = """
                    {
                      asyncQuery
                        (
                          input: {
                            content: ""
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task ValidNull()
    {
        var query = """
                    {
                      inputQuery
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task NoValidatorValid()
    {
        var query = """
                    {
                      noValidatorQuery
                        (
                          input: {
                            content: "TheContent"
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task NoValidatorInvalid()
    {
        var query = """
                    {
                      noValidatorQuery
                        (
                          input: {
                            content: ""
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task Valid()
    {
        var query = """
                    {
                      inputQuery
                        (
                          input: {
                            content: "TheContent"
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task Invalid()
    {
        var query = """
                    {
                      inputQuery
                        (
                          input: {
                            content: ""
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task ComplexValid()
    {
        var query = """
                    {
                      complexInputQuery
                        (
                          input: {
                            inner: {
                              content: "TheContent"
                            },
                            items: [
                                { id: 1, content: "Some content 1" },
                                { id: 2, content: "Some content 2" }
                            ]
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task ComplexInvalid()
    {
        var query = """
                    {
                      complexInputQuery
                        (
                          input: {
                            inner: {
                              content: ""
                            },
                            items: []
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task DerivedComplexInvalid()
    {
        var query = """
                    {
                      derivedComplexInputQuery
                        (
                          input: {
                            inner: {
                              content: ""
                            },
                            items: []
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task ComplexInvalid2()
    {
        var query = """
                    {
                      complexInputQuery
                        (
                          input: {
                            inner: null,
                            items: null
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task AsyncComplexValid()
    {
        var query = """
                    {
                      asyncComplexInputQuery
                        (
                          input: {
                            inner: {
                              content: "TheContent"
                            },
                            items: [
                                { id: 1, content: "Some content 1" },
                                { id: 2, content: "Some content 2" }
                            ]
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }

    [Fact]
    public async Task AsyncComplexInvalid()
    {
        var query = """
                    {
                      asyncComplexInputQuery
                        (
                          input: {
                            inner: {
                              content: ""
                            },
                            items: null
                          }
                        )
                      {
                        data
                      }
                    }
                    """;
        var result = await QueryExecutor.ExecuteQuery(query, null, cache);
        await VerifyJson(result);
    }
}