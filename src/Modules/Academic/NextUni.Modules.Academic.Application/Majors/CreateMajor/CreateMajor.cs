
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Majors;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Majors.CreateMajor;

public abstract class CreateMajor
{
    public record Command(
        string Code,
        string Name,
        Guid UniversityId,
        string Title,
        string Content) : ICommand<Guid>;


    internal class Handler(
        IAcademicDbContext dbContext) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            bool isUniversityExisted = await dbContext.Universities
                .AnyAsync(u => u.Id == command.UniversityId, cancellationToken);

            if (!isUniversityExisted)
            {
                return Result.Failure<Guid>(UniversityErrors.NotFound(command.UniversityId));
            }
            
            bool isMajorExisted = await dbContext.Majors
                .IgnoreQueryFilters()
                .AnyAsync(m => m.Code == command.Code, cancellationToken);

            if (isMajorExisted)
            {
                return Result.Failure<Guid>(MajorErrors.MajorExisted(command.Code));
            }

            Major major = new()
            {
                Id = Guid.NewGuid(),
                Code = command.Code,
                Name = command.Name,
                UniversityId = command.UniversityId,
            };
            major.Raise(new MajorCreatedDomainEvent(major.Id, command.Title, command.Content));
            dbContext.Majors.Add(major);
            await dbContext.SaveChangesAsync(cancellationToken);
            return major.Id;
        }
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
           RuleFor(c => c.Code).NotNull().NotEmpty().MaximumLength(50);
           RuleFor(c => c.Name).NotNull().NotEmpty().MaximumLength(255);
           RuleFor(c => c.UniversityId).NotNull().NotEmpty();
        }
    }
}