using NetFusion.Core.Bootstrap.Plugins;
using HomeLink.Common.Infra.Plugin.Modules;

namespace HomeLink.Common.Infra.Plugin;

public class CommonInfraPlugin : PluginBase
{
    public override string PluginId => "0DED5AD5-D9FD-44A5-BDC5-A73EDB7656A2";
    public override PluginTypes PluginType => PluginTypes.CorePlugin;
    public override string Name => "HomeLink Common Service Components";

    public CommonInfraPlugin() {
        AddModule<ServiceModule>();

        Description = "Plugin component containing service infrastructure components.";
    }
}