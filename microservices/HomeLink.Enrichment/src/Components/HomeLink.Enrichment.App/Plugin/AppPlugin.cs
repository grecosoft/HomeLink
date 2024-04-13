using NetFusion.Core.Bootstrap.Plugins;
using HomeLink.Enrichment.App.Plugin.Modules;

namespace HomeLink.Enrichment.App.Plugin;

public class AppPlugin : PluginBase
{
    public override string PluginId => "46BCA923-DA94-48CE-8DCA-33F188C023B0";
    public override PluginTypes PluginType => PluginTypes.AppPlugin;
    public override string Name => "Application Services Component";

    public AppPlugin()
    {
        AddModule<EventEnrichmentModule>();

        Description = "Plugin component containing the Microservice's application services.";
    }   
}