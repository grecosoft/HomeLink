using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Azure.DigitalTwins.Core;
using HomeLink.Management.App;
using HomeLink.Management.App.Extensions;
using HomeLink.Management.App.Repositories;
using HomeLink.Management.Domain.Entities;
using HomeLink.Management.Infra.Plugin;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.Infra.Providers;

public class IotCentralDeviceProvider(
    IIotCentralModule iotCentralModule, 
    IHttpClientFactory httpClientFactory,
    IDeviceIotCentralRepository deviceRepo) : IDeviceProvider
{
    private const string ApiVersion = "api-version=2022-07-31";
    
    private readonly IIotCentralModule _iotCentralModule = iotCentralModule;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IDeviceIotCentralRepository _deviceIotHubRepo = deviceRepo;
    
    public ProviderTypes ProviderType => ProviderTypes.IotCentral;
    
    public async Task<BasicDigitalTwin> GetDeviceTwinAsync(IReadOnlyDictionary<Dtmi, DTEntityInfo> deviceModel,
        string deviceId,
        CancellationToken cancellationToken = default)
    {
        var deviceProperties = await _deviceIotHubRepo.GetDeviceProperties(deviceId);
        
        return new BasicDigitalTwin
        {
            Id = deviceId,
            Contents = PopulateDeviceComponentValues(deviceModel, deviceProperties)
        };
    }

    private Dictionary<string, object> PopulateDeviceComponentValues(
        IReadOnlyDictionary<Dtmi, DTEntityInfo> deviceModel,
        JsonObject deviceProperties) => deviceModel.GetComponentNames()
        .Select(cn => new
        {
            componentName = cn,
            component = new BasicDigitalTwinComponent
            {
                Contents = _deviceIotHubRepo.GetDeviceComponentPropertyValues(
                    deviceProperties,
                    deviceModel, 
                    cn)
            }
        }).ToDictionary(kv => kv.componentName, kv => (object)kv.component);
    
    public async Task<string> SendCommand(string deviceId, string name, JsonNode payload, int timeoutInSeconds)
    {
        var client = _httpClientFactory.CreateClient("IotCentral");
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", await _iotCentralModule.GetAuthTokenAsync());

        using var response = await client.PostAsJsonAsync(
            $"/api/devices/{deviceId}/commands/{name}?{ApiVersion}", new JsonObject
            {
                { "request", payload }
            });

        return string.Empty;
    }
}