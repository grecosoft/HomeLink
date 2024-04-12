using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Enrichment.Domain.Plugin;

public class DomainPlugin : PluginBase
{
    public override string PluginId => "0F2D3C41-7612-44B4-8799-9220F53D936F";
    public override PluginTypes PluginType => PluginTypes.AppPlugin;
    public override string Name => "Domain Model Component";
        
    public DomainPlugin()
    {
        Description = "Plugin component containing the Microservice's domain model.";
    }
}