using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Infrastructure.Database;

public class AdmissionGPAScoreConfiguration : IEntityTypeConfiguration<AdmissionGPAScore>
{
    public void Configure(EntityTypeBuilder<AdmissionGPAScore> builder)
    {
        builder.ToTable("admission_gpa_scores");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .IsRequired();

        builder.Property(a => a.MajorId)
            .IsRequired();

        builder.Property(a => a.Year)
            .IsRequired();

        builder.Property(a => a.Score)
            .IsRequired();

        builder.HasOne(a => a.Major)
            .WithMany(m => m.AdmissionGPAScores) 
            .HasForeignKey(a => a.MajorId);

        builder.HasIndex(a => new { a.MajorId, a.Year }).IsUnique();
    }
}