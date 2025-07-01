using Microsoft.EntityFrameworkCore;
using NextUni.Common.Infrastructure.Inbox;
using NextUni.Common.Infrastructure.Outbox;
using NextUni.Modules.Chatbot.Application.Abstractions.Data;
using NextUni.Modules.Chatbot.Domain.SemanticEmbeddings;

namespace NextUni.Modules.Chatbot.Infrastructure.Database;

public class VectorDbContext(DbContextOptions<VectorDbContext> options) : DbContext(options), IVectorDbContext
{
    public DbSet<SemanticEmbedding> SemanticEmbeddings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.HasPostgresExtension("vector");
        modelBuilder.HasDefaultSchema(Schemas.Chatbot);
        // modelBuilder.ApplyConfiguration(new SemanticEmbeddingConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
    }
}