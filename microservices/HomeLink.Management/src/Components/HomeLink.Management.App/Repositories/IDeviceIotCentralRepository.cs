using System.Text.Json.Nodes;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.App.Repositories;

public interface IDeviceIotCentralRepository
{
    Task<JsonObject> GetDeviceProperties(string deviceId);

    IDictionary<string, object> GetDeviceComponentPropertyValues(JsonObject deviceProperties,
        IReadOnlyDictionary<Dtmi, DTEntityInfo> deviceSchema,
        string componentName);
}