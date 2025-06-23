namespace NextUni.Common.Application.User;

public interface ICurrentUser
{
    Guid UserId { get; }
    HashSet<string> GetPermissions();
}
