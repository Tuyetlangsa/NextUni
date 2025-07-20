using System.Windows.Input;
using FluentValidation;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.PublicApi;
using NextUni.Modules.Contents.Application.Abstractions.Data;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Application.CreateUniversityCounsellingArticle;

public abstract class CreateUniversityCounsellingArticle
{
    public record Command(
        Guid UniversityId,
        string Title,
        string Content) : ICommand<Guid>;
    
    internal sealed class Handler(
        IContentDbContext dbContext, 
        IDateTimeProvider dateTimeProvider, 
        IUniversityApi universityApi) : ICommandHandler<Command, Guid>
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

            var article = new CounsellingArticle()
            {
                Id = Guid.NewGuid(),
                UniversityId = request.UniversityId,
                Title = request.Title,
                Content = request.Content,
                Status = CounsellingArticleStatus.Draft,
                PublishAt = dateTimeProvider.UtcNow,
                Type = CounsellingArticleType.University,
            };
            
            dbContext.CounsellingArticles.Add(article);
            await dbContext.SaveChangesAsync(cancellationToken);
            return article.Id;
        }
    }
    
    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UniversityId)
                .NotEmpty()
                .WithMessage("UniversityId is required.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.");

            RuleFor(x => x.Title)
                .MaximumLength(500)
                .WithMessage("Title must not exceed 500 characters.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.");
        }
    }
    
}