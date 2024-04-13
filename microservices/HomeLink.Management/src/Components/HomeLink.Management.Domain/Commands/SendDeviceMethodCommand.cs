using System.Text.Json.Nodes;
using HomeLink.Management.Domain.Entities;
using NetFusion.Messaging.Types;

namespace HomeLink.Management.Domain.Commands;

public class SendDeviceMethodCommand(string methodName,
    JsonNode payload,
    ProviderTypes providerType,
    int timeoutInSeconds = 10): Command<string>
{
    public string MethodName { get; } = methodName;
    public JsonNode Payload { get; } = payload;
    public ProviderTypes ProviderType { get; set; } = providerType;
    public int TimeoutInSeconds { get; } = timeoutInSeconds;
    
    public string DeviceId { get; set; } = null!;
}