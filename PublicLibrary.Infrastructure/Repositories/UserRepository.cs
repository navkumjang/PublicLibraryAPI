using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PublicLibrary.Domain.Entities;
using PublicLibrary.Domain.Interfaces;
using PublicLibrary.Infrastructure.Data;

namespace PublicLibrary.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public UserRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<UserLendingRate>> GetUserLendingRateAsync(System.DateTime fromDate, System.DateTime toDate, CancellationToken cancellationToken = default)
        {
            using IDbConnection db = _dbFactory.CreateConnection();
            var parameters = new { FromDate = fromDate, ToDate = toDate };

            var result = await db.QueryAsync<UserLendingRate>(new CommandDefinition(StoredProcedureNames.GetUserLendingRate, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            return result;
        }

        public async Task<IEnumerable<UserReadingPace>> GetUserReadingPaceAsync(int borrowId, CancellationToken cancellationToken = default)
        {
            using IDbConnection db = _dbFactory.CreateConnection();
            var result = await db.QueryAsync<UserReadingPace>(new CommandDefinition(StoredProcedureNames.GetUserReadingPace, new { BorrowerId = borrowId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            return result;
        }

        public async Task<IEnumerable<UserBorrowingPattern>> GetUserBorrowingPatternsAsync(int bookId, CancellationToken cancellationToken = default)
        {
            using IDbConnection db = _dbFactory.CreateConnection();
            var result = await db.QueryAsync<UserBorrowingPattern>(new CommandDefinition(StoredProcedureNames.GetUserBorrowingPatterns, new { BookId = bookId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            return result;
        }
    }
}
