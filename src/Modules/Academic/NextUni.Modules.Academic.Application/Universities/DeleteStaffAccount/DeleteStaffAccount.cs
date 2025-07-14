using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Universities.DeleteStaffAccount;

public abstract class DeleteStaffAccount
{
    public record Command(
        Guid StaffId,
        Guid UniversityId
    ): ICommand;

    
    internal class Handler(
        IAcademicDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            
            var university = await dbContext.Universities.SingleOrDefaultAsync(u => u.Id == command.UniversityId);
            if (university is null)
            {
                return Result.Failure(UniversityErrors.NotFound(command.UniversityId));
            }

            university.StaffAccountId = null;
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
    
    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.StaffId).NotNull();
            RuleFor(x => x.UniversityId).NotNull();
        }
    }
}