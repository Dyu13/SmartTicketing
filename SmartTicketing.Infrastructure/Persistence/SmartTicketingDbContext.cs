using Microsoft.EntityFrameworkCore;
using SmartTicketing.Application.Interfaces;
using SmartTicketing.Domain;

namespace SmartTicketing.Infrastructure.Persistence;

public class SmartTicketingDbContext : DbContext, IApplicationDbContext
{
    public SmartTicketingDbContext(
        DbContextOptions<SmartTicketingDbContext> options)
        : base(options)
    {
        // Database.Migrate();
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Ticket> Tickets { get; set; }

    public async Task BulkInsertAsync<T>(IEnumerable<T> entities)
    {
        await BulkInsertAsync(entities);
    }
}
