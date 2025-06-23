using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Universities.HideUniversity
{
    public abstract class HideUniversity
    {
        public record Command(Guid Id) : ICommand<Guid>;

        internal class Handler(
        IAcademicDbContext dbContext) : ICommandHandler<Command, Guid>
        {
            public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
            {
                var university = await dbContext.Universities
                    .SingleOrDefaultAsync(m => m.Id == command.Id, cancellationToken);
                if (university is null)
                {
                    return Result.Failure<Guid>(UniversityErrors.NotFound(command.Id));
                }

                if (university.IsDeleted)
                {
                    university.IsDeleted = false;
                }
                else
                {
                    university.IsDeleted = true;
                }
                dbContext.Universities.Update(university);
                await dbContext.SaveChangesAsync(cancellationToken);
                return university.Id;
            }
        }
    }
}
