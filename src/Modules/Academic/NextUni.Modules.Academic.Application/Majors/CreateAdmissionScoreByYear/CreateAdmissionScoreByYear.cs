
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Application.Majors.CreateAdmissionScoreByYear;

public abstract class CreateAdmissionScoreByYear
{
    public record Command(
        int Year,
        List<AdmissionScore> AdmissionScores) : ICommand;

    public record AdmissionScore(Guid MajorId, float GpaScore, float ExamScore);
    
    internal sealed class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var majorIds = request.AdmissionScores.Select(a => a.MajorId).ToList();

            var existingMajors =  await dbContext.Majors
                .Where(m => majorIds.Contains(m.Id))
                .ToListAsync(cancellationToken);
            
            var universityId = existingMajors
                .Select(m => m.UniversityId)
                .FirstOrDefault();
            
            var existingMajorIds = existingMajors
                .Select(m => m.Id)
                .ToList();

            var notFoundIds = majorIds.Except(existingMajorIds).ToList();

            if (notFoundIds.Any())
            {
                return Result.Failure(new Error(
                    "Major.NotExisted",
                    $"The following majors do not exist: {string.Join(", ", notFoundIds)}",
                    ErrorType.Conflict));
            }
            
            var existingScores = await dbContext.AdmissionScores
                .Where(a => a.Year.Year == request.Year && majorIds.Contains(a.MajorId))
                .ToListAsync(cancellationToken);

            foreach (var existingScore in existingScores)
            {
                var updatedScore = request.AdmissionScores.FirstOrDefault(x => x.MajorId == existingScore.MajorId);
                if (updatedScore != null)
                {
                    existingScore.GpaScore = updatedScore.GpaScore;
                    existingScore.ExamScore = updatedScore.ExamScore;
                }
            }

            var majorIdsHaveScore = existingScores.Select(s => s.MajorId).ToList();
            
            
            var newAdmissionScores = request.AdmissionScores
                .Where(admissionScore => !majorIdsHaveScore.Contains(admissionScore.MajorId))
                .Select(admissionScore => new Domain.Majors.AdmissionScore
                {
                    Id = Guid.NewGuid(),
                    MajorId = admissionScore.MajorId,
                    Year = new DateOnly(request.Year, 1, 1),
                    GpaScore = admissionScore.GpaScore,
                    ExamScore = admissionScore.ExamScore
                }).ToList();
            if (newAdmissionScores.Count != 0)
            {
                newAdmissionScores.First().Raise(new AdmissionScoreByYearCreatedDomainEvent(universityId, request.Year));
            }
            else
            {
                existingScores.First().Raise(new AdmissionScoreByYearCreatedDomainEvent(universityId, request.Year));
            }
            dbContext.AdmissionScores.AddRange(newAdmissionScores);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }


    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Year).NotNull();
            RuleFor(c => c.AdmissionScores)
                .Must(HaveUniqueMajorIds)
                .WithMessage("MajorIds must be unique.");
            // Validate GPA and Exam Score not null or invalid
            RuleForEach(c => c.AdmissionScores)
                .ChildRules(admissionScore =>
                {
                    admissionScore.RuleFor(x => x.GpaScore)
                        .GreaterThan(0).WithMessage("GPA Score must be greater than 0.");

                    admissionScore.RuleFor(x => x.ExamScore)
                        .GreaterThan(0).WithMessage("Exam Score must be greater than 0.");
                });
        }

        private bool HaveUniqueMajorIds(List<AdmissionScore> admissionScores)
        {
            return admissionScores.Select(a => a.MajorId).Distinct().Count() == admissionScores.Count;
        }
    }
}