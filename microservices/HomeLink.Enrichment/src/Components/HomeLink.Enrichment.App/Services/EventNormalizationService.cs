using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using HomeLink.Common.Infra.EventHub;
using HomeLink.Enrichment.Domain.Events;
using HomeLink.Enrichment.Domain.Services;

namespace HomeLink.Enrichment.App.Services;

public class EventNormalizationService(IEventProducerService producerService) : IEventNormalizationService
{
    private const string IotCentralIdentifyingPropName = "iotcentral-message-source";
    private const string IotCentralDeviceIdPropName = "iotcentral-device-id";
    private const string IotHubDeviceIdPropName = "iothub-connection-device-id";

    private readonly IEventProducerService _producerService = producerService;
    
    public async Task NormalizeEvent(JsonObject eventData, IDictionary<string, object> properties,
        CancellationToken cancellationToken)
    {
        var telemetryEvent = IsIotCentralEvent(properties)
            ? BuildIotCentralTelemetryEvent(eventData, properties)
            : BuildIotHubTelemetryEvent(eventData, properties);
        
        Console.WriteLine(JsonSerializer.Serialize(telemetryEvent));

        await _producerService.SendEvents(new[] { telemetryEvent }, "device-data-enriched", cancellationToken);
    }

    private static bool IsIotCentralEvent(IDictionary<string, object> properties) 
        => properties.ContainsKey(IotCentralIdentifyingPropName);

    private static DeviceTelemetryDomainEvent BuildIotCentralTelemetryEvent(JsonObject eventData,
        IDictionary<string, object> properties)
    {
        var id = properties.TryGetValue(IotCentralDeviceIdPropName, out var value) ? value.ToString() : null;
        if (id is null)
        {
            throw new InvalidOperationException($"IoT Central property {IotCentralDeviceIdPropName} not found.");
        }

        if (! eventData.TryGetPropertyValue("telemetry", out JsonNode? node))
        {
            throw new InvalidOperationException("Expected IoT Central telemetry child property not found.");
        }
        
        var telemetry = node.Deserialize<JsonObject>() ??
                     throw new InvalidCastException("Telemetry data could not be deserialized.");

        return new DeviceTelemetryDomainEvent(id, telemetry);
    }
    
    private static DeviceTelemetryDomainEvent BuildIotHubTelemetryEvent(JsonObject eventData,
        IDictionary<string, object> properties)
    {
        var id = properties.TryGetValue(IotHubDeviceIdPropName, out var value) ? value.ToString() : null;
        if (id is null)
        {
            throw new InvalidOperationException($"IoT Hub property {IotHubDeviceIdPropName} not found.");
        }
        
        return new DeviceTelemetryDomainEvent(id, eventData);
    }
}