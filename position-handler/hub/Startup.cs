using System;
using Adapter;
using ContainerApp.Domain;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(hub.Startup))]
namespace hub
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var urlWorkerApi = Environment.GetEnvironmentVariable("WORKER_API");

            builder.Services.AddSingleton<ITelemetryInitializer, CloudRoleName>();
            builder.Services.AddSingleton<IWorkerApiProvider>(x=> new WorkerApiProvider(urlWorkerApi));
            builder.Services.AddLogging();
        }
    }

    public class CloudRoleName : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = "PositionHandler-CApp";
        }
    }
}