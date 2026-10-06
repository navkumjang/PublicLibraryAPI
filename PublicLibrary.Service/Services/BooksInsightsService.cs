using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PublicLibrary.Contracts.Protos;
using PublicLibrary.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace PublicLibrary.Service.Services
{
    public class BooksInsightsService : BookService.BookServiceBase
    {
        private readonly IBookRepository _repository;
        private readonly ILogger<BooksInsightsService> _logger;

        public BooksInsightsService(IBookRepository repository, ILogger<BooksInsightsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<InventoryInsightsResponse> GetInventoryInsights(Empty request, ServerCallContext context)
        {
            var response = new InventoryInsightsResponse();

            var insights = await _repository.GetInventoryInsightsAsync(context.CancellationToken);

            foreach (var item in insights)
            {
                response.Insights.Add(new InventoryInsight
                {
                    BookId = item.BookId,
                    BookName = item.BookName,
                    TotalBorrowed = item.TotalBorrowed
                });
            }

            return response;
        }
    }
}
