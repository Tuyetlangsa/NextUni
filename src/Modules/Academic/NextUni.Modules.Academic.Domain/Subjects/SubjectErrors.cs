using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Subjects;

public class SubjectErrors
{
    public static Error NotFound(Guid subjectId) =>
        Error.NotFound("Subject.NotFound", $"The subject with the identifier {subjectId} not found");
    
    public static Error SubjectExisted(string subjectName) =>
        Error.Conflict("Subject.Existed", $"The subject with the name {subjectName} already exists");
    
}

