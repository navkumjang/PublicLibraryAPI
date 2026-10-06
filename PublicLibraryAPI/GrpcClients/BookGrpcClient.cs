using System.Threading;
using System.Threading.Tasks;
using PublicLibrary.Contracts.Protos;
using Grpc.Net.Client;

namespace PublicLibrary.API.GrpcClients
{
    public class BookGrpcClient : IBookGrpcClient
    {
        private readonly BookService.BookServiceClient _bookClient;        

        public BookGrpcClient(BookService.BookServiceClient bookClient)
        {
            _bookClient = bookClient;            
        }

        public async Task<InventoryInsightsResponse> GetInventoryInsightsAsync(CancellationToken cancellationToken = default)
        {
            var call = _bookClient.GetInventoryInsightsAsync(new global::Google.Protobuf.WellKnownTypes.Empty(), cancellationToken: cancellationToken);
            return await call.ResponseAsync;
        }

    }
}
