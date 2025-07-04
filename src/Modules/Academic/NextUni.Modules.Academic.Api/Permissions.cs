namespace NextUni.Modules.Academic.Api;

internal static class Permissions
{
    internal const string CreateUniversity = "university:create";
    internal const string GetUniversities = "university:read";
    internal const string GetAdministrativeUniversities = "university:read:administrative";
    internal const string ModifyUniversity = "university:modify";
    internal const string CreateSubject = "subject:create";
    internal const string GetSubjects = "subject:read";
    internal const string GetAdministrativeSubjects = "subject:read:administrative";
    internal const string ModifySubject = "subject:modify";
    internal const string CreateSubjectGroup = "subjectgroup:create";
    internal const string GetSubjectGroups = "subjectgroup:read";
    internal const string GetAdministrativeSubjectGroups = "subjectgroup:read:administrative";
    internal const string ModifySubjectGroup = "subjectgroup:modify";
    internal const string CreateMajor = "major:create";
    internal const string GetMajors = "major:read";
    internal const string GetAdministrativeMajors = "major:read:administrative";
    internal const string ModifyMajor = "major:modify";
}
