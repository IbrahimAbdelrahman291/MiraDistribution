using Microsoft.EntityFrameworkCore;
using MiraDistribution.Domain.Entities;

namespace MiraDistribution.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Book> Books { get; }
        DbSet<Distributor> Distributors { get; }
        DbSet<BookAssignmentHistory> BookAssignmentHistories { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
