using PublicLibrary.Contracts.Protos;

namespace PublicLibrary.API.GrpcClients
{
    public class UserGrpcClient : IUserGrpcClient
    {
        private readonly UserService.UserServiceClient _userClient;
        public UserGrpcClient(UserService.UserServiceClient userClient)
        {            
            _userClient = userClient;
        }
        public async Task<UserLendingRateResponse> GetUserLendingRateAsync(System.DateTime fromDate, System.DateTime toDate, CancellationToken cancellationToken = default)
        {
            var req = new UserLendingRateRequest
            {
                FromDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(fromDate.ToUniversalTime()),
                ToDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(toDate.ToUniversalTime())
            };

            var call = _userClient.GetUserLendingRateAsync(req, cancellationToken: cancellationToken);
            return await call.ResponseAsync;
        }

        public async Task<UserReadingPaceResponse> GetUserReadingPaceAsync(int borrowId, CancellationToken cancellationToken = default)
        {
            var req = new UserReadingPaceRequest { BorrowId = borrowId };
            var call = _userClient.GetUserReadingPaceAsync(req, cancellationToken: cancellationToken);
            return await call.ResponseAsync;
        }

        public async Task<UserBorrowingPatternsResponse> GetUserBorrowingPatternsAsync(int bookId, CancellationToken cancellationToken = default)
        {
            var req = new UserBorrowingPatternsRequest { BookId = bookId };
            var call = _userClient.GetUserBorrowingPatternsAsync(req, cancellationToken: cancellationToken);
            return await call.ResponseAsync;
        }
    }
}
