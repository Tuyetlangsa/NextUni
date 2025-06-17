using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.Abstractions.Data;

public interface IEventDbContext
{
    DbSet<Event> Events { get; set; }
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}