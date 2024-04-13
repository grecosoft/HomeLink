using System.Threading;
using HomeLink.Management.App.Repositories;
using HomeLink.Management.Domain.Events;
using HomeLink.Management.Domain.Services;

namespace HomeLink.Management.App.Services;

public class DeviceTwinService(ITwinRepository twinRepository) : IDeviceTwinService
{
    private readonly ITwinRepository _twinRepository = twinRepository;
    
    public async Task UpdateTwin(DeviceTelemetryDomainEvent telemetryDomainEvent, 
        IDictionary<string, object> properties,
        CancellationToken cancellationToken)
    {
        var patch = new JsonPatchDocument();
        
        if (!_twinRepository.TryGetDeviceDataTwin(telemetryDomainEvent.DeviceId, out var dataTwin))
        {
            throw new InvalidOperationException(
                $"The data-twin associated with device {telemetryDomainEvent.DeviceId} not found.");
        }
        
        foreach(var (property, value) in telemetryDomainEvent.Telemetry)
        {
            patch.AppendAdd($"/{property}", value);
        }

        await _twinRepository.PatchTwin(dataTwin.Id, patch);
    }
}