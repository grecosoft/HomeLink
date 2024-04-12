using System.Text;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using NetFusion.Messaging.Types.Contracts;

namespace HomeLink.Common.Infra.EventHub.Producer;

public class EventProducerService(IProducerEventHubModule producerModule) : IEventProducerService
{
    private readonly IProducerEventHubModule _producerModule = producerModule;

    public async Task SendEvents<T>(IEnumerable<T> domainEvents, string eventHubName,
        CancellationToken cancellationToken) where T : IDomainEvent
    {
        var registration = _producerModule.GetRegistration(eventHubName);
        
        using var eventBatch = await registration.Client.CreateBatchAsync(cancellationToken);

        foreach(var domainEvent in domainEvents)
        {
            if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(domainEvent)))))
            {
                throw new Exception($"Event is too large for the batch and cannot be sent.");
            }
        }

        await registration.Client.SendAsync(eventBatch, cancellationToken);
    }
}