using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;
using NextUni.Modules.Academic.Domain.Majors;
using NextUni.Modules.Academic.Domain.Subjects;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Abstractions.Data;

public interface IAcademicDbContext
{
    DbSet<University> Universities { get; set; }
    DbSet<Major> Majors { get; set; }
    DbSet<Subject> Subjects { get; set; }
    DbSet<SubjectGroup> SubjectGroups { get; set; }
    DbSet<MajorSubjectGroupByYear> MajorSubjectGroupByYear { get; set; }
    // DbSet<AdmissionExamScore> AdmissionExamScores { get; set; }
    // DbSet<AdmissionGPAScore> AdmissionGPAScores { get; set; }
    DbSet<AdmissionScore> AdmissionScores { get; set; }
    DbSet<IntroductionBlog>   IntroductionBlogs { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}