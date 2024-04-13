using System;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using HomeLink.Management.App;
using HomeLink.Management.Infra.Providers;
using Microsoft.Extensions.DependencyInjection;
using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Management.Infra.Plugin.Modules;

public class IotCentralModule : PluginModule,
    IIotCentralModule
{
    private string? _authToken;
    
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddHttpClient("IotCentral", 
            httpClient => httpClient.BaseAddress = new Uri("https://home-central-3dudyhs.azureiotcentral.com/"));

        services.AddScoped<IDeviceProvider, IotCentralDeviceProvider>();
    }

    public async Task<string> GetAuthTokenAsync(bool refresh = false)
    {
        if (_authToken is null || refresh)
        {
            using var httpClient = new HttpClient();
            var cred = new DefaultAzureCredential();
            
            _authToken = (await cred.GetTokenAsync(
                    new TokenRequestContext(["https://apps.azureiotcentral.com/.default"]))).Token;
        }

        return _authToken;
    }
}