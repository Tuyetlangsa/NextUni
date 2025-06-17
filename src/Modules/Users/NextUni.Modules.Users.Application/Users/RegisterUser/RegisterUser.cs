using FluentValidation;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Users.Application.Abstractions.Data;
using NextUni.Modules.Users.Application.Abstractions.Identity;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Application.Users.RegisterUser;

public abstract class RegisterUser
{
    public sealed record Command(string Email, string Password, string FirstName, string LastName, string PhoneNumber)
        : ICommand<Guid>;
    internal sealed class Handler(
        IIdentityProviderService identityProviderService, 
        IUserDbContext dbContext) 
        : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            Result<string> identityId = await identityProviderService.RegisterUserAsync(
                new UserModel(command.Email, command.Password, command.FirstName, command.LastName), cancellationToken);
            if (identityId.IsFailure)
            {
                return Result.Failure<Guid>(identityId.Error);
            }

            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneNumber = command.PhoneNumber,
                IdentityId = identityId.Value,
            };
            var studentRole = dbContext.Roles.Attach(Role.Student);
            user.Roles.Add(studentRole.Entity);
            user.Raise(new UserRegisteredDomainEvent(user.Id));
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success(user.Id);
        }
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.FirstName).NotEmpty();
            RuleFor(c => c.LastName).NotEmpty();
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.Password).MinimumLength(6);
            RuleFor(c => c.PhoneNumber).MinimumLength(10);
        }
    }
    
}