using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Application.Subjects.UpdateSubject
{
    public abstract class UpdateSubject
    {
        public record Command(Guid Id, string Name) : ICommand<Guid>;

        internal class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command, Guid>
        {
            public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
            {
                bool isExisted = await dbContext.Subjects.AnyAsync(s => s.Name == command.Name && s.Id != command.Id, cancellationToken);
                if (isExisted)
                {
                    return Result.Failure<Guid>(SubjectErrors.SubjectExisted(command.Name));
                }

                var subject = await dbContext.Subjects
                    .FirstOrDefaultAsync(s => s.Id == command.Id, cancellationToken);
                if (subject is null)
                {
                    return Result.Failure<Guid>(SubjectErrors.NotFound(command.Id));
                }
                subject.Name = command.Name;
                dbContext.Subjects.Update(subject);
                await dbContext.SaveChangesAsync(cancellationToken);

                return Result.Success(subject.Id);
            }
        }

        internal sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Name).NotNull().NotEmpty();
            }
        }
    }
}