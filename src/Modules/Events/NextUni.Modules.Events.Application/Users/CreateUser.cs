using FluentValidation;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.Users;

public abstract class CreateUser
{
    public sealed record Command(Guid CustomerId, string Email, string FirstName, string LastName, string PhoneNumber)
        : ICommand;
    
    internal sealed class Handler(IEventDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneNumber = command.PhoneNumber,
            };
             dbContext.Users.Add(user);
             await dbContext.SaveChangesAsync(cancellationToken);
             return Result.Success();
        }
    }

    internal sealed class CreateCustomerCommandValidator : AbstractValidator<Command>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.FirstName).NotEmpty();
            RuleFor(c => c.LastName).NotEmpty();
        }
    }

}