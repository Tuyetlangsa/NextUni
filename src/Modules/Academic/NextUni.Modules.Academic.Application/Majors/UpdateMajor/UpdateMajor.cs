using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;
using NextUni.Modules.Academic.Domain.Majors;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Majors.UpdateMajor
{
    public abstract class UpdateMajor
    {
        public record Command(
        Guid Id,
        string Code,
        string Name,
        Guid UniversityId,
        string Title,
        string Content) : ICommand<Guid>;

        internal class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command, Guid>
        {
            public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
            {
                bool isUniversityExisted = await dbContext.Universities
                    .AnyAsync(u => u.Id == command.UniversityId, cancellationToken);
                if (!isUniversityExisted)
                {
                    return Result.Failure<Guid>(UniversityErrors.NotFound(command.UniversityId));
                }

                var major = await dbContext.Majors
                    .SingleAsync(m => m.Id == command.Id, cancellationToken);
                if (major is null)
                {
                    return Result.Failure<Guid>(MajorErrors.NotFound(command.Id));
                }

                bool isCodeMajorExisted = await dbContext.Majors
                    .AnyAsync(m => m.Code == command.Code && m.Id != command.Id, cancellationToken);
                if (isCodeMajorExisted)
                {
                    return Result.Failure<Guid>(MajorErrors.NotFound(command.Id));
                }

                major.Code = command.Code;
                major.Name = command.Name;
                major.UniversityId = command.UniversityId;

                var introductionBlog = await dbContext.IntroductionBlogs
                    .FirstOrDefaultAsync(b => b.TargetId == major.Id 
                                              && b.IntroductionType == IntroductionType.Major, cancellationToken);
                if (introductionBlog is null)
                {
                    return Result.Failure<Guid>(IntroductionBlogErrors.NotFound(major.Id));
                }

                major.Raise(new MajorUpdatedDomainEvent(major.Id, introductionBlog.Id, command.Title, command.Content));
                dbContext.Majors.Update(major);
                await dbContext.SaveChangesAsync(cancellationToken);
                return major.Id;
            }
        }

        internal sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Id).NotNull().NotEmpty();
                RuleFor(c => c.Code).NotNull().NotEmpty();
                RuleFor(c => c.Name).NotNull().NotEmpty();
                RuleFor(c => c.UniversityId).NotNull().NotEmpty();
            }
        }
    }
}
