using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.App.Extensions;

public record TwinModelProperty(string Name, Dtmi TwinType);
public record TwinValueProperty(string Name, string Value);
    
public static class DtdlSchemaExtensions
{
    private static readonly Dictionary<string, Func<string, object?>> PrimitiveTypeConversions = new()
    {
        { "dtdl:instance:Schema:string;2", v => v.ToString() },
        { "dtdl:instance:Schema:double;2", v => double.TryParse(v, out var cv ) ? cv: null },
        { "dtdl:instance:Schema:integer;2", v => int.TryParse(v, out var cv ) ? cv: null },
        { "dtdl:instance:Schema:boolean;2", v => bool.TryParse(v, out var cv ) ? cv: null }
    };
    
    public static string? GetComponentSchema(this IReadOnlyDictionary<Dtmi, DTEntityInfo> schema,
        string componentName) => schema.Select(m => m.Value).OfType<DTComponentInfo>()
                                       .FirstOrDefault(c => c.Name == componentName)?.Schema.Id.AbsolutePath;

    public static IEnumerable<string> GetComponentNames(this IReadOnlyDictionary<Dtmi, DTEntityInfo> schema)
        => schema.Select(m => m.Value).OfType<DTComponentInfo>().Select(c => c.Name);
    
    public static IEnumerable<TwinModelProperty> GetComponentPrimitiveProperties(
        this IReadOnlyDictionary<Dtmi, DTEntityInfo> schema, 
        string componentName)
    {
        var componentSchema = schema.GetComponentSchema(componentName);
        if (componentSchema is null) return Enumerable.Empty<TwinModelProperty>();
        
        return schema
            .Select(m => m.Value).OfType<DTPropertyInfo>()
            .Where(m => m.ChildOf.AbsolutePath == componentSchema)
            .Where(m => PrimitiveTypeConversions.ContainsKey(m.Schema.Id.AbsolutePath))
            .Select(m => new TwinModelProperty(m.Name, m.Schema.Id));
    }
    
    public static IDictionary<string, object> ConvertModelPropertyValues(
        this IEnumerable<TwinModelProperty> modelProperties, 
        IEnumerable<TwinValueProperty> devicePropertyValues) =>
    
        modelProperties.Join(devicePropertyValues,
                outerKey => outerKey.Name,
                innerKey => innerKey.Name,
                (tp, tv) => new {
                    tp.Name,
                    tp.TwinType,
                    tv.Value
                })
            .Select(p => new
            {
                p.Name,
                Value = PrimitiveTypeConversions.TryGetValue(p.TwinType.AbsolutePath, out var cv) ? cv(p.Value) : null
            })
            .Where(pv => pv.Value is not null)
            .ToDictionary(pv => pv.Name, pv => pv.Value!);

}