using System.Text.Json.Serialization;

namespace HomeLink.Management.Domain.Entities.Schema;

public class TwinSchemaProperty(string name, string schema)
{
    [JsonPropertyName("name")]
    public string Name { get; } = name;

    [JsonPropertyName("@type")] public string Type => "Property";

    [JsonPropertyName("schema")]
    public string Schema { get; } = schema;
}