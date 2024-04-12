using HomeLink.Common.Infra.EventHub.Processor;
using NetFusion.Core.Bootstrap.Plugins;

namespace HomeLink.Common.Infra.EventHub;

public interface IProcessorEventHubModule : IPluginModuleService
{
    void SubscribeToEventHub<TEvent>(EventProcessor processor,
        Func<TEvent, IDictionary<string, object>, CancellationToken, Task> eventHandler);
}