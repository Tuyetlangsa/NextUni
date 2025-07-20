using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Universities.UpdateUniversity;

public abstract class UpdateUniversity
{
    public record Command(
        Guid Id,
        string Code, 
        string Name, 
        Region Region, 
        UniversityType Type, 
        string Address, 
        string Email, 
        string WebsiteUrl, 
        string FacebookUrl) : ICommand;
    
    
    internal sealed class Handler(IAcademicDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {

            var university = await dbContext.Universities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (university is null)
            {
                return Result.Failure(UniversityErrors.NotFound(request.Id));
            }

            if (university.Code != request.Code)
            {
                bool isExisted = await dbContext.Universities.AnyAsync(x => x.Code == request.Code, cancellationToken);
                if (isExisted)
                {
                    return Result.Failure(UniversityErrors.UniversityExisted(request.Code));
                }
            }
            
            //update new information for the old university
            university.Code = request.Code;
            university.Name = request.Name;
            university.Region = request.Region;
            university.UniversityType = request.Type;
            university.Address = request.Address;
            university.Email = request.Email;
            university.WebsiteUrl = request.WebsiteUrl;
            university.FacebookUrl = request.FacebookUrl;
            university.Raise(new UniversityCreatedDomainEvent(university.Id));
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success(); 
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
            RuleFor(c => c.Type).NotNull().IsInEnum();
        }
    }
    
    
}