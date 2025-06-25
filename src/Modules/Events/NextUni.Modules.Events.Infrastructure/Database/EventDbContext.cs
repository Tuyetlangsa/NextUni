using Microsoft.EntityFrameworkCore;
using NextUni.Common.Infrastructure.Inbox;
using NextUni.Common.Infrastructure.Outbox;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;
using NextUni.Modules.Events.Domain.IntroductionBlogs;

namespace NextUni.Modules.Events.Infrastructure.Database;

public class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options), IEventDbContext

{
    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<EventRegistration> EventRegistrations { get; set; }
    public DbSet<IntroductionBlog> IntroductionBlogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Events);
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new IntroductionBlogConfiguration());
        modelBuilder.ApplyConfiguration(new EventRegistrationConfiguration());
    }
}