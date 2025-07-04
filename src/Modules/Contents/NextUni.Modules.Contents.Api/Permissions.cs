namespace NextUni.Modules.Contents.Api;

internal static class Permissions
{
    internal const string CreateMasterArticle = "masterarticle:create";
    internal const string GetMasterArticles = "masterarticle:read";
    internal const string GetAdministrativeMasterArticles = "masterarticle:read:administrative";
    internal const string ModifyMasterArticle = "masterarticle:modify";
    internal const string CreateUniversityArticle = "universityarticle:create";
    internal const string GetUniversityArticles = "universityarticle:read";
    internal const string GetAdministrativeUniversityArticles = "universityarticle:read:administrative";
    internal const string ModifyUniversityArticle = "universityarticle:modify";
    internal const string HideArticle = "article:hide";
    internal const string ProcessUniversityArticle = "universityarticle:process";
}
