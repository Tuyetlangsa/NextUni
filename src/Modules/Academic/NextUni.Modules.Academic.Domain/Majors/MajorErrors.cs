using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors;

public class MajorErrors
{
    public static Error NotFound(Guid majorId) =>
        Error.NotFound("Major.NotFound", $"The major with the identifier {majorId} not found");
    
    public static Error MajorExisted(string majorCode) =>
        Error.Conflict("Major.Existed", $"The major with the name {majorCode} already exists");

}