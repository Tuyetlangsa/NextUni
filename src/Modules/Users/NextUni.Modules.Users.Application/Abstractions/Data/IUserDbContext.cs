using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Application.Abstractions.Data;

public interface IUserDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Permission> Permissions { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}