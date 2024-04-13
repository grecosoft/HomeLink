using System.Threading.Tasks;
using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Management.Infra.Plugin;

public interface IIotCentralModule : IPluginModuleService
{
    Task<string> GetAuthTokenAsync(bool refresh = false);
}