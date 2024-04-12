using Azure.Messaging.EventHubs;

namespace HomeLink.Common.Infra.EventHub;

public interface IEventHubRegistration
{
    EventProcessorClient ProcessorClient { get; }
    Task RegisterHandler();
    Task UnRegisterHandler();
}