using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Management.WebApi.Plugin;

public class WebApiPlugin : PluginBase
{
    public const string HostId = "945E8564-3C7D-47DB-ADA6-3931F20C4228";
    public const string HostName = "homelink-management";

    public override PluginTypes PluginType => PluginTypes.HostPlugin;
    public override string PluginId => HostId;
    public override string Name => HostName;
        
    public WebApiPlugin()
    {
        Description = "WebApi host exposing REST/HAL based Web API.";
    }
}