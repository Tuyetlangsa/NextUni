using FluentValidation;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Application.Subjects.HideSubject;

public abstract class HideSubject
{
    public record Command(Guid Id): ICommand;
    
    internal sealed class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
           Subject? subject  = await dbContext.Subjects.FindAsync(command.Id, cancellationToken);
           
           if (subject is null)
           {
               return Result.Failure(SubjectErrors.NotFound(command.Id));
           }
           
           subject.IsDeleted = true;
           await dbContext.SaveChangesAsync(cancellationToken);
           
           return Result.Success();
        }
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();
        }
    }
}