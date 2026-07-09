using FluentValidation;
using GraphQL;
using GraphQL.FluentValidation;

public class ServiceProviderTests
{
    [Fact]
    public void ResolvesServiceFromValidationContext()
    {
        var provider = new StubProvider(new("resolved-from-provider"));
        var cache = BuildCache(new ServiceReadingValidator());

        var exception = Assert.Throws<ValidationException>(
            () => ArgumentValidation.Validate(cache, typeof(TheInput), new TheInput(), new Dictionary<string, object?>(), provider));

        Assert.Contains("resolved-from-provider", exception.Message);
    }

    [Fact]
    public void TryGetServiceProviderIsFalseWhenNotSupplied()
    {
        var cache = BuildCache(new ProviderProbeValidator());

        var exception = Assert.Throws<ValidationException>(
            () => ArgumentValidation.Validate(cache, typeof(TheInput), new TheInput(), new Dictionary<string, object?>(), null));

        Assert.Contains("no-provider", exception.Message);
    }

    static ValidatorInstanceCache BuildCache(IValidator validator) =>
        new(_ => _ == typeof(TheInput) ? validator : null);

    class StubProvider(Dependency dependency) :
        IServiceProvider
    {
        public object? GetService(Type serviceType) =>
            serviceType == typeof(Dependency) ? dependency : null;
    }

    record TheInput;

    record Dependency(string Value);

    class ServiceReadingValidator :
        AbstractValidator<TheInput>
    {
        public ServiceReadingValidator() =>
            RuleFor(_ => _)
                .Custom((_, context) => context.AddFailure(context.GetRequiredService<Dependency>().Value));
    }

    class ProviderProbeValidator :
        AbstractValidator<TheInput>
    {
        public ProviderProbeValidator() =>
            RuleFor(_ => _)
                .Custom((_, context) =>
                {
                    if (context.TryGetServiceProvider(out var provider))
                    {
                        context.AddFailure(
                            provider.GetType().Name);
                    }
                    else
                    {
                        context.AddFailure(
                            "no-provider");
                    }
                });
    }
}
