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
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public BookRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<InventoryInsight>> GetInventoryInsightsAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection db = _dbFactory.CreateConnection();
            
            var result = await db.QueryAsync<InventoryInsight>(new CommandDefinition(StoredProcedureNames.GetBooksByBorrowedCount, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

            return result;
        }
    }
}
