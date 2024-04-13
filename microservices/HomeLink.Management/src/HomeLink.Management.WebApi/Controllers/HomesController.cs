using HomeLink.Management.Domain.Commands;
using HomeLink.Management.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using NetFusion.Messaging;

namespace HomeLink.Management.WebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class HomesController(IMessagingService messaging) : ControllerBase
{
    private readonly IMessagingService _messaging = messaging;
    
    [HttpPost]
    public async Task<IActionResult> CreateHome(MonitorHomeCommand command)
    {
        var result = await _messaging.SendAsync(command);
        return result.ToActionResult();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHome(string id, MonitorHomeCommand command)
    {
        command.Id = id;
        
        var result = await _messaging.SendAsync(command);
        return result.ToActionResult();
    }

    [HttpPost("{id}/floors")]
    public async Task<IActionResult> CreateFloor(string id, MonitorFloorCommand command)
    {
        command.HomeId = id;
        
        var result = await _messaging.SendAsync(command);
        return result.ToActionResult();
    }
    
    [HttpPut("{id}/relationship/{relationshipId}/floors")]
    public async Task<IActionResult> UpdateFloor(string id, string relationshipId, MonitorFloorCommand command)
    {
        command.HomeId = id;
        command.RelationshipId = relationshipId;
        
        var result = await _messaging.SendAsync(command);
        return result.ToActionResult();
    }
}