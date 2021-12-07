using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ContainerApp.Domain;

namespace ContainerApp
{
    public class GetWorker
    {
        [FunctionName(nameof(GetWorker))]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "worker/{workerId}")] HttpRequest req,
            string workerId,
            ILogger log)
        {
            log.LogInformation("GetWorker: request for worker with id {workerId}", workerId);
            return new OkObjectResult(new Worker{WorkerId = "1", Name = "John Doe"});
        }
    }
}
