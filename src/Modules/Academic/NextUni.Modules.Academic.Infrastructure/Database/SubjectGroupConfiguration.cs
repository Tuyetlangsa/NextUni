using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Infrastructure.Database;

public class SubjectGroupConfiguration : IEntityTypeConfiguration<SubjectGroup>
{
    public void Configure(EntityTypeBuilder<SubjectGroup> builder)
    {
        builder.ToTable("subject_groups");

        builder.HasKey(sg => sg.Id);

        builder.Property(sg => sg.Id)
            .IsRequired();

        builder.Property(sg => sg.Code)
            .IsRequired()
            .HasMaxLength(50);
        //
        // builder.Property(sg => sg.Name)
        //     .IsRequired()
        //     .HasMaxLength(255);

        builder.HasMany(sg => sg.Subjects)
            .WithMany(sg => sg.SubjectGroups);

        builder.HasIndex(sg => sg.Code)
            .IsUnique();
        
    }
}