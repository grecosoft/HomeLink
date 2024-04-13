using System;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using HomeLink.Common.Infra.EventHub;
using HomeLink.Common.Infra.EventHub.Processor;
using HomeLink.Common.Infra.EventHub.Producer;
using HomeLink.Enrichment.App.Services;
using HomeLink.Enrichment.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using NetFusion.Core.Bootstrap.Plugins;
using NetFusion.Core.Settings;

namespace HomeLink.Enrichment.App.Plugin.Modules;

public class EventEnrichmentModule : PluginModule
{
    private IProcessorEventHubModule ProcessorModule { get; set; } = null!;
    private IProducerEventHubModule ProducerModule { get; set; } = null!;
    
    private EventHubConfig? _eventHubConfig;

    public override void Initialize()
    {
        _eventHubConfig = Context.Configuration.GetSettings<EventHubConfig>();
    }
    
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IEventNormalizationService, EventNormalizationService>();
    }

    protected override Task OnStartModuleAsync(IServiceProvider services)
    {
        if (_eventHubConfig is null)
        {
            throw new NullReferenceException("Event Hub Configuration not loaded.");
        }
        
        var normalizationService = services.GetRequiredService<IEventNormalizationService>();
        var processor = new EventProcessor(
            _eventHubConfig.EventHubHost,
            _eventHubConfig.DeviceDataHubName,
            _eventHubConfig.DeviceDataConsumerGroupName,
            _eventHubConfig.StorageAccountEndpoint,
            _eventHubConfig.EventHubStorageAccountCollectionName);
        
        ProcessorModule.SubscribeToEventHub<JsonObject>(processor, normalizationService.NormalizeEvent);

        var producer = new EventProducer(_eventHubConfig.EventHubHost, _eventHubConfig.DeviceDataEnrichedHubName);
        ProducerModule.ProduceToEventHub(producer);
        
        return base.OnRunModuleAsync(services);
    }
}