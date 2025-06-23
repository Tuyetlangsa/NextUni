using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Majors;
using Microsoft.EntityFrameworkCore;

namespace NextUni.Modules.Academic.Application.Majors.HideMajor
{
    public abstract class HideMajor
    {
        public record Command(Guid Id) : ICommand<Guid>;

        internal class Handler(
        IAcademicDbContext dbContext) : ICommandHandler<Command, Guid>
        {
            public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
            {
                var major = await dbContext.Majors
                    .SingleOrDefaultAsync(m => m.Id == command.Id, cancellationToken);
                if (major is null)
                {
                    return Result.Failure<Guid>(MajorErrors.NotFound(command.Id));
                }

                if (major.IsDeleted)
                {
                    major.IsDeleted = false;
                }
                else
                {
                    major.IsDeleted = true;
                }
                dbContext.Majors.Update(major);
                await dbContext.SaveChangesAsync(cancellationToken);
                return major.Id;
            }
        }
    }
}
