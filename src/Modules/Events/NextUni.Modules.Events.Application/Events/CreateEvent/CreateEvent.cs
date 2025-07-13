using FluentValidation;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.PublicApi;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.Events.CreateEvent;

public abstract class CreateEvent
{
    public record Command(
        string Name,
        DateOnly StartDate,
        string Address,
        bool IsOnline,
        Guid UniversityId,
        string Title,
        string Content
        ) : ICommand<Guid>;
    
    internal sealed class Handler(IEventDbContext dbContext, IUniversityApi universityApi) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            bool isExisted = await universityApi.CheckUniversityExistsAsync(request.UniversityId);

            if (!isExisted)
            {
                return Result.Failure<Guid>(
                    new Error(
                        "University.NotExisted",
                        $"The University with Id {request.UniversityId} does not exist.",
                        ErrorType.NotFound));
            }

            var eventEntity = new Domain.Events.Event()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                StartDate = request.StartDate,
                Address = request.Address,
                IsOnline = request.IsOnline,
                UniversityId = request.UniversityId,
                Status = EventStatus.Pending
            };
            eventEntity.Raise(new EventCreatedDomainEvent(eventEntity.Id, request.Title, request.Content));
            
            dbContext.Events.Add(eventEntity);
            await dbContext.SaveChangesAsync(cancellationToken);
            return eventEntity.Id;
        }
    }
    
    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(x => x.StartDate)
                .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .NotEmpty()
                .WithMessage("StartDate is required.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.");

            RuleFor(x => x.UniversityId)
                .NotEmpty()
                .WithMessage("UniversityId is required.");
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(500)
                .WithMessage("Title must not exceed 500 characters.");
            
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.");
        }
    }
}