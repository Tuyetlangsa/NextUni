using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Infrastructure.Database;

public class AdmissionScoreConfiguration : IEntityTypeConfiguration<AdmissionScore>
{
    public void Configure(EntityTypeBuilder<AdmissionScore> builder)
    {
        builder.ToTable("admission_gpa_scores");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .IsRequired();

        builder.Property(a => a.MajorId)
            .IsRequired();

        builder.Property(a => a.Year)
            .IsRequired();

        builder.Property(a => a.GpaScore)
            .IsRequired();
        builder.Property(a => a.ExamScore)
            .IsRequired();
        
        builder.HasOne(a => a.Major)
            .WithMany(m => m.AdmissionScore) 
            .HasForeignKey(a => a.MajorId);

        builder.HasIndex(a => new { a.MajorId, a.Year }).IsUnique();

        builder.HasQueryFilter(s => !s.Major.IsDeleted);
    }
}