using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Domain;
using NextUni.Modules.Contents.Application.Abstractions.Data;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Application.GetUniversityCounsellingArticles;

public abstract class GetUniversityCounsellingArticles 
{ 
    public record Query(
        int PageNumber, 
        int PageSize, 
        QueryStatus? Status,
        bool? IsAdmin,
        bool? IsStaff) : IQuery<Page<Response>>, IPageable;
    
    public enum QueryStatus
    {
        Published,
        Draft,
        Pending,
    }
    public record Response(
        Guid Id,
        string Title,
        string Content,
        CounsellingArticleStatus Status);
    
    internal sealed class Handler(IContentDbContext dbContext) : IQueryHandler<Query, Page<Response>>
    {
        public async Task<Result<Page<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.CounsellingArticles
                .OrderByDescending(c => c.PublishAt)
                .AsQueryable();

            if (request.IsAdmin.HasValue && request.IsAdmin.Value)
            {
                query = query.IgnoreQueryFilters();
                query = request.Status switch
                {
                    QueryStatus.Published => query.Where(x => x.Status == CounsellingArticleStatus.Published),
                    QueryStatus.Pending => query.Where(x => x.Status == CounsellingArticleStatus.Pending),
                };
            }

            if (request.IsStaff.HasValue && request.IsStaff.Value)
            {
                query = query.IgnoreQueryFilters();
                query = request.Status switch
                {
                    QueryStatus.Published => query.Where(x => x.Status == CounsellingArticleStatus.Published),
                    QueryStatus.Draft => query.Where(x => x.Status == CounsellingArticleStatus.Draft),
                    QueryStatus.Pending => query.Where(x => x.Status == CounsellingArticleStatus.Pending),
                };
            }
            
            int count = await query.CountAsync();
            var result = query.Applypagination(request.PageNumber, request.PageSize)
                .Select(c => new Response(
                    c.Id,
                    c.Title,
                    c.Content,
                    c.Status))
                .ToList();
            
            return new Page<Response>(result, count, request.PageNumber, request.PageSize);
        }
    }

    
    internal class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}