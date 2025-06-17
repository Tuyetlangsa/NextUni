namespace NextUni.Modules.Contents.Application.Abstractions.Data;

public interface IContentDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}