using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;
using NextUni.Modules.Events.Domain.IntroductionBlogs;

namespace NextUni.Modules.Events.Application.IntroductionBlogs;


public abstract class CreateEventIntroductionBlog
{
    public record Command(
        Guid EventId,
        string Title,
        string Content) : ICommand<Guid>;
    
    internal sealed class Handler(IEventDbContext dbContext, IDateTimeProvider dateTimeProvider) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            bool isExisted = await dbContext.Events.IgnoreQueryFilters().AnyAsync(u => u.Id == command.EventId, cancellationToken);

            if (!isExisted)
            {
                return Result.Failure<Guid>(EventErrors.NotFound(command.EventId));
            }

            IntroductionBlog blog = new()
            {
                Id = Guid.NewGuid(),
                TargetId = command.EventId,
                IntroductionType = IntroductionType.Event,
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
            RuleFor(c => c.EventId).NotNull().NotEmpty();
        }
    }
}