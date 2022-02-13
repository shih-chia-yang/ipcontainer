using Microsoft.EntityFrameworkCore;
using practice.domain.Entities;

namespace practice.infrastructure;

public class UserContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public UserContext(DbContextOptions<UserContext> options):base(options)
    {
        
    }
}