using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;
using NextUni.Modules.Academic.Domain.Majors;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.IntroductionBlogs.CreateMajorIntroductionBlog;

public abstract class CreateMajorIntroductionBlog
{
    public record Command(
        Guid MajorId,
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
                .FirstOrDefaultAsync(b => b.TargetId == command.MajorId && 
                                          b.IntroductionType == IntroductionType.University, cancellationToken);

            if (introductionBlog != null)
            {
                introductionBlog.Title = command.Title;
                introductionBlog.Content = command.Content;
                await dbContext.SaveChangesAsync(cancellationToken);
                return introductionBlog.Id;
            }

            IntroductionBlog blog = new()
            {
                Id = Guid.NewGuid(),
                TargetId = command.MajorId,
                IntroductionType = IntroductionType.Major,
                Title = command.Title,
                Content = command.Content,
                PublishedAt = dateTimeProvider.UtcNow
            };
            
            dbContext.IntroductionBlogs.Add(blog);
            await dbContext.SaveChangesAsync(cancellationToken);

            return blog.Id;
        }
    }
    
    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Title).NotNull().NotEmpty().MaximumLength(500);
            RuleFor(c => c.Content).NotNull().NotEmpty();
            RuleFor(c => c.MajorId).NotNull().NotEmpty();
        }
    }
}