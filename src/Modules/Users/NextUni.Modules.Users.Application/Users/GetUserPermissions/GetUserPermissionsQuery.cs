using NextUni.Common.Application.Authorization;
using NextUni.Common.Application.Messaging;

namespace NextUni.Modules.Users.Application.Users.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;
