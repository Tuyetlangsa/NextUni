using MediatR;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Majors;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Application.Majors.UpdateMajorSubjectGroupsByYear;

public abstract class UpdateMajorSubjectGroupByYear
{
    public record Command(Guid MajorId, List<Guid> GroupIds, int Year) : ICommand;
    
    internal sealed class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var major = await dbContext.Majors.FirstOrDefaultAsync(m => m.Id == request.MajorId, cancellationToken);
            if (major is null)
            {
                return Result.Failure(MajorErrors.NotFound(request.MajorId));
            }

            if (request.GroupIds.Distinct().Count() != request.GroupIds.Count)
            {
                return Result.Failure<Guid>(SubjectGroupErrors.DuplicatedRequestSubjects());
            }

            var existingGroupIds = await dbContext.SubjectGroups
                .Where(sg => request.GroupIds.Contains(sg.Id))
                .Select(sg => sg.Id)
                .ToListAsync(cancellationToken);

            var notExistedIds = request.GroupIds.Except(existingGroupIds).ToList();

            if (notExistedIds.Any())
            {
                return Result.Failure(new Error(
                    "SubjectGroup.NotExisted",
                    $"The following subject group Ids were not existed: {string.Join(", ", notExistedIds)}",
                    ErrorType.Conflict));
            }

            var existingRelations = await dbContext.MajorSubjectGroupByYear
                .Where(x => x.MajorId == request.MajorId && x.Year.Year == request.Year)
                .ToListAsync(cancellationToken);

            dbContext.MajorSubjectGroupByYear.RemoveRange(existingRelations);

            var year = new DateOnly(request.Year, 1, 1);

            // 4. Add new relations
            var newRelations = request.GroupIds.Select(groupId => new MajorSubjectGroupByYear
            {
                MajorId = request.MajorId,
                SubjectGroupId = groupId,
                Year = year
            }).ToList();

            await dbContext.MajorSubjectGroupByYear.AddRangeAsync(newRelations, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}