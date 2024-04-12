using NetFusion.Messaging.Types.Contracts;

namespace HomeLink.Common.Infra.EventHub;

public interface IEventProducerService
{
    Task SendEvents<T>(IEnumerable<T> domainEvents, string eventHubName,
        CancellationToken cancellationToken) where T : IDomainEvent;
}