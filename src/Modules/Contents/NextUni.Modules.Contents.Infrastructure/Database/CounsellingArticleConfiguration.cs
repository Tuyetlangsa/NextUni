using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Infrastructure.Database;

public class CounsellingArticleConfiguration : IEntityTypeConfiguration<CounsellingArticle>
{
    public void Configure(EntityTypeBuilder<CounsellingArticle> builder)
    {
        builder.ToTable("counselling_articles");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .IsRequired();

        builder.Property(a => a.UniversityId)
            .IsRequired(false); 

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.Content)
            .IsRequired();

        builder.Property(a => a.PublishAt)
            .IsRequired();

        builder.Property(a => a.Type)
            .HasConversion<byte>()
            .IsRequired();
        builder.HasQueryFilter(c => c.Status != CounsellingArticleStatus.Draft && c.Status != CounsellingArticleStatus.Pending);
    }
}