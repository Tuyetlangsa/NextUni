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
        string Title,
        string Content) : ICommand;

        internal class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var query = dbContext.Majors.AsNoTracking().AsQueryable();
                var major = await query.FirstOrDefaultAsync(m => m.Id == command.Id, cancellationToken);
                if (major is null)
                {
                    return Result.Failure<Guid>(MajorErrors.NotFound(command.Id));
                }

                bool isCodeMajorExisted = await query
                    .AnyAsync(m => m.Code == command.Code && m.Id != command.Id, cancellationToken);
                if (isCodeMajorExisted)
                {
                    return Result.Failure<Guid>(MajorErrors.NotFound(command.Id));
                }

                major.Code = command.Code;
                major.Name = command.Name;

                var introductionBlog = await dbContext.IntroductionBlogs
                    .FirstOrDefaultAsync(b => b.TargetId == major.Id 
                                              && b.IntroductionType == IntroductionType.Major, cancellationToken);
                if (introductionBlog is null)
                {
                    return Result.Failure<Guid>(IntroductionBlogErrors.NotFound(major.Id));
                }

                major.Raise(new MajorUpdatedDomainEvent(major.Id, introductionBlog.Id, command.Title, command.Content));
                major.Raise(new MajorCreatedDomainEvent(major.UniversityId));
                dbContext.Majors.Update(major);
                await dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
        }

        internal sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Id).NotNull().NotEmpty();
                RuleFor(c => c.Code).NotNull().NotEmpty();
                RuleFor(c => c.Name).NotNull().NotEmpty();
            }
        }
    }
}
