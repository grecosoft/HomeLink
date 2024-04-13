using System;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using HomeLink.Management.App.Plugin.Configs;
using Microsoft.Extensions.DependencyInjection;
using NetFusion.Core.Bootstrap.Plugins;
using NetFusion.Core.Settings;

namespace HomeLink.Management.Infra.Plugin.Modules;

public class DigitalTwinModule : PluginModule
{
    public override void RegisterServices(IServiceCollection services)
    {
        var configs = Context.Configuration.GetSettings<DigitalTwinServiceConfig>();
        var credentials = new DefaultAzureCredential();

        var client = new DigitalTwinsClient(new Uri($"https://{configs.Host}"), credentials);
        services.AddSingleton(client);
    }
}