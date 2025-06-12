using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Application.Abstraction.Data;

public interface IUserDbContext
{
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}