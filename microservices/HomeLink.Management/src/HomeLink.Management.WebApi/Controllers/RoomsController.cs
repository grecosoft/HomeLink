using HomeLink.Management.WebApi.Models;
using HomeLink.Management.Domain.Commands;
using HomeLink.Management.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using NetFusion.Messaging;

namespace HomeLink.Management.WebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class RoomsController(IMessagingService messaging) : ControllerBase
{
    private readonly IMessagingService _messaging = messaging;

    [HttpPost("{id}/devices")]
    public async Task<IActionResult> AddDevice(string id, ConfigureDeviceCommand command)
    {
        command.RoomId = id;
        
        var result = await _messaging.SendAsync(command);
        return result.ToActionResult();
    }
}