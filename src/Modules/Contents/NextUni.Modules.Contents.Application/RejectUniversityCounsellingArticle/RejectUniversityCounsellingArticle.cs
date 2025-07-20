using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Contents.Application.Abstractions.Data;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Application.RejectUniversityCounsellingArticle
{
    public abstract class RejectUniversityCounsellingArticle
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

                if (article.Status != CounsellingArticleStatus.Pending)
                {
                    return Result.Failure<Guid>(CounsellingArticleErrors.IncorrectStatus(command.Id, article.Status));
                }

                article.Status = CounsellingArticleStatus.Draft;
                dbContext.CounsellingArticles.Update(article);
                await dbContext.SaveChangesAsync(cancellationToken);
                return article.Id;
            }
        }
    }
}
