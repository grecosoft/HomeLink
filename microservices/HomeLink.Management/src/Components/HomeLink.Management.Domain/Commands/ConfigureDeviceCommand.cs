using HomeLink.Management.Domain.Entities;
using NetFusion.Messaging.Types;

namespace HomeLink.Management.Domain.Commands;

/// <summary>
/// Command for configuring a device monitoring a specific room.
/// The ModelId of the device and the room to which it is being added
/// is received.  Based on the ModelId of the device, a twin based on
/// the telemetry of the device is created and will be updated when
/// readings are received. 
/// </summary>
public class ConfigureDeviceCommand(string deviceId, string deviceModelId, ProviderTypes providerType)
    : Command<EntityResult>
{
    /// <summary>
    /// The name of the device
    /// </summary>
    public string DeviceId { get; } = deviceId;
    public string DeviceModelId { get; } = deviceModelId;
    public ProviderTypes ProviderType { get; set; } = providerType;
    
    public string? RoomId { get; set; }
    public string? SourcePath { get; set; }
}