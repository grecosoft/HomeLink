using HomeLink.Common.Infra.Plugin;
using HomeLink.Management.App.Plugin;
using HomeLink.Management.App.Plugin.Configs;
using HomeLink.Management.Domain.Plugin;
using HomeLink.Management.Infra.Plugin;
using HomeLink.Management.WebApi.Controllers;
using HomeLink.Management.WebApi.Plugin;
using NetFusion.Common.Base.Serialization;
using NetFusion.Core.Settings.Plugin;
using NetFusion.Messaging.Plugin;
using NetFusion.Services.Mapping.Plugin;
using NetFusion.Services.Serialization;
using NetFusion.Web.FileProviders;

static void InitializeLogger(IConfiguration configuration)
{
    // Send any Serilog configuration issues logs to console.
    Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
    Serilog.Debugging.SelfLog.Enable(Console.Error);

    var logConfig = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
        .Destructure.UsingAttributes()

        .Enrich.FromLogContext()
        .Enrich.WithCorrelationId()
        .Enrich.WithHostIdentity(WebApiPlugin.HostId, WebApiPlugin.HostName)
        .Filter.SuppressReoccurringRequestEvents(); 

    logConfig.WriteTo.Console(theme: AnsiConsoleTheme.Code);

    var seqUrl = configuration.GetValue<string>("logging:seqUrl");
    if (!string.IsNullOrEmpty(seqUrl))
    {
        logConfig.WriteTo.Seq(seqUrl);
    }

    Log.Logger = logConfig.CreateLogger();
}


var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile(CheckSumFileProvider.FromRelativePath("configs"), 
    "appsettings.json", 
    optional: true, 
    reloadOnChange: true).AddEnvironmentVariables();

builder.Services.Configure<Settings>(builder.Configuration.GetSection("settings"));

// Configure Logging:
InitializeLogger(builder.Configuration);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);
builder.Host.UseSerilog();

builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

var bootstrapLoggerFactory = LoggerFactory.Create(config =>
{ 
    config.ClearProviders();
    config.AddSerilog(Log.Logger);
    config.SetMinimumLevel(LogLevel.Trace);
});

try
{
    // Add Plugins to the Composite-Container:
    builder.Services.CompositeContainer(builder.Configuration, bootstrapLoggerFactory, new SerilogExtendedLogger())
        .AddSettings()
        .AddMapping()
        .AddMessaging()
        //.AddAzureServiceBus()
        
        .AddPlugin<CommonInfraPlugin>()
        .AddPlugin<InfraPlugin>()
        .AddPlugin<AppPlugin>()
        .AddPlugin<DomainPlugin>()
    .AddPlugin<WebApiPlugin>()
    .Compose(s =>
    {
        s.AddSingleton<ISerializationManager, SerializationManager>();
    });
}
catch
{
    Log.CloseAndFlush();
}

builder.Services.AddMvc(options =>
{
    options.ModelValidatorProviders.Add(new MessageModelValidator());
});

var app = builder.Build();

string? viewerUrl = app.Configuration.GetValue<string>("Netfusion:ViewerUrl");
if (!string.IsNullOrWhiteSpace(viewerUrl))
{
    app.UseCors(cors => cors.WithOrigins(viewerUrl)
        .AllowAnyMethod()
        .AllowCredentials()
        .WithExposedHeaders("WWW-Authenticate", "resource-404")
        .AllowAnyHeader());
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapCompositeLog();
app.MapHealthCheck();
app.MapStartupCheck();
app.MapReadinessCheck();

if (app.Environment.IsDevelopment())
{
    app.MapCompositeLog();
}


// Reference the Composite-Application to start the plugins then
// start the web application.
var compositeApp = app.Services.GetRequiredService<ICompositeApp>();
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStopping.Register(() =>
{
    compositeApp.Stop();
    Log.CloseAndFlush();
});

var runTask = app.RunAsync();
await compositeApp.StartAsync();
await runTask;