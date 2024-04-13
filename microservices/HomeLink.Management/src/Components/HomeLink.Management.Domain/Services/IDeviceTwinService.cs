using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeLink.Management.Domain.Events;

namespace HomeLink.Management.Domain.Services;

public interface IDeviceTwinService
{
    Task UpdateTwin(DeviceTelemetryDomainEvent telemetryDomainEvent, IDictionary<string, object> properties, 
        CancellationToken cancellationToken);
}