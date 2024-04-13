using HomeLink.Management.App;
using HomeLink.Management.App.Plugin.Configs;
using HomeLink.Management.Infra.Providers;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.DependencyInjection;
using NetFusion.Core.Bootstrap.Plugins;
using NetFusion.Core.Settings;

namespace HomeLink.Management.Infra.Plugin.Modules;

public class IotHubModule : PluginModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        var configs = Context.Configuration.GetSettings<DigitalTwinServiceConfig>();

        var registryMgr = RegistryManager.CreateFromConnectionString(configs.IotHubConn);
        services.AddSingleton(registryMgr);
        
        var serviceClient = ServiceClient.CreateFromConnectionString(configs.IotHubConn);
        services.AddSingleton(serviceClient);

        services.AddScoped<IDeviceProvider, IotHubDeviceProvider>();
    }
}