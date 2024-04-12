using HomeLink.Common.Infra.EventHub.Producer;
using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Common.Infra.EventHub;

public interface IProducerEventHubModule : IPluginModuleService
{
    void ProduceToEventHub(EventProducer producer);
    ProducerRegistration GetRegistration(string eventHubName);
}