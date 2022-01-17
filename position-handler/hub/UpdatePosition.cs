using System.Threading.Tasks;
using ContainerApp.Domain;
using Dapr.AzureFunctions.Extension;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContainerApp
{
    public class UpdatePosition
    {
        private readonly IWorkerApiProvider provider;
        public UpdatePosition(IWorkerApiProvider provider)
        {
            this.provider = provider;
        }
        

        [FunctionName(nameof(UpdatePosition))]
        public async Task RunAsync(
            [DaprBindingTrigger(BindingName = "positionhandler")] JObject triggerData,
            ILogger log)
        {
            var incomingEvent = JsonConvert.DeserializeObject<Event>(triggerData.ToString());
            log.LogInformation("UpdatePosition: longitude={longitude}, latitude={latitude}", incomingEvent.Longitude, incomingEvent.Latitude);

            var workerName = await provider.GetWorkerNameAsync();
            log.LogInformation("UpdatePosition: found Workername {WorkerName}", workerName);
        }
    }
}
