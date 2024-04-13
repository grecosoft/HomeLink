using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace HomeLink.Enrichment.Domain.Services;

public interface IEventNormalizationService
{
    Task NormalizeEvent(JsonObject eventData, IDictionary<string, object> properties, CancellationToken cancellationToken);
}