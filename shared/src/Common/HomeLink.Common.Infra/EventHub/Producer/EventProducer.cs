namespace HomeLink.Common.Infra.EventHub.Producer;

public record EventProducer(
    string EventHubHost,
    string EventHubName);