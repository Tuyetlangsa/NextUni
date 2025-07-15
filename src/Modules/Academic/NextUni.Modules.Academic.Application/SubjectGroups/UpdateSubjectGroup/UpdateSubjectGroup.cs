using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Application.SubjectGroups.UpdateSubjectGroup
{
    public abstract class UpdateSubjectGroup
    {
        public record Command(Guid Id, string Code, List<Guid> SubjectIds) : ICommand;

        internal class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                bool isExisted = await dbContext.SubjectGroups.AnyAsync(s => s.Code == command.Code && s.Id != command.Id, cancellationToken);
                if (isExisted)
                {
                    return Result.Failure<Guid>(SubjectGroupErrors.SubjectExisted(command.Code));
                }

                if (command.SubjectIds.Distinct().Count() != command.SubjectIds.Count)
                {
                    return Result.Failure<Guid>(SubjectErrors.DuplicatedNewSubjects());
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

                var subjectGroup = await dbContext.SubjectGroups
                    .Include(sg => sg.Subjects)
                    .FirstOrDefaultAsync(sg => sg.Id == command.Id, cancellationToken);

                if (subjectGroup is null)
                {
                    return Result.Failure<Guid>(SubjectGroupErrors.NotFound(command.Id));
                }
                subjectGroup.Subjects.Clear();

                await dbContext.SaveChangesAsync(cancellationToken); subjectGroup.Code = command.Code;
                subjectGroup.Subjects = subjects;
                dbContext.SubjectGroups.Update(subjectGroup);
                await dbContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
        }

        internal sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Code).NotNull().NotEmpty();
                RuleFor(x => x.SubjectIds).NotNull()
                                          .NotEmpty()
                                          .Must(list => list.Count == 3)
                                          .WithMessage("Exactly 3 Subject IDs are required.");
            }
        }
    }
}