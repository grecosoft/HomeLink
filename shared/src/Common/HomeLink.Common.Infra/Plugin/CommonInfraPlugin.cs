using HomeLink.Common.Infra.EventHub.Processor;
using HomeLink.Common.Infra.EventHub.Producer;
using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Common.Infra.Plugin;

public class CommonInfraPlugin : PluginBase
{
    public override string PluginId => "0DED5AD5-D9FD-44A5-BDC5-A73EDB7656A2";
    public override PluginTypes PluginType => PluginTypes.CorePlugin;
    public override string Name => "HomeLink Common Service Components";

    public CommonInfraPlugin() {
        AddModule<ProcessorEventHubModule>();
        AddModule<ProducerEventHubModule>();
        Description = "Plugin component containing the application infrastructure.";
    }
}