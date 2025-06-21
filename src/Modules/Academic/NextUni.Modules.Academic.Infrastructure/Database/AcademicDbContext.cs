using Microsoft.EntityFrameworkCore;
using NextUni.Common.Infrastructure.Inbox;
using NextUni.Common.Infrastructure.Outbox;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;
using NextUni.Modules.Academic.Domain.Majors;
using NextUni.Modules.Academic.Domain.Subjects;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Infrastructure.Database;

public class AcademicDbContext(DbContextOptions<AcademicDbContext> options) : DbContext(options), IAcademicDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Academic);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new UniversityConfiguration());
        modelBuilder.ApplyConfiguration(new MajorConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectGroupConfiguration());
        modelBuilder.ApplyConfiguration(new AdmissionExamScoreConfiguration());
        modelBuilder.ApplyConfiguration(new AdmissionGPAScoreConfiguration());
        modelBuilder.ApplyConfiguration(new IntroductionBlogConfiguration());
    }

    public DbSet<University> Universities { get; set; }
    public DbSet<Major> Majors { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectGroup> SubjectGroups { get; set; }
    public DbSet<MajorSubjectGroupByYear> MajorSubjectGroupByYear { get; set; }
    public DbSet<AdmissionExamScore> AdmissionExamScores { get; set; }
    public DbSet<AdmissionGPAScore> AdmissionGPAScores { get; set; }
    public DbSet<IntroductionBlog> IntroductionBlogs { get; set; }
}