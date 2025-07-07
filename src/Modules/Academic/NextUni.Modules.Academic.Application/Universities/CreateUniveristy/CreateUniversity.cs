using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Universities.CreateUniveristy;

public abstract class CreateUniversity
{
    public record Command(
        string Code, 
        string Name, 
        Region Region, 
        UniversityType Type, 
        string Address, 
        string Email, 
        string WebsiteUrl, 
        string FacebookUrl,
        string Title,
        string Content) : ICommand<Guid>;

    
    internal class Handler(
        IAcademicDbContext dbContext) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            // code is unique
            bool isExisted = await dbContext.Universities.IgnoreQueryFilters().AnyAsync(x => x.Code == command.Code, cancellationToken);

            if (isExisted)
            {
                return Result.Failure<Guid>(UniversityErrors.UniversityExisted(command.Code));
            }

            var university = new University()
            {
                Id = Guid.NewGuid(),
                Code = command.Code,
                Name = command.Name,
                Region = command.Region,
                UniversityType = command.Type,
                Address = command.Address,
                Email = command.Email,
                WebsiteUrl = command.WebsiteUrl,
                FacebookUrl = command.FacebookUrl,
            };
            
            university.Raise(new UniversityCreatedDomainEvent(university.Id, command.Title, command.Content));
            
            dbContext.Universities.Add(university);
            await dbContext.SaveChangesAsync(cancellationToken);

            return university.Id;
        }
    }
    
    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(c => c.Name).NotNull().NotEmpty().MaximumLength(255);
            RuleFor(c => c.Region).NotNull().IsInEnum();
            RuleFor(c => c.Address).NotNull().NotEmpty().MaximumLength(500);
            RuleFor(c => c.Email).NotNull().NotEmpty().MaximumLength(255);
            RuleFor(c => c.WebsiteUrl).NotNull().NotEmpty().MaximumLength(255);
            RuleFor(c => c.FacebookUrl).NotNull().NotEmpty().MaximumLength(255);
            RuleFor(c => c.Title).NotNull().NotEmpty().MaximumLength(500);
            RuleFor(c => c.Content).NotNull().NotEmpty();
            RuleFor(c => c.Type).NotNull().IsInEnum();
        }
    }
}