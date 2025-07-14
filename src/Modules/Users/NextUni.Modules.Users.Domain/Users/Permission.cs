namespace NextUni.Modules.Users.Domain.Users;

public sealed class Permission
{
    public static readonly Permission CreateUniversity = new("university:create");
    public static readonly Permission GetAdministrativeUniversities = new("university:read:administrative");
    public static readonly Permission ModifyUniversity = new("university:modify");
    public static readonly Permission CreateMajor = new("major:create");
    public static readonly Permission GetAdministrativeMajors = new("major:read:administrative");
    public static readonly Permission ModifyMajor = new("major:modify");
    public static readonly Permission CreateMajorGroup = new("majorgroup:create");
    public static readonly Permission CreateSubject = new("subject:create");
    public static readonly Permission GetAdministrativeSubjects = new("subject:read:administrative");
    public static readonly Permission ModifySubject = new("subject:modify");
    public static readonly Permission CreateSubjectGroup = new("subjectgroup:create");
    public static readonly Permission GetAdministrativeSubjectGroups = new("subjectgroup:read:administrative");
    public static readonly Permission ModifySubjectGroup = new("subjectgroup:modify");
    public static readonly Permission CreateMasterArticle = new("masterarticle:create");
    public static readonly Permission GetAdministrativeMasterArticles = new("masterarticle:read:administrative");
    public static readonly Permission ModifyMasterArticle = new("masterarticle:modify");
    public static readonly Permission CreateUniversityArticle = new("universityarticle:create");
    public static readonly Permission GetAdministrativeUniversityArticles = new("universityarticle:read:administrative");
    public static readonly Permission ModifyUniversityArticle = new("universityarticle:modify");
    public static readonly Permission HideArticle = new("article:hide");
    public static readonly Permission ProcessUniversityArticle = new("universityarticle:process");
    public static readonly Permission CreateEvent = new("event:create");
    public static readonly Permission GetStaffEvent = new("event:read:staff");
    public static readonly Permission GetAdministrativeEvents = new("event:read:administrative");
    public static readonly Permission ProcessEvents = new("event:process");
    public static readonly Permission RegisterEvent = new("event:register");
    public static readonly Permission CancelEventRegistration = new("event:cancelregistration");
    public static readonly Permission CreateStaffAccount = new("staffaccount:create");
    public static readonly Permission DeleteStaffAccount = new("deletestaffaccount:delete");
    public static readonly Permission GetStaffAccountByUniversity = new("staffaccount:read");
    public static readonly Permission CreateAdmissionScoreByYear = new("admissionscore:create");
    public static readonly Permission GetStaffUniversityArticles = new("universityarticle:read:staff");


    
    public Permission(string code)
    {
        Code = code;
    }

    public string Code { get; }
}
