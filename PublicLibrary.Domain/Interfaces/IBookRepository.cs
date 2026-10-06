using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PublicLibrary.Domain.Entities;

namespace PublicLibrary.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<InventoryInsight>> GetInventoryInsightsAsync(CancellationToken cancellationToken = default);
    }
}
