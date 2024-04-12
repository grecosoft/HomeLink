using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Management.Domain.Plugin;

public class DomainPlugin : PluginBase
{
    public override string PluginId => "0D88F582-49F4-4934-BE30-D8D2BB33FD92";
    public override PluginTypes PluginType => PluginTypes.AppPlugin;
    public override string Name => "Domain Model Component";
        
    public DomainPlugin()
    {
        Description = "Plugin component containing the Microservice's domain model.";
    }
}