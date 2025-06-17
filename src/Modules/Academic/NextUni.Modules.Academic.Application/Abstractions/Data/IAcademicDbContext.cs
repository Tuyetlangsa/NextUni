namespace NextUni.Modules.Academic.Application.Abstractions.Data;

public interface IAcademicDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}