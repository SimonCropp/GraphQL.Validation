using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
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
    public Task With_validator_invalid()
    {
        var message = new MessageWithValidator();
        return Assert.ThrowsAsync<ValidationException>(() => Send(message));
    }

    static async Task Send(object message, [CallerMemberName] string key = null)
    {
        var configuration = new EndpointConfiguration("DataAnnotationsOutgoing" + key);
        configuration.UseTransport<LearningTransport>();
        configuration.PurgeOnStartup(true);
        configuration.DisableFeature<TimeoutManager>();

        configuration.UseDataAnnotationsValidation(incoming: false);

        var endpoint = await Endpoint.Start(configuration);
        await endpoint.SendLocal(message);
    }
}