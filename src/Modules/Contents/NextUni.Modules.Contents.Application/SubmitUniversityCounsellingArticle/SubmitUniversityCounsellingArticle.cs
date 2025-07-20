using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Contents.Application.Abstractions.Data;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Application.SubmitUniversityCounsellingArticle
{
    public abstract class SubmitUniversityCounsellingArticle
    {
        public record Command(Guid Id) : ICommand<Guid>;

        internal sealed class Handler(
        IContentDbContext dbContext,
        IDateTimeProvider dateTimeProvider) : ICommandHandler<Command, Guid>
        {
            public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
            {
                var article = await dbContext.CounsellingArticles.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
                if (article is null)
                {
                    return Result.Failure<Guid>(CounsellingArticleErrors.NotFound(command.Id));
                }

                if(article.Status != CounsellingArticleStatus.Draft)
                {
                    return Result.Failure<Guid>(CounsellingArticleErrors.IncorrectStatus(command.Id, article.Status));
                }

                article.Status = CounsellingArticleStatus.Pending;
                dbContext.CounsellingArticles.Update(article);
                await dbContext.SaveChangesAsync(cancellationToken);
                return article.Id;
            }
        }
    }
}
