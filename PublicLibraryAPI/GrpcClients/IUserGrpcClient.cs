using PublicLibrary.Contracts.Protos;

namespace PublicLibrary.API.GrpcClients
{
    public interface IUserGrpcClient
    {
        Task<UserLendingRateResponse> GetUserLendingRateAsync(System.DateTime fromDate, System.DateTime toDate, CancellationToken cancellationToken = default);
        Task<UserReadingPaceResponse> GetUserReadingPaceAsync(int borrowId, CancellationToken cancellationToken = default);
        Task<UserBorrowingPatternsResponse> GetUserBorrowingPatternsAsync(int bookId, CancellationToken cancellationToken = default);
    }
}
