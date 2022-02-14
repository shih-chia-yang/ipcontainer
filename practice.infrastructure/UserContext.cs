using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using practice.domain.Entities;
using practice.domain.Kernel.Repository;

namespace practice.infrastructure;

public class UserContext:DbContext,IUnitOfWork
{
    private readonly ILogger _logger;

    public DbSet<User> Users { get; set; }
    public UserContext(
        DbContextOptions<UserContext> options,
        ILoggerFactory loggerFactory)
        :base(options)
    {
        _logger = loggerFactory.CreateLogger("db_logs");
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}