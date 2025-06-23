using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Academic.Domain.Majors;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Infrastructure.Database;

public class MajorConfiguration : IEntityTypeConfiguration<Major>
{
    public void Configure(EntityTypeBuilder<Major> builder)
    {
        builder.ToTable("majors");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .IsRequired();

        builder.Property(m => m.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(m => m.University)
            .WithMany(u => u.Majors)
            .HasForeignKey(m => m.UniversityId);
        
        builder.Property(m => m.IsDeleted)
            .IsRequired();

        // builder.HasMany(m => m.SubjectGroups).WithMany(sg => sg.Majors).UsingEntity<MajorSubjectGroupByYear>(ms =>
        //     {
        //         ms.Property(m => m.Year);
        //         ms.HasIndex(m => new { m.MajorId, m.SubjectGroupId, m.Year });
        //     }
        // );
        
        // builder.HasMany(m => m.SubjectGroups)
        //     .WithMany(sg => sg.Majors)
        //     .UsingEntity<MajorSubjectGroupByYear>(
        //         join => join
        //             .HasOne<SubjectGroup>()
        //             .WithMany()
        //             .HasForeignKey(ms => ms.SubjectGroupId),
        //         join => join
        //             .HasOne<Major>()
        //             .WithMany()
        //             .HasForeignKey(ms => ms.MajorId),
        //         join =>
        //         {
        //             join.HasKey(ms => ms.Id); 
        //             join.Property(ms => ms.Year).IsRequired();
        //             join.HasIndex(ms => new { ms.MajorId, ms.SubjectGroupId, ms.Year }).IsUnique();
        //             join.ToTable("major_subject_group_by_year");
        //         });
        
        builder.HasMany(m => m.SubjectGroupsByYear).WithOne().HasForeignKey(m => m.MajorId);
        builder.HasIndex(m => new { m.Code, m.UniversityId })
            .IsUnique();
        builder.HasQueryFilter(m => !m.IsDeleted && !m.University.IsDeleted);
    }
} 