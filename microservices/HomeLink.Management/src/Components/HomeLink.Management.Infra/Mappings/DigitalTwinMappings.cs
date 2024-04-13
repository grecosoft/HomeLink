using System;
using System.Collections.Generic;
using Azure.DigitalTwins.Core;
using HomeLink.Management.Domain.Entities;
using NetFusion.Services.Mapping;

namespace HomeLink.Management.Infra.Mappings;

public class DigitalTwinMappings : IMappingStrategyFactory
{
    public IEnumerable<IMappingStrategy> GetStrategies()
    {
        yield return DelegateMap.Map((DigitalHome entity) => new BasicDigitalTwin
        {
            Id = entity.Id,
            Metadata = { ModelId = "dtmi:iothome:house;1" },
            Contents =
            {
                { "constructionYear", entity.YearBuilt },
                { "owner", entity.Owner },
                {
                    "address", new BasicDigitalTwinComponent
                    {
                        Contents =
                        {
                            { "street", entity.Address.Street },
                            { "city", entity.Address.City },
                            { "state", entity.Address.State },
                            { "zip", entity.Address.Zip }
                        }
                    }
                }
            }
        });

        yield return DelegateMap.Map((DigitalFloor entity) => new BasicDigitalTwin
        {
            Id = Guid.NewGuid().ToString(),
            Metadata = { ModelId = "dtmi:iothome:floor;1" }
        });
        
        yield return DelegateMap.Map((DigitalRoom entity) => new BasicDigitalTwin
        {
            Id = Guid.NewGuid().ToString(),
            Metadata = { ModelId = "dtmi:iothome:room;1" },
            Contents =
            {
                { "description", entity.Description }
            }
        });
    }
}