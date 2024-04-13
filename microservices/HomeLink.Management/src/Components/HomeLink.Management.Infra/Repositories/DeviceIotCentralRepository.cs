using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using HomeLink.Management.App.Extensions;
using HomeLink.Management.App.Repositories;
using HomeLink.Management.Infra.Plugin;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.Infra.Repositories;

public class DeviceIotCentralRepository(
    IIotCentralModule iotCentralModule, 
    IHttpClientFactory httpClientFactory) : IDeviceIotCentralRepository
{
    private const string ApiVersion = "api-version=2022-07-31";
    
    private readonly IIotCentralModule _iotCentralModule = iotCentralModule;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<JsonObject> GetDeviceProperties(string deviceId)
    {
        var client = _httpClientFactory.CreateClient("IotCentral");
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", await _iotCentralModule.GetAuthTokenAsync());

        using var response = await client.GetAsync($"/api/devices/{deviceId}/properties?{ApiVersion}");
        response.EnsureSuccessStatusCode();

        return (
                await JsonSerializer.DeserializeAsync<JsonObject>(await response.Content.ReadAsStreamAsync())
            ) ?? new JsonObject();
    }

    public IDictionary<string, object> GetDeviceComponentPropertyValues(JsonObject deviceProperties,
        IReadOnlyDictionary<Dtmi, DTEntityInfo> deviceSchema,
        string componentName)
    {
        if (! deviceProperties.TryGetPropertyValue(componentName, out var componentProp))
        {
            return new Dictionary<string, object>();
        }
        
        var reportedTwinPropValues = componentProp!.AsObject().Select(propertyValue => 
            new TwinValueProperty(propertyValue.Key, propertyValue.Value?.ToString() ?? string.Empty)).ToList();

        var componentProps = deviceSchema.GetComponentPrimitiveProperties(componentName);
        return componentProps.ConvertModelPropertyValues(reportedTwinPropValues);
    }
}
