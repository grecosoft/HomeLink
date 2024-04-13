namespace HomeLink.Management.WebApi.Models;

public class DeviceIdentification
{
    public required string DeviceName { get; set; } 
    public required string DeviceModelId { get; set; }
    public string? SourcePath { get; set; }
}