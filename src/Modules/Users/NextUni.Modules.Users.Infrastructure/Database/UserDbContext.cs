using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Users.Application.Abstraction.Data;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Infrastructure.Database;

public class UserDbContext(DbContextOptions options) : DbContext(options), IUserDbContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}