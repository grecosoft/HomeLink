using HomeLink.Management.Domain.Commands;

namespace HomeLink.Management.App.Handlers;

public class DeviceMethodHandler(IEnumerable<IDeviceProvider> deviceProviders) : IMessageConsumer
{
    [InProcessHandler]
    public Task<string> SendCommand(SendDeviceMethodCommand command)
    {
        var deviceProvider = GetDeviceTwinProvider(command);
        return deviceProvider.SendCommand(command.DeviceId, command.MethodName, command.Payload,
            command.TimeoutInSeconds);
    }

    private IDeviceProvider GetDeviceTwinProvider(SendDeviceMethodCommand command)
    {
        return deviceProviders.First(p => p.ProviderType == command.ProviderType);
    }
}