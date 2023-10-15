using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using SmartTicketing.Domain;

namespace SmartTicketing.Application.Interfaces;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();

    DbSet<User> Users { get; }

    DbSet<Ticket> Tickets { get; }

    Task BulkInsertAsync<T>(IEnumerable<T> entities);

    void Dispose();
}
