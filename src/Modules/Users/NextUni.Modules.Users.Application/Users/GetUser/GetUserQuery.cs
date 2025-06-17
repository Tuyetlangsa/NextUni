using NextUni.Common.Application.Messaging;

namespace NextUni.Modules.Users.Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;
