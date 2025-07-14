using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Infrastructure.Database;
using NextUni.Modules.Academic.PublicApi;

namespace NextUni.Modules.Academic.Infrastructure.PublicApi;

public sealed class UniversityApi(IAcademicDbContext dbContext) : IUniversityApi
{
    public async Task<bool> CheckUniversityExistsAsync(Guid universityId)
    {
        return await dbContext.Universities
            .AnyAsync(x => x.Id == universityId);
    }
    
    public async Task<Guid> GetStaffIdByUniversityIdAsync(Guid universityId, CancellationToken cancellationToken = default)
    {
        var university = await dbContext.Universities
            .FirstOrDefaultAsync(x => x.Id == universityId, cancellationToken);

        return university?.StaffAccountId ?? Guid.Empty;
    }

    public async Task<Guid?> GetUniversityIdByStaffIdAsync(Guid staffId, CancellationToken cancellationToken = default)
    {
        var university = await dbContext.Universities
            .FirstOrDefaultAsync(x => x.StaffAccountId == staffId, cancellationToken);

        return university?.Id ?? null;
    }
}