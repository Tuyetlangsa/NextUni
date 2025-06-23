using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Universities;

public class UniversityErrors
{
    public static Error NotFound(Guid universityId) =>
        Error.NotFound("University.NotFound", $"The university with the identifier {universityId} not found");
    
    public static Error UniversityExisted(string universityCode) =>
        Error.Conflict("University.Existed", $"The university with the code {universityCode} already exists");
}