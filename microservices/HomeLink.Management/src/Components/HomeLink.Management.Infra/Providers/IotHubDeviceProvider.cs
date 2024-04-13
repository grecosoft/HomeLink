using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Azure.DigitalTwins.Core;
using HomeLink.Management.App;
using HomeLink.Management.App.Extensions;
using HomeLink.Management.App.Repositories;
using HomeLink.Management.Domain.Entities;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.Infra.Providers;

public class IotHubDeviceProvider(
    IDeviceIotHubRepository deviceRepo,
    ServiceClient serviceClient) : IDeviceProvider
{
    private readonly IDeviceIotHubRepository _deviceIotHubRepo = deviceRepo;
    private readonly ServiceClient _serviceClient = serviceClient;
    
    public ProviderTypes ProviderType => ProviderTypes.IotHub;
    
    public async Task<BasicDigitalTwin> GetDeviceTwinAsync(IReadOnlyDictionary<Dtmi, DTEntityInfo> deviceModel,
        string deviceId,
        CancellationToken cancellationToken = default)
    {
        var deviceTwin = await _deviceIotHubRepo.GetDeviceTwin(deviceId);
        
        return new BasicDigitalTwin
        {
            Id = deviceId,
            Contents = PopulateDeviceComponentValues(deviceModel, deviceTwin)
        };
    }
    
    private Dictionary<string, object> PopulateDeviceComponentValues(
        IReadOnlyDictionary<Dtmi, DTEntityInfo> deviceModel,
        Twin deviceTwin) => deviceModel.GetComponentNames()
        .Select(cn => new
        {
            componentName = cn,
            component = new BasicDigitalTwinComponent
            {
                Contents = _deviceIotHubRepo.GetReportedComponentPropertyValues(
                    deviceTwin,
                    deviceModel, 
                    cn)
            }
        }).ToDictionary(kv => kv.componentName, kv => (object)kv.component);

    
    public async Task<string> SendCommand(string deviceId, string name, JsonNode payload,
        int timeoutInSeconds)
    {
        var method = new CloudToDeviceMethod(name)
        {
            ResponseTimeout = TimeSpan.FromSeconds(timeoutInSeconds)
        };
        
        method.SetPayloadJson(payload.ToJsonString());
        
        var result = await _serviceClient.InvokeDeviceMethodAsync(deviceId, method);
        return result.GetPayloadAsJson();
    }
}