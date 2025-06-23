
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;

namespace NextUni.Modules.Academic.Application.Majors.CreateAdmissionScoreByYear;

public abstract class CreateAdmissionScoreByYear
{
    public record Command(
        DateOnly Year,
        Dictionary<Guid, AdmissionScore> AdmissionScores) : ICommand;

    public record AdmissionScore(float GpaScore, float ExamScore);
    
    internal sealed class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            
            var majorIds = request.AdmissionScores.Keys.ToList();

            var existingMajorIds = await dbContext.Majors
                .Where(m => majorIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync(cancellationToken);

            var notFoundIds = majorIds.Except(existingMajorIds).ToList();

            if (notFoundIds.Any())
            {
                return Result.Failure(new Error(
                    "Major.NotExisted",
                    $"The following majors do not exist: {string.Join(", ", notFoundIds)}",
                    ErrorType.Conflict));
            }
            
            var existingScores = await dbContext.AdmissionScores
                .Where(a => a.Year.Year == request.Year.Year && majorIds.Contains(a.MajorId))
                .ToListAsync(cancellationToken);

            var duplicatedMajorIds = existingScores.Select(x => x.MajorId).Distinct().ToList();

            if (duplicatedMajorIds.Count > 0)
            {
                return Result.Failure(new Error(
                    "AdmissionScore.Existed", 
                    $"Admission scores for majors already exist for year {request.Year.Year}: {string.Join(", ", duplicatedMajorIds)}", 
                    ErrorType.Conflict));
            }

            var newAdmissionScores = request.AdmissionScores.Select(kv =>
                new Domain.Majors.AdmissionScore
                {
                    Id = Guid.NewGuid(),
                    MajorId = kv.Key,
                    Year = request.Year,
                    GpaScore = kv.Value.GpaScore,
                    ExamScore = kv.Value.ExamScore
                });

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
                    admissionScore.RuleFor(x => x.Value.GpaScore)
                        .GreaterThan(0).WithMessage("GPA Score must be greater than 0.");

                    admissionScore.RuleFor(x => x.Value.ExamScore)
                        .GreaterThan(0).WithMessage("Exam Score must be greater than 0.");
                });
        }

        private bool HaveUniqueMajorIds(Dictionary<Guid, AdmissionScore> admissionScores)
        {
            return admissionScores.Keys.Distinct().Count() == admissionScores.Count;
        }
    }
}