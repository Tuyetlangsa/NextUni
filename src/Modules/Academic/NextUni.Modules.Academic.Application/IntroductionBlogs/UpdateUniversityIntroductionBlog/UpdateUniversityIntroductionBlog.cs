using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Application.IntroductionBlogs.UpdateUniversityIntroductionBlog
{
    public abstract class UpdateUniversityIntroductionBlog
    {
        public record Command(
        Guid UniversityId,
        string Title,
        string Content) : ICommand<Guid>;

        internal sealed class Handler(IAcademicDbContext dbContext, IDateTimeProvider dateTimeProvider) : ICommandHandler<Command, Guid>
        {
            public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
            {
                bool isExisted = await dbContext.Majors.AnyAsync(u => u.Id == command.UniversityId, cancellationToken);

                if (!isExisted)
                {
                    return Result.Failure<Guid>(MajorErrors.NotFound(command.UniversityId));
                }

                var introductionBlog = await dbContext.IntroductionBlogs
                    .SingleAsync(b => b.TargetId == command.UniversityId, cancellationToken);
               
                introductionBlog.Title = command.Title;
                introductionBlog.Content = command.Content;
                
                dbContext.IntroductionBlogs.Update(introductionBlog);
                await dbContext.SaveChangesAsync(cancellationToken);

                return introductionBlog.Id; 
            }
        }
    }
}