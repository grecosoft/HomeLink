using Azure.DigitalTwins.Core;
using HomeLink.Management.App.Repositories;
using HomeLink.Management.Domain.Commands;
using HomeLink.Management.Domain.Entities;
using HomeLink.Management.Domain.Services;

namespace HomeLink.Management.App.Handlers;

public class RoomConfigurationHandler(
    ITwinSchemaRepository twinModelRepo,
    ITwinRepository twinRepo,
    IDeviceDataService modelService,
    IEnumerable<IDeviceProvider> deviceTwinProvider) : IMessageConsumer
{
    private readonly IDeviceDataService _deviceDataService = modelService;
    private readonly ITwinRepository _twinRepo = twinRepo;
    private readonly ITwinSchemaRepository _twinSchemaRepo = twinModelRepo;
    
    private readonly IEnumerable<IDeviceProvider> _deviceTwinProviders = deviceTwinProvider;

    private IDeviceProvider GetDeviceTwinProvider(ConfigureDeviceCommand command)
    {
        return _deviceTwinProviders.First(p => p.ProviderType == command.ProviderType);
    }
    
    [InProcessHandler]
    public async Task<EntityResult> ConfigureDevice(ConfigureDeviceCommand command)
    {
        var deviceDataTwinId = Guid.NewGuid().ToString();
        
        // Create twin based on the device's associated telemetry data model, and associate it with the containing room.
        // The instance of this twin will have its properties updated with the current state of the device's telemetry.
        var dataTwin = await _deviceDataService.CreateDeviceDataTwin(
            command.DeviceModelId,
            deviceDataTwinId, 
            command.SourcePath);
        
        var dataTwinResp = await _twinRepo.WriteTwin(dataTwin);
        var relationResp = await _twinRepo.CreateRelationship(command.RoomId!, dataTwin.Id, "has_device_data");

        // Create twin based on the device's model and initialize any component properties with
        // the device's current reported properties and relate twin to corresponding data twin.  
        await CreateRelatedDevice(command, dataTwin);
        
        return EntityResult.For(dataTwinResp.Value.Id, dataTwinResp.Value.ETag.ToString(), 
            result => result.RelationshipId = relationResp.Value.Id);
    }
    
    private async Task CreateRelatedDevice(ConfigureDeviceCommand command, BasicDigitalTwin dataTwin)
    {
        var deviceModel = await _twinSchemaRepo.ReadModelSchemaAsync(command.DeviceModelId);
        var twinProvider = GetDeviceTwinProvider(command);

        var deviceDigitalTwin = await twinProvider.GetDeviceTwinAsync(deviceModel, command.DeviceId);
        deviceDigitalTwin.Metadata = new DigitalTwinMetadata { ModelId = command.DeviceModelId };
        
        await _twinRepo.WriteTwin(deviceDigitalTwin);
        await _twinRepo.CreateRelationship(dataTwin.Id, deviceDigitalTwin.Id, "generated_by");
    }
}