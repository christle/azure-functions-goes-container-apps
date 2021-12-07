using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContainerApp.Domain;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            [EventHubTrigger("hub-positions", Connection = "positionEventhubConnection", ConsumerGroup = "capp-consumer-group")] EventData[] events, 
            ILogger log)
        {
            var exceptions = new List<Exception>();
            foreach (EventData eventData in events)
            {
                string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                var incomingEvent = JsonConvert.DeserializeObject<Event>(messageBody);
                log.LogInformation("UpdatePosition: longitude={longitude}, latitude={latitude}", incomingEvent.Longitude, incomingEvent.Latitude);

                var workerName = await provider.GetWorkerNameAsync();
                log.LogInformation("UpdatePosition: found Workername {WorkerName}", workerName);
            }

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
