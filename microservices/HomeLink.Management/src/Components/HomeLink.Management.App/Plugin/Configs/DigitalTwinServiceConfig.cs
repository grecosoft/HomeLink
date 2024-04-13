using NetFusion.Core.Settings;

namespace HomeLink.Management.App.Plugin.Configs;

[ConfigurationSection("Azure:Services:DigitalTwins")]
public class DigitalTwinServiceConfig : IAppSettings
{
    public required string Host { get; set; } 
    public required string IotHubConn { get; set; }
}

public class Settings 
{
    public required string Make { get; set; }
    public required string Model { get; set; }
    public required string LinkedSecret { get; set; }
}