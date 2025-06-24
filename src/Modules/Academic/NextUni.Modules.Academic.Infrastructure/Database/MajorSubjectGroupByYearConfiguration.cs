using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Infrastructure.Database;

public class MajorSubjectGroupByYearConfiguration : IEntityTypeConfiguration<MajorSubjectGroupByYear>
{
    public void Configure(EntityTypeBuilder<MajorSubjectGroupByYear> builder)
    {
        builder.HasKey(ms => ms.Id); 
        builder.Property(ms => ms.Year).IsRequired();
        builder.HasIndex(ms => new { ms.MajorId, ms.SubjectGroupId, ms.Year }).IsUnique();
        builder.ToTable("major_subject_group_by_year");
    }
}