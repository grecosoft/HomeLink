using NetFusion.Core.Bootstrap.Catalog;
using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Management.App.Plugin.Modules;

public class ServiceModule : PluginModule
{
    public override void ScanForServices(ITypeCatalog catalog)
    {
        catalog.AsImplementedInterface("Service", ServiceLifetime.Scoped);
    }
}