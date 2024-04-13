using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.DigitalTwins.Core;
using HomeLink.Management.App.Repositories;

namespace HomeLink.Management.Infra.Repositories;

public class TwinRepository(DigitalTwinsClient client) : ITwinRepository
{
    private readonly DigitalTwinsClient _client = client;

    public Task<Response<BasicDigitalTwin>> WriteTwin(BasicDigitalTwin twin)
    {
        return _client.CreateOrReplaceDigitalTwinAsync(twin.Id, twin);
    }

    public Task<Response<BasicRelationship>> CreateRelationship(string sourceTwinId, string targetTwinId, 
        string relationName,
        IDictionary<string, object>? properties = null)
    {
        var rel = new BasicRelationship
        {
            Id = Guid.NewGuid().ToString(),
            SourceId = sourceTwinId,
            TargetId = targetTwinId,
            Name = relationName,
            Properties = properties ?? new Dictionary<string, object>()
        };

        return _client.CreateOrReplaceRelationshipAsync(sourceTwinId, rel.Id, rel);
    }

    public Task<Response> UpdateRelationship(string sourceTwinId, string relationshipId, IDictionary<string, object> properties)
    {
        var patch = new JsonPatchDocument();
        foreach (var (prop, value) in properties)
        {
            patch.AppendReplace($"/{prop}", value);
        }

        return _client.UpdateRelationshipAsync(sourceTwinId, relationshipId, patch);
    }
    
    public async Task<BasicRelationship> ReadRelationship(string sourceTwinId, string relationshipId)
    {
        var response = await _client.GetRelationshipAsync<BasicRelationship>(sourceTwinId, relationshipId);
        return response.Value;
    }
    
    public bool TryGetDeviceDataTwin(string deviceId, [NotNullWhen(true)]out BasicDigitalTwin? twin)
    {
        twin = _client.QueryAsync<BasicDigitalTwin>(
                "SELECT DT.$dtId, D.$metadata.$model " +
                "FROM DIGITALTWINS DT JOIN D RELATED DT.generated_by " +
                $"WHERE D.$dtId = '{deviceId}'")
            .ToBlockingEnumerable()
            .FirstOrDefault();

        if (twin == null)
        {
           // _logger.LogError("Room digital twin containing device {DeviceId} not found", deviceReading.DeviceId);
        }

        return twin != null;
    }

    public Task PatchTwin(string twinId, JsonPatchDocument patch)
    {
        return _client.UpdateDigitalTwinAsync(twinId, patch);
    }
}