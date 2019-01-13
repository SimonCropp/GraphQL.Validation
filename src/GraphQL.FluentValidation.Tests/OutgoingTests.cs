using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentValidation;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.FluentValidation;
using Xunit;

public class OutgoingTests
{
    [Fact]
    public Task With_no_validator()
    {
        var message = new MessageWithNoValidator();
        return Send(message);
    }

    [Fact]
    public Task With_validator_valid()
    {
        var message = new MessageWithValidator
        {
            Content = "content"
        };
        return Send(message);
    }

    [Fact]
    public Task With_uow_validator()
    {
        var message = new MessageWithValidator();
        return Assert.ThrowsAsync<ValidationException>(() => Send(message, ValidatorLifecycle.UnitOfWork));
    }

    [Fact]
    public Task With_validator_invalid()
    {
        var message = new MessageWithValidator();
        return Assert.ThrowsAsync<ValidationException>(() => Send(message));
    }

    [Fact]
    public Task With_async_validator_valid()
    {
        var message = new MessageWithAsyncValidator
        {
            Content = "content"
        };
        return Send(message);
    }

    [Fact]
    public Task With_async_validator_invalid()
    {
        var message = new MessageWithAsyncValidator();
        return Assert.ThrowsAsync<ValidationException>(() => Send(message));
    }

    static async Task Send(object message, ValidatorLifecycle lifecycle = ValidatorLifecycle.Endpoint, [CallerMemberName] string key = null)
    {
        var configuration = new EndpointConfiguration("FluentValidationOutgoing" + key);
        configuration.UseTransport<LearningTransport>();
        configuration.PurgeOnStartup(true);
        configuration.DisableFeature<TimeoutManager>();

        var validation = configuration.UseFluentValidation(lifecycle, incoming: false);
        validation.AddValidatorsFromAssemblyContaining<MessageWithNoValidator>();

        var endpoint = await Endpoint.Start(configuration);
        await endpoint.SendLocal(message);
    }
}