using System.Threading;
using System.Threading.Tasks;
using PublicLibrary.Contracts.Protos;

namespace PublicLibrary.API.GrpcClients
{
    public interface IBookGrpcClient
    {
        Task<InventoryInsightsResponse> GetInventoryInsightsAsync(CancellationToken cancellationToken = default);

    }
}
