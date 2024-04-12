using NetFusion.Core.Bootstrap.Plugins;
using HomeLink.Enrichment.Infra.Plugin.Modules;

namespace HomeLink.Enrichment.Infra.Plugin;

public class InfraPlugin : PluginBase
{
    public override string PluginId => "7AD1E254-9296-465A-8C46-33467FB0F0A2";
    public override PluginTypes PluginType => PluginTypes.AppPlugin;
    public override string Name => "Infrastructure Application Component";

    public InfraPlugin() {
        AddModule<RepositoryModule>();

        Description = "Plugin component containing the application infrastructure.";
    }
}