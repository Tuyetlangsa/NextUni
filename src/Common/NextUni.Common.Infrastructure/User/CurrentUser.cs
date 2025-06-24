using Microsoft.AspNetCore.Http;
using NextUni.Common.Application.User;
using NextUni.Common.Infrastructure.Authentication;

namespace NextUni.Common.Infrastructure.User;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Guid UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User context is unavailable");
    
    public HashSet<string> GetPermissions()
    {
        HashSet<string> permissionClaims = _httpContextAccessor
                                                  .HttpContext?
                                                  .User.GetPermissions()!;

        return permissionClaims;
    }
}