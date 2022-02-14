using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using practice.domain.Entities;
using practice.domain.Kernel.Repository;

namespace practice.infrastructure;

public class UserContext:DbContext,IUnitOfWork
{
    private readonly ILogger<UserContext> _logger;

    public DbSet<User> Users { get; set; }
    public UserContext(
        DbContextOptions<UserContext> options,
        ILogger<UserContext> logger)
        :base(options)
    {
        _logger = logger;
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}