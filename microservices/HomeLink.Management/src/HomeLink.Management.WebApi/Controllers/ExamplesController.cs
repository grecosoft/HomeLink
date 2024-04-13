using HomeLink.Management.App.Plugin.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HomeLink.Management.WebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class ExamplesController(
    ICompositeApp compositeApp,
    IOptionsSnapshot<Settings> testConfig)
    : ControllerBase
{
    private readonly ICompositeApp _compositeApp = compositeApp;
    private readonly IOptionsSnapshot<Settings> _testConfig = testConfig;

    [HttpGet("host-plugin")]
    public IActionResult GetHostPlugin() => Ok(_compositeApp.HostPlugin);

    [HttpGet("host-configs")]
    public IActionResult GetConfigs() => Ok(_testConfig);  
}