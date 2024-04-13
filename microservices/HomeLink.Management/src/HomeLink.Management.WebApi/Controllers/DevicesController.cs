using HomeLink.Management.App.Repositories;
using HomeLink.Management.Domain.Commands;
using HomeLink.Management.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using NetFusion.Messaging;

namespace HomeLink.Management.WebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class DevicesController(
    IDeviceDataService deviceDataModelService,
    ITwinSchemaRepository twinModelRepo,
    IMessagingService messaging) : ControllerBase
{
    private readonly IDeviceDataService _deviceDataModelService = deviceDataModelService;
    private readonly ITwinSchemaRepository _twinModelRepo = twinModelRepo;
    private readonly IMessagingService _messaging = messaging;

    [HttpPost("models/{deviceModelId}")]
    public async Task<IActionResult> CreateDataModel(string deviceModelId)
    {
        var dataModel = await _deviceDataModelService.CreateDeviceDataModel(deviceModelId);
        await _twinModelRepo.WriteModelSchemaAsync(dataModel);

        return Ok();
    }

    [HttpPut("{deviceId}/method")]
    public async Task<IActionResult> GetDevice(string deviceId, SendDeviceMethodCommand command)
    {
        command.DeviceId = deviceId;

        var result = await _messaging.SendAsync(command);
        return Ok(result);
    }
    
    [HttpGet("commands")]
    public async Task<IActionResult> GetCommands()
    {
        var schema = await _twinModelRepo.ReadModelSchemaAsync("dtmi:azurertos:devkit:gsgmxchip;2");
        return Ok();
    }
}