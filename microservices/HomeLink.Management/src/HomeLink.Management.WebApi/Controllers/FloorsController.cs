using HomeLink.Management.Domain.Commands;
using HomeLink.Management.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using NetFusion.Messaging;

namespace HomeLink.Management.WebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class FloorsController(IMessagingService messaging) : ControllerBase
{
    private readonly IMessagingService _messaging = messaging;

    [HttpPost("{id}/rooms")]
    public async Task<IActionResult> CreateRoom(string id, MonitorRoomCommand command)
    {
        command.FloorId = id;
        
        var result = await _messaging.SendAsync(command);
        return result.ToActionResult();
    }
}