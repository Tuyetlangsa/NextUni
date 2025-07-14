using FluentValidation;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Users.Application.Abstractions.Identity;

namespace NextUni.Modules.Users.Application.Users.LoginUser;

public abstract class Login 
{
    public record Command(
        string Email,
        string Password) : ICommand<TokenResponse>;
    
    public record TokenResponse(string AccessToken, string RefreshToken);
    
    
    internal sealed class Handler(IIdentityProviderService indentityService) : ICommandHandler<Command, TokenResponse>
    {
        public async Task<Result<TokenResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var token =  await indentityService.LoginUserAsync(request.Email, request.Password, cancellationToken);
            if (token.IsFailure)
            {
                return Result.Failure<TokenResponse>(token.Error);
            }
            var response = new TokenResponse(token.Value.AccessToken, token.Value.RefreshToken);
            return response;
        }
    }
    
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Email).NotNull().NotEmpty();
            RuleFor(c => c.Password).NotNull().NotEmpty();
        }
    }
}