using NetFusion.Core.Bootstrap.Plugins;
using HomeLink.Management.App.Plugin.Modules;

namespace HomeLink.Management.App.Plugin;

public class AppPlugin : PluginBase
{
    public override string PluginId => "C10229EF-3981-4012-BCEA-01A208EE33A8";
    public override PluginTypes PluginType => PluginTypes.AppPlugin;
    public override string Name => "Application Services Component";

    public AppPlugin()
    {
        AddModule<ServiceModule>();
        AddModule<TwinUpdateModule>();

        Description = "Plugin component containing the Microservice's application services.";
    }   
}