using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Chatbot.Domain.SemanticEmbeddings;

namespace NextUni.Modules.Chatbot.Application.Abstractions.Data;

public interface IVectorDbContext
{
    public DbSet<SemanticEmbedding> SemanticEmbeddings { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}