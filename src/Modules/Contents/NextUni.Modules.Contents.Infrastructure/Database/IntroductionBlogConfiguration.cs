using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Contents.Domain.IntroductionBlogs;

namespace NextUni.Modules.Contents.Infrastructure.Database;

public class IntroductionBlogConfiguration : IEntityTypeConfiguration<IntroductionBlog>
{
    public void Configure(EntityTypeBuilder<IntroductionBlog> builder)
    {
        builder.ToTable("introduction_blogs");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.IntroductionType)
            .HasConversion<byte>() 
            .IsRequired();

        builder.Property(b => b.UniversityId)
            .IsRequired(false); 

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(b => b.Content)
            .IsRequired();

        builder.Property(b => b.PublishedAt)
            .IsRequired();

    }
}