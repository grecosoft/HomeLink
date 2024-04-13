using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using NetFusion.Messaging.Types;

namespace HomeLink.Management.Domain.Events;

public class DeviceTelemetryDomainEvent(
    string deviceId,
    JsonObject telemetry) : DomainEvent
{
    [JsonPropertyName("deviceId")]
    public string DeviceId { get; } = deviceId;
    
    [JsonPropertyName("telemetry")]
    public JsonObject Telemetry { get; } = telemetry;
}