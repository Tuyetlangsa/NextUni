namespace NextUni.Modules.Users.Domain.Users;

public sealed class Permission
{
    public static readonly Permission CreateUniversity = new("university:create");
    public static readonly Permission GetUniversities = new("university:read");
    public static readonly Permission GetAdministrativeUniversities = new("university:read:administrative");
    public static readonly Permission ModifyUniversity = new("university:modify");
    public static readonly Permission CreateMajor = new("major:create");
    public static readonly Permission GetMajors = new("major:read");
    public static readonly Permission GetAdministrativeMajors = new("major:read:administrative");
    public static readonly Permission ModifyMajor = new("major:modify");
    public static readonly Permission CreateMajorGroup = new("major:create");
    public static readonly Permission GetMajorGroups = new("major:read");
    public static readonly Permission GetAdministrativeMajorGroups = new("major:read:administrative");
    public static readonly Permission CreateSubject = new("subject:create");
    public static readonly Permission GetSubjects = new("subject:read");
    public static readonly Permission GetAdministrativeSubjects = new("subject:read:administrative");
    public static readonly Permission ModifySubject = new("subject:modify");
    public static readonly Permission CreateSubjectGroup = new("subjectgroup:create");
    public static readonly Permission GetSubjectGroups = new("subjectgroup:read");
    public static readonly Permission GetAdministrativeSubjectGroups = new("subjectgroup:read:administrative");
    public static readonly Permission ModifySubjectGroup = new("subjectgroup:modify");
    public static readonly Permission CreateMasterArticle = new("masterarticle:create");
    public static readonly Permission GetMasterArticles = new("masterarticle:read");
    public static readonly Permission GetAdministrativeMasterArticles = new("masterarticle:read:administrative");
    public static readonly Permission ModifyMasterArticle = new("masterarticle:modify");
    public static readonly Permission CreateUniversityArticle = new("universityarticle:create");
    public static readonly Permission GetUniversityArticles = new("universityarticle:read");
    public static readonly Permission GetAdministrativeUniversityArticles = new("universityarticle:read:administrative");
    public static readonly Permission ModifyUniversityArticle = new("universityarticle:modify");
    public static readonly Permission HideArticle = new("article:hide");
    public static readonly Permission ProcessUniversityArticle = new("universityarticle:process");

    
    
    public Permission(string code)
    {
        Code = code;
    }

    public string Code { get; }
}
