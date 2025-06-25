using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Domain;
using NextUni.Modules.Contents.Application.Abstractions.Data;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Application.GetCounsellingArticlesByUniversity;

public abstract class GetCounsellingArticlesByUniversity
{
    public record Query(
        Guid UniversityId, 
        int PageNumber, 
        int PageSize, 
        QueryStatus Status, 
        bool IsAdmin) : IQuery<Page<Response>>, IPageable;

    public enum QueryStatus
    {
        All,
        Published,
        Draft,
        Pending,
    }
    
    internal sealed class Handler(IContentDbContext dbContext) : IQueryHandler<Query, Page<Response>>
    {
        public async Task<Result<Page<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.CounsellingArticles
                .Where(c => c.UniversityId == request.UniversityId)
                .OrderByDescending(c => c.PublishAt)
                .AsQueryable();

            query = request.Status switch
            {
                QueryStatus.All => query,
                QueryStatus.Published => query.Where(x => x.Status == CounsellingArticleStatus.Published),
                QueryStatus.Draft => query.Where(x => x.Status == CounsellingArticleStatus.Draft),
                QueryStatus.Pending => query.Where(x => x.Status == CounsellingArticleStatus.Pending),
            };
            
            if (!request.IsAdmin)
            {
                query = query.Where(x => x.Status == CounsellingArticleStatus.Published);
            }
            else
            {
                query = query.Where(x => x.Status != CounsellingArticleStatus.Draft);
            }

            int count = await query.CountAsync(cancellationToken);
            var result = await query.Applypagination(request.PageNumber, request.PageSize)
                .Select(c => new Response(
                    c.Id,
                    c.Title,
                    c.Content,
                    c.Status))
                .ToListAsync(cancellationToken);

            return new Page<Response>(result, count, request.PageNumber, request.PageSize);
        }
    }

    public record Response(
        Guid Id,
        string Title,
        string Content,
        CounsellingArticleStatus Status);

    internal class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.UniversityId).NotEmpty().WithMessage("University ID cannot be empty.");
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}