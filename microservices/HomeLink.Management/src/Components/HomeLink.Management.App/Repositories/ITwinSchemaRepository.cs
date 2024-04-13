using Azure.DigitalTwins.Core;
using HomeLink.Management.Domain.Entities.Schema;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.App.Repositories;

public interface ITwinSchemaRepository
{
    Task<IReadOnlyDictionary<Dtmi, DTEntityInfo>> ReadModelSchemaAsync(string modelId);
    Task<Response<DigitalTwinsModelData[]>> WriteModelSchemaAsync(TwinSchemaModel model);
}