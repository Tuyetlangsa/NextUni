using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.User;
using NextUni.Common.Domain;
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
        Role Role
    );
    
    internal sealed class Handler(IUserDbContext dbContext, ICurrentUser currentUser) : IQueryHandler<Query, CurrentUser>
    {
        public async Task<Result<CurrentUser>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;
            
            var user = await dbContext.Users.SingleAsync(u => u.Id == userId, cancellationToken); 
            return new CurrentUser(
                userId, 
                user.Email, 
                user.FirstName, 
                user.LastName, 
                user.PhoneNumber,
                user.Roles.First());
        }
    }
}