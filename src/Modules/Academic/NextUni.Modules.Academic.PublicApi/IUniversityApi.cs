namespace NextUni.Modules.Academic.PublicApi;

public interface IUniversityApi
{
    Task<bool> CheckUniversityExistsAsync(Guid universityId);
    Task<Guid> GetStaffIdByUniversityIdAsync(Guid universityId, CancellationToken cancellationToken = default);
    Task<Guid?> GetUniversityIdByStaffIdAsync(Guid staffId, CancellationToken cancellationToken = default);
    
}


    