using System.Text.Json.Nodes;
using System.Threading;
using Azure.DigitalTwins.Core;
using HomeLink.Management.Domain.Entities;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.App;

public interface IDeviceProvider
{
    ProviderTypes ProviderType { get; }

    Task<BasicDigitalTwin> GetDeviceTwinAsync(IReadOnlyDictionary<Dtmi, DTEntityInfo> deviceModel,
        string deviceId,
        CancellationToken cancellationToken = default);

    Task<string> SendCommand(string deviceId, string name, JsonNode payload,
        int timeoutInSeconds);
}