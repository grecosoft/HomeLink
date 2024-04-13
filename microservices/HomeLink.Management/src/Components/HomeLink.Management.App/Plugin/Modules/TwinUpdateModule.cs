using HomeLink.Common.Infra.EventHub;
using HomeLink.Common.Infra.EventHub.Processor;
using HomeLink.Management.Domain.Events;
using HomeLink.Management.Domain.Services;
using NetFusion.Core.Bootstrap.Plugins;
using NetFusion.Core.Settings;

namespace HomeLink.Management.App.Plugin.Modules;

public class TwinUpdateModule : PluginModule
{
    private IProcessorEventHubModule ProcessorModule { get; set; } = null!;
    private IProducerEventHubModule ProducerModule { get; set; } = null!;
    
    private EventHubConfig? _eventHubConfig;

    public override void Initialize()
    {
        _eventHubConfig = Context.Configuration.GetSettings<EventHubConfig>();
    }

    protected override Task OnStartModuleAsync(IServiceProvider services)
    {
        if (_eventHubConfig is null)
        {
            throw new NullReferenceException("Event Hub Configuration not loaded.");
        }
        
        var deviceTwinService = services.GetRequiredService<IDeviceTwinService>();
        var processor = new EventProcessor(
            _eventHubConfig.EventHubHost,
            _eventHubConfig.DeviceDataEnrichedHubName,
            _eventHubConfig.DeviceDataConsumerGroupName,
            _eventHubConfig.StorageAccountEndpoint,
            _eventHubConfig.EventHubStorageAccountCollectionName);
        
        ProcessorModule.SubscribeToEventHub<DeviceTelemetryDomainEvent>(processor, deviceTwinService.UpdateTwin);
        
        return base.OnRunModuleAsync(services);
    }
}