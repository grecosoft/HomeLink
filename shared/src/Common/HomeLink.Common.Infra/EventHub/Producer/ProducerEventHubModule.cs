using Azure.Identity;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.DependencyInjection;
using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Common.Infra.EventHub.Producer;

public class ProducerEventHubModule : PluginModule,
    IProducerEventHubModule
{
    private readonly Dictionary<string, ProducerRegistration> _producers = new();

    public override void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IEventProducerService, EventProducerService>();
    }
    
    public void ProduceToEventHub(EventProducer producer)
    {
        var producerClient = new EventHubProducerClient(producer.EventHubHost, producer.EventHubName,
            new DefaultAzureCredential());
        
        _producers[producer.EventHubName] = new ProducerRegistration(producerClient);
    }

    public ProducerRegistration GetRegistration(string eventHubName)
    {
        if (! _producers.TryGetValue(eventHubName, out var registration))
        {
            throw new InvalidOperationException($"Producer not found for event hub name: {eventHubName}");
        }

        return registration;
    }
}