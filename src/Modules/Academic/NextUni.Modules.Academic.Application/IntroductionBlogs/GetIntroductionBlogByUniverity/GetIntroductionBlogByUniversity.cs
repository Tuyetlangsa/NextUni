using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;

namespace NextUni.Modules.Academic.Application.IntroductionBlogs.GetIntroductionBlogByUniverity;


public abstract class GetIntroductionBlogByUniversity
{
    public record Query(Guid UniversityId) : IQuery<ResponseItem>;
    
    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, ResponseItem>
    {
        public async Task<Result<ResponseItem>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.IntroductionBlogs.AsNoTracking().AsQueryable();
      
            var blog = await query
                .FirstOrDefaultAsync(u => request.UniversityId == u.TargetId 
                                          && u.IntroductionType == IntroductionType.University , cancellationToken);

            var response = new ResponseItem(blog?.Title ?? "", blog?.Content ?? "");

            return Result.Success(response);
        }
    }

    public record ResponseItem(
        string Title,
        string Content); 

}