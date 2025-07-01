// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using NextUni.Modules.Chatbot.Domain.SemanticEmbeddings;
//
// namespace NextUni.Modules.Chatbot.Infrastructure.Database;
//
// public class SemanticEmbeddingConfiguration : IEntityTypeConfiguration<SemanticEmbedding>
// {
//     public void Configure(EntityTypeBuilder<SemanticEmbedding> builder)
//     {
//         builder.ToTable("semantic_embeddings");
//
//         builder.HasKey(e => e.Id);
//
//         builder.Property(e => e.EntityType)
//             .IsRequired()
//             .HasMaxLength(100);
//
//         builder.Property(e => e.EntityId)
//             .IsRequired();
//
//         builder.Property(e => e.Embedding)
//             .IsRequired()
//             .HasColumnType("vector(768)");
//     }
// }