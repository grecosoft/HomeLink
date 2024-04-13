using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace HomeLink.Management.Domain.Entities.Schema;

public class TwinSchemaModel(
    string id,
    string[] context,
    IEnumerable<JsonNode> contents,
    params string[] extends)
{
    [JsonPropertyName("@id")]
    public string Id { get; } = id;

    [JsonPropertyName("@type")]
    public string Type => "Interface";
    
    [JsonPropertyName("@context")]
    public string[] Context { get; } = context;

    [JsonPropertyName("displayName")] 
    public string DisplayName { get; init; } = string.Empty;
    
    [JsonPropertyName("extends")]
    public string[] Extends { get; } = extends;

    [JsonPropertyName("contents")]
    public IEnumerable<JsonNode> Contents { get; } = contents;
}