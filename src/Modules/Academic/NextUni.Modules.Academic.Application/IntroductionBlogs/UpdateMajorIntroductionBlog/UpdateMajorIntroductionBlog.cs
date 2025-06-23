using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Application.IntroductionBlogs.UpdateMajorIntroductionBlog
{
    public abstract class UpdateMajorIntroductionBlog
    {
        public record Command(
        Guid MajorId,
        Guid Id,
        string Title,
        string Content) : ICommand<Guid>;

        internal sealed class Handler(IAcademicDbContext dbContext, IDateTimeProvider dateTimeProvider) : ICommandHandler<Command, Guid>
        {
            public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
            {
                bool isExisted = await dbContext.Majors.AnyAsync(u => u.Id == command.MajorId, cancellationToken);

                if (!isExisted)
                {
                    return Result.Failure<Guid>(MajorErrors.NotFound(command.MajorId));
                }

                var introductionBlog = await dbContext.IntroductionBlogs
                    .FirstOrDefaultAsync(b => b.TargetId == command.MajorId, cancellationToken);
                if (introductionBlog == null)
                {
                    return Result.Failure<Guid>(IntroductionBlogErrors.NotFound(command.Id));
                }

                introductionBlog.Title = command.Title;
                introductionBlog.Content = command.Content;
                
                dbContext.IntroductionBlogs.Update(introductionBlog);
                await dbContext.SaveChangesAsync(cancellationToken);

                return introductionBlog.Id; 
            }
        }
    }
}