using Azure.Messaging.EventHubs.Producer;

namespace HomeLink.Common.Infra.EventHub.Producer;

public class ProducerRegistration(EventHubProducerClient producerClient)
{
    public EventHubProducerClient Client { get; } = producerClient;
}