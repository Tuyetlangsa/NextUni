using NextUni.Modules.Users.Application.Abstractions.Identity;
using NextUni.Common.Domain;

namespace NextUni.Modules.Users.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
}
