using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PublicLibrary.Domain.Entities;

namespace PublicLibrary.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserLendingRate>> GetUserLendingRateAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserReadingPace>> GetUserReadingPaceAsync(int borrowId, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserBorrowingPattern>> GetUserBorrowingPatternsAsync(int bookId, CancellationToken cancellationToken = default);
    }
}
