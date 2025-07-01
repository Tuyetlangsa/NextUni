using NextUni.Common.Domain;
using Pgvector;

namespace NextUni.Modules.Chatbot.Domain.SemanticEmbeddings;

public class SemanticEmbedding : Entity
{
    public Guid Id { get; set; }
    
    public string EntityType { get; set; } = null!;  
    
    public Guid EntityId { get; set; }  
    
    // public Vector Embedding { get; set; } = null!;
    public float[] Embedding { get; set; } = null!; 
}