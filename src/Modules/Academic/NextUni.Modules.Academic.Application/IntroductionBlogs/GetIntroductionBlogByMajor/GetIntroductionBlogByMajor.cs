using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;

namespace NextUni.Modules.Academic.Application.IntroductionBlogs.GetIntroductionBlogByMajor;


public abstract class GetIntroductionBlogByMajor
{
    public record Query(Guid MajorId) : IQuery<ResponseItem>;
    
    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, ResponseItem>
    {
        public async Task<Result<ResponseItem>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.IntroductionBlogs.AsNoTracking().AsQueryable();
      
            var blog = await query
                .FirstOrDefaultAsync(u => request.MajorId == u.TargetId 
                                          && u.IntroductionType == IntroductionType.Major , cancellationToken);

            var response = new ResponseItem(blog?.Title ?? "", blog?.Content ?? "");

            return Result.Success(response);
        }
    }

    public record ResponseItem(
        string Title,
        string Content); 

}