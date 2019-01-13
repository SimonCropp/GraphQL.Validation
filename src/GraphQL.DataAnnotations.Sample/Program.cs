using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        var configuration = new EndpointConfiguration("DataAnnotationsValidationSample");
        configuration.UsePersistence<LearningPersistence>();
        configuration.UseTransport<LearningTransport>();
        configuration.UseDataAnnotationsValidation(outgoing:false);

        var endpoint = await Endpoint.Start(configuration);

        await endpoint.SendLocal(new MyMessage{Content = "sd"});
        await endpoint.SendLocal(new MyMessage());

        Console.WriteLine("Press any key to stop program");
        Console.Read();
        await endpoint.Stop();
    }
}