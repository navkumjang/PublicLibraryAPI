using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PublicLibrary.Contracts.Protos;
using PublicLibrary.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace PublicLibrary.Service.Services
{
    public class UserInsightsService : UserService.UserServiceBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserInsightsService> _logger;

        public UserInsightsService(IUserRepository repository, ILogger<UserInsightsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<UserLendingRateResponse> GetUserLendingRate(UserLendingRateRequest request, ServerCallContext context)
        {
            var from = request.FromDate.ToDateTime();
            var to = request.ToDate.ToDateTime();

            var response = new UserLendingRateResponse();

            var users = await _repository.GetUserLendingRateAsync(from, to, context.CancellationToken);

            foreach (var u in users)
            {
                response.Users.Add(new UserLendingRate
                {
                    BorrowerId = u.BorrowerId,
                    Name = u.Name,
                    BorrowCount = u.BorrowCount
                });
            }

            return response;
        }

        public override async Task<UserReadingPaceResponse> GetUserReadingPace(UserReadingPaceRequest request, ServerCallContext context)
        {
            var resp = new UserReadingPaceResponse();

            var paceEntries = await _repository.GetUserReadingPaceAsync(request.BorrowId, context.CancellationToken);

            foreach (var p in paceEntries)
            {
                resp.Pace.Add(new UserReadingPace
                {
                    BorrowerId = p.BorrowerId,
                    BookName = p.BookName,
                    ReturnDate = p.ReturnDate == default ? null : Timestamp.FromDateTime(p.ReturnDate.ToUniversalTime()),
                    ReadingPace = p.ReadingPace
                });
            }

            return resp;
        }

        public override async Task<UserBorrowingPatternsResponse> GetUserBorrowingPatterns(UserBorrowingPatternsRequest request, ServerCallContext context)
        {
            var response = new UserBorrowingPatternsResponse();

            var patterns = await _repository.GetUserBorrowingPatternsAsync(request.BookId, context.CancellationToken);

            foreach (var p in patterns)
            {
                response.Patterns.Add(new UserBorrowingPattern
                {
                    BookId = p.BookId,
                    BookName = p.BookName
                });
            }

            return response;
        }
    }
}
