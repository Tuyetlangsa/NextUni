using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Contents.Domain.CounsellingArticles;
using NextUni.Modules.Contents.Domain.IntroductionBlogs;

namespace NextUni.Modules.Contents.Application.Abstractions.Data;

public interface IContentDbContext
{
    DbSet<CounsellingArticle>  CounsellingArticles { get; set; }
    DbSet<IntroductionBlog>   IntroductionBlogs { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}