using System.Data.Common;
using Dapper;
using NextUni.Common.Application.Data;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Users.Application.Abstractions.Data;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Application.Users.GetUser;

internal sealed class GetUserQueryHandler
    (IUserDbContext dbContext)
    : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle
        (GetUserQuery request, 
        CancellationToken cancellationToken)
    {

        User? user = await dbContext.Users.FindAsync(request.UserId, cancellationToken);
        
        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(request.UserId));
        }

        return new UserResponse
        (
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber
        );
    }
}
