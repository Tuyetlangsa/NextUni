using Microsoft.EntityFrameworkCore;
using NextUni.Common.Infrastructure.Inbox;
using NextUni.Common.Infrastructure.Outbox;
using NextUni.Modules.Contents.Application.Abstractions.Data;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Infrastructure.Database;

public class ContentDbContext(DbContextOptions<ContentDbContext> options) : DbContext(options), IContentDbContext
{
    public DbSet<CounsellingArticle> CounsellingArticles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Contents);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new CounsellingArticleConfiguration());
    }


}