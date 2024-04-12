using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;
using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Common.Infra.EventHub.Processor;

public class ProcessorEventHubModule : PluginModule,
    IProcessorEventHubModule
{
    private readonly Dictionary<EventProcessor, IEventHubRegistration> _eventHubRegistrations = new();
    
    public void SubscribeToEventHub<TEvent>(EventProcessor config,
        Func<TEvent, IDictionary<string, object>, CancellationToken, Task> eventHandler)
    {
        if (_eventHubRegistrations.TryGetValue(config, out var registration))
        {
            
        }

        var containerClient = CreateContainerClient(config);
        var processorClient = CreateProcessorClient(config, containerClient);
        
        _eventHubRegistrations[config] = new ProcessorRegistration<TEvent>(processorClient, eventHandler);
    }

    private static BlobContainerClient CreateContainerClient(EventProcessor processor)
    {
        var credential = new DefaultAzureCredential();
        
        var blobUriBuilder = new BlobUriBuilder(new Uri(processor.StorageAccountEndpoint))
        {
            BlobContainerName = processor.StorageAccountCollectionName
        };
        
        return new BlobContainerClient(
            blobUriBuilder.ToUri(),
            credential);
    }

    private static EventProcessorClient CreateProcessorClient(EventProcessor processor, 
        BlobContainerClient containerClient)
    {
        var credential = new DefaultAzureCredential();
        
        return new EventProcessorClient(
            containerClient,
            processor.ConsumerGroupName,
            processor.EventHubHost,
            processor.EventHubName,
            credential);
    }

    protected override async Task OnRunModuleAsync(IServiceProvider services)
    {
        foreach (var (_, registration) in _eventHubRegistrations)
        {
            await registration.RegisterHandler();
        }
    }

    protected override async Task OnStopModuleAsync(IServiceProvider services)
    {
        foreach (var (_, registration) in _eventHubRegistrations)
        {
            await registration.UnRegisterHandler();
        }
    }
}