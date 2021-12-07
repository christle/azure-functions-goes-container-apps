using System.Threading.Tasks;

namespace ContainerApp.Domain
{
    public interface IWorkerApiProvider
    {
        Task<string> GetWorkerNameAsync();
    }
}