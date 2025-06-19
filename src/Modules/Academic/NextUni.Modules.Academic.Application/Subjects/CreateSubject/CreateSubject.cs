using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Application.Subjects.CreateSubject;

public abstract class CreateSubject
{
    public record Command(string Name) : ICommand<Guid>;
    
    internal sealed class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            bool isExisted = await dbContext.Subjects.AnyAsync(s => s.Name == command.Name, cancellationToken);
            if (isExisted)
            {
                return Result.Failure<Guid>(SubjectErrors.SubjectExisted(command.Name));
            }

            Subject subject = new Subject
            {
                Id = Guid.NewGuid(),
                Name = command.Name
            };
            dbContext.Subjects.Add(subject);
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