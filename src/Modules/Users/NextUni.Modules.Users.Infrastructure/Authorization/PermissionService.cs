using MediatR;
using NextUni.Common.Application.Authorization;
using NextUni.Common.Domain;
using NextUni.Modules.Users.Application.Users.GetUserPermissions;

namespace NextUni.Modules.Users.Infrastructure.Authorization;

internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        return await sender.Send(new GetUserPermissionsQuery(identityId));
    }
}
