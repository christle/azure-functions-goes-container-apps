using System;
using System.Net.Http;
using System.Threading.Tasks;
using ContainerApp.Domain;
using Dapr.Client;

namespace Adapter
{

    public class WorkerApiProvider : IWorkerApiProvider
    {
        public async Task<string> GetWorkerNameAsync()
        {
            var client = DaprClient.CreateInvokeHttpClient();

            var response = await client.GetAsync("http://worker-api-capp/api/worker/1");
            var content = await response.Content.ReadAsAsync<Worker>();
            
            return content.Name;
        }
    }
}