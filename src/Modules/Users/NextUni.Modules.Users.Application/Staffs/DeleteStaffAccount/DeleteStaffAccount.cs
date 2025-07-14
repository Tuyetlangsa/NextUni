using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.PublicApi;
using NextUni.Modules.Users.Application.Abstractions.Data;
using NextUni.Modules.Users.Application.Abstractions.Identity;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Application.Staffs.DeleteStaffAccount;

public abstract class DeleteStaffAccount
{
    public record Command(Guid UniversityId) : ICommand;
    
    internal sealed class Handler(IUserDbContext dbContext, IUniversityApi publicApi, IIdentityProviderService identityProviderService) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var staffId = await publicApi.GetStaffIdByUniversityIdAsync(request.UniversityId, cancellationToken);
            if (staffId == Guid.Empty)
            {
                return Result.Failure(new Error("Staff.NotExisted",
                    $"The Staff with UniversityId {request.UniversityId} does not exist.", ErrorType.NotFound));
            }
            
            var staff = await dbContext.Users.FirstOrDefaultAsync( u => u.Id == staffId,cancellationToken);
            if (staff is null)
            {
                return Result.Failure(new Error("Staff.NotExisted",
                    $"The Staff with Id {staffId} does not exist.", ErrorType.NotFound));
            }
            await identityProviderService.DeleteUserAsync(staff.IdentityId, cancellationToken);
            dbContext.Users.Remove(staff);
            staff.Raise(new StaffAccountDeletedDomainEvent(staffId, request.UniversityId));
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}