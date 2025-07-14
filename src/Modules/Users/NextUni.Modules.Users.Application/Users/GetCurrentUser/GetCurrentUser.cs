using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.User;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.PublicApi;
using NextUni.Modules.Users.Application.Abstractions.Data;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Application.Users.GetCurrentUser;

public abstract class GetCurrentUser
{
    public record Query : IQuery<CurrentUser>;
    public record CurrentUser
    (
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Role,
        Guid? UniversityId
    );
    
    internal sealed class Handler(IUserDbContext dbContext, ICurrentUser currentUser, IUniversityApi publicApi) : IQueryHandler<Query, CurrentUser>
    {
        public async Task<Result<CurrentUser>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;
            
            var user = await dbContext.Users.Include(u => u.Roles).SingleAsync(u => u.Id == userId, cancellationToken);
            Guid? universityId = null;
            if (user.Roles.First().Name == Role.Staff.Name)
            {
                 universityId = await publicApi.GetUniversityIdByStaffIdAsync(user.Id, cancellationToken);
            }
            return new CurrentUser(
                userId, 
                user.Email, 
                user.FirstName, 
                user.LastName, 
                user.PhoneNumber,
                user.Roles.First().Name,
                universityId
                );
        }
    }
}