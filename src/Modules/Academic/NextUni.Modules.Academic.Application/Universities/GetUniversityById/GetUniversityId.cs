using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Universities.GetUniversityById;

public abstract class GetUniversityId
{
    public record Query(Guid UniversityId, bool IsAdmin) : IQuery<ResponseItem>;
    
    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, ResponseItem>
    {
        public async Task<Result<ResponseItem>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.Universities.AsNoTracking().AsQueryable();
            if (request.IsAdmin)
            {
                query.IgnoreQueryFilters();
            }
            var university = await query
                .FirstOrDefaultAsync(u => request.UniversityId == u.Id , cancellationToken);

            if (university is null)
            {
                return Result.Failure<ResponseItem>(UniversityErrors.NotFound(request.UniversityId));
            }

            var response = new ResponseItem(
                university.Id,
                university.Code,
                university.Name,
                university.Region,
                university.UniversityType,
                university.Address,
                university.Email,
                university.WebsiteUrl,
                university.FacebookUrl,
                university.IsDeleted);

            return Result.Success(response);
        }
    }

    public record ResponseItem(
        Guid Id,
        string Code,
        string Name,
        Region Region,
        UniversityType UniversityType,
        string Address,
        string Email,
        string WebsiteUrl,
        string FacebookUrl,
        bool IsDeleted); 

}