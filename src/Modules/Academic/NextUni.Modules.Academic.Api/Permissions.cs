namespace NextUni.Modules.Academic.Api;

internal static class Permissions
{
    internal const string CreateUniversity = "university:create";
    internal const string GetAdministrativeUniversities = "university:read:administrative";
    internal const string ModifyUniversity = "university:modify";
    internal const string CreateSubject = "subject:create";
    internal const string GetAdministrativeSubjects = "subject:read:administrative";
    internal const string ModifySubject = "subject:modify";
    internal const string CreateSubjectGroup = "subjectgroup:create";
    internal const string GetAdministrativeSubjectGroups = "subjectgroup:read:administrative";
    internal const string ModifySubjectGroup = "subjectgroup:modify";
    internal const string CreateMajor = "major:create";
    internal const string GetAdministrativeMajors = "major:read:administrative";
    internal const string ModifyMajor = "major:modify";
    internal const string CreateAdmissionScoreByYear = "admissionscore:create";
    internal const string CreateMajorGroup = "majorgroup:create";
}
