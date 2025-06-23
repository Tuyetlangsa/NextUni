// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using NextUni.Modules.Academic.Domain.Majors;
//
// namespace NextUni.Modules.Academic.Infrastructure.Database;
//
// public class AdmissionExamScoreConfiguration : IEntityTypeConfiguration<AdmissionExamScore>
// {
//     public void Configure(EntityTypeBuilder<AdmissionExamScore> builder)
//     {
//         builder.ToTable("admission_exam_scores");
//
//         builder.HasKey(a => a.Id);
//
//         builder.Property(a => a.Id)
//             .IsRequired();
//
//         builder.Property(a => a.MajorId)
//             .IsRequired();
//
//         builder.Property(a => a.Year)
//             .IsRequired();
//
//         builder.Property(a => a.Score)
//             .IsRequired();
//
//         builder.HasOne(a => a.Major)
//             .WithMany(m => m.AdmissionExamScores) 
//             .HasForeignKey(a => a.MajorId);
//
//         builder.HasIndex(a => new { a.MajorId, a.Year }).IsUnique();
//     }
// }