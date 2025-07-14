using System.Net;
using Microsoft.Extensions.Logging;
using NextUni.Common.Domain;
using NextUni.Modules.Users.Application.Abstractions.Identity;

namespace NextUni.Modules.Users.Infrastructure.Identity;

internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger)
    : IIdentityProviderService
{
    private const string PasswordCredentialType = "password";

    // POST /admin/realms/{realm}/users
    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.Email,
            user.Email,
            user.FirstName,
            user.LastName,
            true,
            true,
            [new CredentialRepresentation(PasswordCredentialType, user.Password, false)]);

        try
        {
            string identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);

            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(exception, "User registration failed");

            return Result.Failure<string>(IdentityProviderErrors.EmailIsNotUnique);
        }
    }

    public async Task<Result<IIdentityProviderService.TokenResponse>> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await  keyCloakClient.LoginUserAsync(email, password, cancellationToken);

            return response;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Unauthorized)
        {
            logger.LogError(exception, "User registration failed");

            return Result.Failure<IIdentityProviderService.TokenResponse>(new Error("Identity.LoginFailed", "Login failed due to incorrect credentials.", ErrorType.UnAuthorized));
        }
        
    }
    
    
    public async Task<Result> ResetPasswordAsync(string userId, string newPassword, CancellationToken cancellationToken = default)
    {
        try
        {
            await keyCloakClient.ResetPasswordAsync(userId, newPassword, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Reset password failed for userId: {UserId}", userId);
            return Result.Failure(new Error("Identity.ResetPasswordFailed", "Unable to reset password.", ErrorType.Conflict));
        }
    }
    
    public async Task<Result> DeleteUserAsync(string identityId, CancellationToken cancellationToken = default)
    {
        
        try
        {
            await keyCloakClient.DeleteUserAsync(identityId, cancellationToken);
            return Result.Success();
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogError(exception, "User deletion failed for userId: {UserId}", identityId);
            return Result.Failure(new Error("Identity.UserNotFound", "User not found.", ErrorType.NotFound));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User deletion failed for userId: {UserId}", identityId);
            return Result.Failure(new Error("Identity.UserDeletionFailed", "Unable to delete user.", ErrorType.Conflict));
        }
        
    }
    
}