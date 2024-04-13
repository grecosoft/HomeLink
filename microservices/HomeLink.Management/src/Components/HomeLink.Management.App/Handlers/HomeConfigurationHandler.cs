using Azure.DigitalTwins.Core;
using HomeLink.Management.App.Repositories;
using HomeLink.Management.Domain.Commands;
using HomeLink.Management.Domain.Entities;
using NetFusion.Services.Mapping;

namespace HomeLink.Management.App.Handlers;

/// <summary>
/// Responsible for defining the structure of home to be monitored by devices.
/// </summary>
public class HomeConfigurationHandler(IObjectMapper mapper, ITwinRepository twinRepo) : IMessageConsumer
{
    private readonly IObjectMapper _mapper = mapper;
    private readonly ITwinRepository _twinRepo = twinRepo;

    [InProcessHandler]
    public async Task<EntityResult> ConfigureHome(MonitorHomeCommand command)
    {
        var home = CreateValidHomeEntity(command);
        var twin = _mapper.Map<BasicDigitalTwin>(home);
        try
        {
            var response = await _twinRepo.WriteTwin(twin);
            return EntityResult.For(response.Value.Id, response.Value.ETag.ToString());
        }
        catch (RequestFailedException ex)
        {
            return EntityResult.WithError(ex.Message);
        }
    }

    private static DigitalHome CreateValidHomeEntity(MonitorHomeCommand command)
    {
        return new DigitalHome(command.Owner, command.YearBuilt,
            new Address(command.Street, command.City, command.State, command.Zip))
        {
            Id = command.Id ?? Guid.NewGuid().ToString()
        };
    }

    [InProcessHandler]
    public async Task<EntityResult> ConfigureFloor(MonitorFloorCommand command)
    {
        var relationshipProps = new Dictionary<string, object>
        {
            { "level", command.Level }
        };

        var floor = new DigitalFloor(command.Level);
        
        try
        {
            if (command.IsNewSetup)
            {
                var twin = _mapper.Map<BasicDigitalTwin>(floor);
                var createdResp = await _twinRepo.WriteTwin(twin);
                var relationResp = await _twinRepo.CreateRelationship(command.HomeId, twin.Id, "has_floor", relationshipProps);

                return EntityResult.For(createdResp.Value.Id, createdResp.Value.ETag.ToString(), 
                    result => result.RelationshipId = relationResp.Value.Id);
            }

            var updateResp = await _twinRepo.UpdateRelationship(command.HomeId, command.RelationshipId!, relationshipProps);
            
            return EntityResult.For(command.HomeId, updateResp.Headers.ETag.ToString(), 
                result => result.RelationshipId = command.RelationshipId);
        }
        catch (RequestFailedException ex)
        {
            return EntityResult.WithError(ex.Message);
        }
    }
}