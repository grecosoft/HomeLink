using Azure.DigitalTwins.Core;
using HomeLink.Management.App.Repositories;
using HomeLink.Management.Domain.Commands;
using HomeLink.Management.Domain.Entities;
using NetFusion.Services.Mapping;

namespace HomeLink.Management.App.Handlers;

public class FloorConfigurationHandler(IObjectMapper mapper, ITwinRepository twinRepo) : IMessageConsumer
{
    private readonly IObjectMapper _mapper = mapper;
    private readonly ITwinRepository _twinRepo = twinRepo;

    [InProcessHandler]
    public async Task<EntityResult> ConfigureRoom(MonitorRoomCommand command)
    {
        var room = new DigitalRoom(command.Description);
        
        var twin = _mapper.Map<BasicDigitalTwin>(room);
        try
        {
            var createdResp = await _twinRepo.WriteTwin(twin);
            var relationResp = await _twinRepo.CreateRelationship(command.FloorId, twin.Id, "contains_room");

            return EntityResult.For(createdResp.Value.Id, createdResp.Value.ETag.ToString(), 
                result => result.RelationshipId = relationResp.Value.Id);
        }
        catch (RequestFailedException ex)
        {
            return EntityResult.WithError(ex.Message);
        }
    }
}