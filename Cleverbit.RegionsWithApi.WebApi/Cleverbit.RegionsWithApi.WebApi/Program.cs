using Cleverbit.RegionsWithApi.Infrastructure.Mediator;
using Cleverbit.RegionsWithApi.WebApi.Configuration;
using Lamar.Microsoft.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                                    optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

if (args != null)
{
    builder.Configuration.AddCommandLine(args);
}

builder.Host.UseLamar((context, serviceRegistry) =>
{
    serviceRegistry.Scan(s =>
    {
        s.WithDefaultConventions();
        s.TheCallingAssembly();
    });

    serviceRegistry.IncludeRegistry<MediatorPipelineRegistry>();
})
            .UseSerilog();

var logFilePath = $"Logs/{DateTime.UtcNow:MM_dd_yy}.txt";

//Initialize Logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.File(logFilePath)
    .CreateLogger();

builder.Logging.AddFile("Logs/app-{Date}.txt");

// Add services to the container.

builder.Services.InstallServicesInAssembly(builder.Configuration, builder.Environment);

await using var provider = builder.Services.BuildServiceProvider();

using (var scope = provider.CreateScope())
{
    var services = scope.ServiceProvider;
    //SeedData.Initialize(services);
}

var app = builder.Build();

app.SetupPipeline();

try
{
    Log.Information("Starting the web host.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
}
finally
{
    Log.CloseAndFlush();
}
