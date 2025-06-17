using Microsoft.EntityFrameworkCore;
using NextUni.Common.Infrastructure.Inbox;
using NextUni.Common.Infrastructure.Outbox;
using NextUni.Modules.Contents.Application.Abstractions.Data;

namespace NextUni.Modules.Contents.Infrastructure.Database;

public class ContentDbContext(DbContextOptions<ContentDbContext> options) : DbContext(options), IContentDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Contents);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
    }
}