using System.Collections.Generic;
using System.Threading.Tasks;
using HomeLink.Management.App.Extensions;
using HomeLink.Management.App.Repositories;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.Infra.Repositories;

public class DeviceIotHubRepository(RegistryManager registryManager) : IDeviceIotHubRepository
{
    private readonly RegistryManager _registryManager = registryManager;

    public Task<Twin> GetDeviceTwin(string deviceId) => _registryManager.GetTwinAsync(deviceId);
    
    public IDictionary<string, object> GetReportedComponentPropertyValues(Twin deviceTwin,
        IReadOnlyDictionary<Dtmi, DTEntityInfo> deviceSchema,
        string componentName)
    {
        if (! deviceTwin.Properties.Reported.Contains(componentName))
        {
            return new Dictionary<string, object>();
        }
   
        var reportedTwinPropValues = new List<TwinValueProperty>();
        foreach (var reportedProp in deviceTwin.Properties.Reported[componentName])
        {
            reportedTwinPropValues.Add(new TwinValueProperty(
                reportedProp.Key, 
                reportedProp.Value.ToString() ?? string.Empty));
        }
        
        var componentProps = deviceSchema.GetComponentPrimitiveProperties(componentName);
        return componentProps.ConvertModelPropertyValues(reportedTwinPropValues);
    }
}