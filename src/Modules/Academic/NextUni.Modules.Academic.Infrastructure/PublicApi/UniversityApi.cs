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
}