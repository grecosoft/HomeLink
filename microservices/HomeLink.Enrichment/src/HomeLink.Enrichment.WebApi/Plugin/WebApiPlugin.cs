using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Enrichment.WebApi.Plugin;

public class WebApiPlugin : PluginBase
{
    public const string HostId = "9306BCE9-D04A-4153-949E-7BBE84BA4B93";
    public const string HostName = "homelink-enrichment";

    public override PluginTypes PluginType => PluginTypes.HostPlugin;
    public override string PluginId => HostId;
    public override string Name => HostName;
        
    public WebApiPlugin()
    {
        Description = "WebApi host exposing REST/HAL based Web API.";
    }
}