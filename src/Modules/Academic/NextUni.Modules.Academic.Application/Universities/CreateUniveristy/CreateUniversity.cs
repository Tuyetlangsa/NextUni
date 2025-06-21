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
            bool isExisted = await dbContext.Universities.AnyAsync(x => x.Code == command.Code, cancellationToken);

            if (isExisted)
            {
                return Result.Failure<Guid>(UniversityErrors.SubjectExisted(command.Code));
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
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(c => c.Name).NotNull().NotEmpty();
            RuleFor(c => c.Region).NotNull().NotEmpty().IsInEnum();
            RuleFor(c => c.Address).NotNull().NotEmpty();
            RuleFor(c => c.Email).NotNull().NotEmpty();
            RuleFor(c => c.WebsiteUrl).NotNull().NotEmpty();
            RuleFor(c => c.FacebookUrl).NotNull().NotEmpty();
            RuleFor(c => c.Title).NotNull().NotEmpty();
            RuleFor(c => c.Content).NotNull().NotEmpty();
            RuleFor(c => c.Type).NotNull().NotEmpty().IsInEnum();
        }
    }
}