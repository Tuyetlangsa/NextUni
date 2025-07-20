
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Contents.Application.Abstractions.Data;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Application.UpdateCounsellingArticle;


public abstract class UpdateCounsellingArticle
{
    public record Command(
        Guid ArticleId,
        string Title,
        string Content) : ICommand;
    
    internal sealed class Handler(
        IContentDbContext dbContext, 
        IDateTimeProvider dateTimeProvider 
        ) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var article = await dbContext.CounsellingArticles.
                FirstOrDefaultAsync(x => x.Id == request.ArticleId, cancellationToken);

            if (article == null)
            {
                return Result.Failure(CounsellingArticleErrors.NotFound(request.ArticleId));
            }
            article.Title = request.Title;
            article.Content = request.Content;
            dbContext.CounsellingArticles.Update(article);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
    
    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
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