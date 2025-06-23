using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Application.SubjectGroups.CreateSubjectGroup;

public abstract class CreateSubjectGroup
{
    public record Command(
        string Code,
        List<Guid> SubjectIds) : ICommand<Guid>;
    
    internal sealed class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            // code is unique
            bool isExisted = await dbContext.SubjectGroups.AnyAsync(x => x.Code == command.Code, cancellationToken);

            if (isExisted)
            {
                return Result.Failure<Guid>(SubjectGroupErrors.SubjectExisted(command.Code));
            }
            
            var existingIds = await dbContext.Subjects
                .Where(s => command.SubjectIds.Contains(s.Id))
                .Select(s => s.Id)
                .ToListAsync(cancellationToken);

            var missingIds = command.SubjectIds
                .Where(id => !existingIds.Contains(id))
                .ToList();

            if (missingIds.Any())
            {
                return Result.Failure<Guid>(
                    new Error(
                        "Subjects.NotExisted",
                        $"The following SubjectIds do not exist: {string.Join(", ", missingIds)}",
                        ErrorType.NotFound));
            }
            
            var subjects = await dbContext.Subjects
                .Where(s => command.SubjectIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            SubjectGroup subjectGroup = new SubjectGroup()
            {
                Id = Guid.NewGuid(),
                Code = command.Code,
                Subjects = subjects,
            };

            dbContext.SubjectGroups.Add(subjectGroup);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return subjectGroup.Id;
        }
    }
    
    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
           RuleFor(s => s.Code).NotNull().NotEmpty().MaximumLength(50);
        }
    }
}