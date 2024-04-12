using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;

namespace HomeLink.Common.Infra.EventHub.Processor;

public class ProcessorRegistration<TEvent>(
    EventProcessorClient processorClient,
    Func<TEvent, IDictionary<string, object>, CancellationToken, Task> handler) : IEventHubRegistration
{
    public EventProcessorClient ProcessorClient { get; } = processorClient;
    private readonly Func<TEvent, IDictionary<string, object>, CancellationToken, Task> _handler = handler;

    public Task RegisterHandler()
    {
        ProcessorClient.ProcessEventAsync += ProcessorClientOnProcessEventAsync;
        ProcessorClient.ProcessErrorAsync += ProcessorClientOnProcessErrorAsync;
        
        return ProcessorClient.StartProcessingAsync();
    }

    public Task UnRegisterHandler() => ProcessorClient.StopProcessingAsync();
    
    private async Task ProcessorClientOnProcessEventAsync(ProcessEventArgs arg)
    {
        var eventHubEvent = JsonSerializer.Deserialize<TEvent>(arg.Data.EventBody);
        if (eventHubEvent is null)
        {
            throw new InvalidOperationException(
                $"The received Event Hub event could not be deserialized into: {typeof(TEvent)}");
        }

        var eventProperties = GetAllEventProperties(arg.Data);
        
        await _handler.Invoke(eventHubEvent, eventProperties, arg.CancellationToken);
        await arg.UpdateCheckpointAsync();
    }

    private static Dictionary<string, object> GetAllEventProperties(EventData eventData)
    {
        var allProperties = new Dictionary<string, object>(eventData.Properties);
        foreach (var systemProp in eventData.SystemProperties)
        {
            if (! allProperties.ContainsKey(systemProp.Key))
            {
                allProperties[systemProp.Key] = systemProp.Value;
            }
        }

        return allProperties;
    }
    
    private Task ProcessorClientOnProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        Console.WriteLine(arg.Exception);
        return Task.CompletedTask;
    }
}