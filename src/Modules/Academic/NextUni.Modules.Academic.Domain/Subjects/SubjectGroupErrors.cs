using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Subjects;

public class SubjectGroupErrors
{
    public static Error NotFound(Guid subjectGroupId) =>
        Error.NotFound("SubjectGroup.NotFound", $"The subject with the identifier {subjectGroupId} not found");
    
    public static Error SubjectExisted(string code) =>
        Error.Conflict("SubjectGroup.Existed", $"The subject with the code {code} already exists");

}