namespace NextUni.Modules.Academic.PublicApi;

public interface IUniversityApi
{
    Task<bool> CheckUniversityExistsAsync(Guid universityId);
    Task<Guid> GetStaffIdByUniversityIdAsync(Guid universityId, CancellationToken cancellationToken = default);
}


    