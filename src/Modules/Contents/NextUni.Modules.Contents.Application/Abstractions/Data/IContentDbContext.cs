using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Application.Abstractions.Data;

public interface IContentDbContext
{
    DbSet<CounsellingArticle>  CounsellingArticles { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}