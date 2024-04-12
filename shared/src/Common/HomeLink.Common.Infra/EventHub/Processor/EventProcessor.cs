namespace HomeLink.Common.Infra.EventHub.Processor;

public record EventProcessor(
    string EventHubHost,
    string EventHubName,
    string ConsumerGroupName,
    string StorageAccountEndpoint,
    string StorageAccountCollectionName);