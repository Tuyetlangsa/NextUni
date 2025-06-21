using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Events.Domain.Events;
using NextUni.Modules.Events.Domain.IntroductionBlogs;

namespace NextUni.Modules.Events.Application.Abstractions.Data;

public interface IEventDbContext
{
    DbSet<Event> Events { get; set; }
    DbSet<User> Users { get; set; }
    
    public DbSet<IntroductionBlog> IntroductionBlogs { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}