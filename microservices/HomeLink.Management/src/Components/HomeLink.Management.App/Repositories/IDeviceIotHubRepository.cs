using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.App.Repositories;

public interface IDeviceIotHubRepository
{
    Task<Twin> GetDeviceTwin(string deviceId);
    
    IDictionary<string, object> GetReportedComponentPropertyValues(Twin deviceTwin,
        IReadOnlyDictionary<Dtmi, DTEntityInfo> deviceSchema,
        string componentName);
}