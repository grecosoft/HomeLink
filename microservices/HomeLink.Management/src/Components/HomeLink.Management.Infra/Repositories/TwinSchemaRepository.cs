using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.DigitalTwins.Core;
using HomeLink.Management.App.Repositories;
using HomeLink.Management.Domain.Entities.Schema;
using Microsoft.Azure.DigitalTwins.Parser;

namespace HomeLink.Management.Infra.Repositories;

public class TwinSchemaRepository(DigitalTwinsClient client) : ITwinSchemaRepository
{
    private readonly DigitalTwinsClient _client = client;

    public async Task<IReadOnlyDictionary<Dtmi, DTEntityInfo>> ReadModelSchemaAsync(string modelId)
    {
        var modelParser = new ModelParser { DtmiResolver = ResolveModels };
        var model = await _client.GetModelAsync(modelId);
        return await modelParser.ParseAsync(new[] { model.Value.DtdlModel });

        async Task<IEnumerable<string>> ResolveModels(IReadOnlyCollection<Dtmi> relatedModels)
        {
            var definitions = new List<string>();
        
            foreach (var relatedModelId in relatedModels)
            {
                var def = await _client.GetModelAsync(relatedModelId.ToString());
                definitions.Add(def.Value.DtdlModel);
            }

            return definitions;
        }
    }

    public Task<Response<DigitalTwinsModelData[]>> WriteModelSchemaAsync(TwinSchemaModel model)
    {
        var dtdlModel = JsonSerializer.Serialize(model);
        return _client.CreateModelsAsync(new[] { dtdlModel });
    }
}