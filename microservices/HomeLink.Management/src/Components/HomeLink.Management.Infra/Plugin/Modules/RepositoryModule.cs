using Microsoft.Extensions.DependencyInjection;
using NetFusion.Core.Bootstrap.Catalog;
using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Management.Infra.Plugin.Modules;

public class RepositoryModule : PluginModule
{
    public override void ScanForServices(ITypeCatalog catalog)
    {
        catalog.AsImplementedInterface("Repository", ServiceLifetime.Scoped);
    }
}