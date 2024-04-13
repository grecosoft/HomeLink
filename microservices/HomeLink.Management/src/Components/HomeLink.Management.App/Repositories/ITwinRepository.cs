using System.Diagnostics.CodeAnalysis;
using Azure.DigitalTwins.Core;

namespace HomeLink.Management.App.Repositories;

public interface ITwinRepository
{
    
    Task<Response<BasicDigitalTwin>> WriteTwin(BasicDigitalTwin twin);
    
    Task<Response<BasicRelationship>> CreateRelationship(string sourceTwinId, string targetTwinId,
        string relationName,
        IDictionary<string, object>? properties = null);
    
    Task<Response> UpdateRelationship(string sourceTwinId, string relationshipId,
        IDictionary<string, object> properties);
    
    bool TryGetDeviceDataTwin(string deviceId, [NotNullWhen(true)] out BasicDigitalTwin? twin);
    Task PatchTwin(string twinId, JsonPatchDocument patch);
}