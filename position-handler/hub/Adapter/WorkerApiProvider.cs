using System;
using System.Net.Http;
using System.Threading.Tasks;
using ContainerApp.Domain;

namespace Adapter
{

    public class WorkerApiProvider : IWorkerApiProvider
    {
        private readonly string url;

        private readonly HttpClient client;

        public WorkerApiProvider(string url) 
        {
            this.url = url;
            this.client = new HttpClient();
        }

        public async Task<string> GetWorkerNameAsync()
        {
            var response = await client.GetAsync($"http://{this.url}/api/worker/1");
            var content = await response.Content.ReadAsAsync<Worker>();

            return content.Name;
        }
    }
}