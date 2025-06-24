using FluentValidation;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Contents.Application.Abstractions.Data;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Application.CreateMasterCounsellingArticle;

public abstract class CreateMasterCounsellingArticle
{
    public record Command(
        string Title,
        string Content) : ICommand<Guid>;
    
    internal sealed class Handler(IContentDbContext dbContext, IDateTimeProvider dateTimeProvider) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {

            var article = new CounsellingArticle()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                PublishAt = dateTimeProvider.UtcNow,
                Type = CounsellingArticleType.System,
                Status = CounsellingArticleStatus.Published
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