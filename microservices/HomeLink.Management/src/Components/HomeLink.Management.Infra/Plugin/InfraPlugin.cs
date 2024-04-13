using NetFusion.Core.Bootstrap.Plugins;
using HomeLink.Management.Infra.Plugin.Modules;

namespace HomeLink.Management.Infra.Plugin;

public class InfraPlugin : PluginBase
{
    public override string PluginId => "74DD6A00-FD99-4C21-84D1-A22A6057D53B";
    public override PluginTypes PluginType => PluginTypes.AppPlugin;
    public override string Name => "Infrastructure Application Component";
    
    public InfraPlugin() {
        AddModule<DigitalTwinModule>();
        AddModule<RepositoryModule>();
        AddModule<IotHubModule>();
        AddModule<IotCentralModule>();

        Description = "Plugin component containing the application infrastructure.";
    }
}