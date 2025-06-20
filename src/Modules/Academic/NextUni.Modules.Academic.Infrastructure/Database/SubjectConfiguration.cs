using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Infrastructure.Database;

public class SubjectConfiguration :  IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("subjects");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .IsRequired();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasIndex(s => s.Name)
            .IsUnique();

        builder.Property(s => s.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}