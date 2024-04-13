using System.Text.Json.Nodes;
using Azure.DigitalTwins.Core;
using HomeLink.Management.App.Repositories;
using HomeLink.Management.Domain.Entities.Schema;
using HomeLink.Management.Domain.Services;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.App.Services;

/// <summary>
/// Responsible for creating a Digital Twin model based on the telemetry of a given
/// device's model.  When data is received from the device, its associated data twin
/// is updated with the latest device readings.  If the device model is named:
/// dtmi:azurertos:devkit:gsgmxchip;2, then the corresponding data model name is:
/// dtmi:azurertos:devkit:gsgmxchip:data;2
/// </summary>
public class DeviceDataService(ITwinSchemaRepository twinSchemaRepo) : IDeviceDataService
{
    private const string DtdlContextSchema = "dtmi:dtdl:context;2";
    private const string DtdlBaseDeviceSchema = "dtmi:iothome:device:data;1";
    
    private readonly ITwinSchemaRepository _twinSchemaRepo = twinSchemaRepo;

    public async Task<TwinSchemaModel> CreateDeviceDataModel(string deviceModelId)
    {
        var parsedModel = await _twinSchemaRepo.ReadModelSchemaAsync(deviceModelId);
        var deviceDataModelId = BuildDeviceDataModelId(deviceModelId, parsedModel);
        
        var telemetryContent = parsedModel.Values.OfType<DTTelemetryInfo>()
            .Select(BuildTelemetryContent);
        
        return new TwinSchemaModel(
            deviceDataModelId,
            [DtdlContextSchema], 
            telemetryContent,
            DtdlBaseDeviceSchema);
    }
    
    private static string BuildDeviceDataModelId(
        string deviceModelId,
        IReadOnlyDictionary<Dtmi, DTEntityInfo> parsedModel)
    {
        var deviceModelInterface = parsedModel.Values.OfType<DTInterfaceInfo>()
            .First(d => d.Id.AbsoluteUri == deviceModelId);
        
        var deviceModelName = deviceModelInterface.Id.Labels.Last();
        return deviceModelId.Replace(deviceModelName, $"{deviceModelName}:data");
    }
    
    private static JsonNode BuildTelemetryContent(DTTelemetryInfo telemetry)
    {
        if (telemetry.Schema is DTObjectInfo objectInfo)
        {
            return CreateObjectProperty(telemetry.Name, objectInfo.Fields);
        }

        return CreatePropertyContent(
            telemetry.Name,
            telemetry.Schema.Id.Labels.Last());
    }
    
    private static JsonObject CreatePropertyContent(string name, string schema) =>
        new()
        {
            { "name", name },
            { "@type", "Property" },
            { "schema", schema }
        };

    private static JsonObject CreateObjectProperty(string name, IEnumerable<DTFieldInfo> fields) =>
        new()
        {
            { "@type", "Property"},
            { "name", name },
            { "schema", new JsonObject
            {
                { "@type", "Object" },
                { "fields", new JsonArray(fields.Select(f => new JsonObject
                    {
                        { "name", f.Name },
                        { "schema", f.Schema.Id.Labels.Last() }
                    }).Cast<JsonNode>().ToArray())
                }
            }}
        };
    
    public async Task<BasicDigitalTwin> CreateDeviceDataTwin(string deviceModelId, string deviceId, string? sourcePath)
    {
        var parsedModel = await _twinSchemaRepo.ReadModelSchemaAsync(deviceModelId);
        var deviceDataModelId = BuildDeviceDataModelId(deviceModelId, parsedModel);

        return new BasicDigitalTwin
        {
            Id = deviceId,
            Metadata = { ModelId = deviceDataModelId },
            Contents =
            {
                { "sourcePath", sourcePath ?? string.Empty }
            }
        };
    }
}