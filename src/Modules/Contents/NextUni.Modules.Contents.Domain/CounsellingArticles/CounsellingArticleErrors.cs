using NextUni.Common.Domain;

namespace NextUni.Modules.Contents.Domain.CounsellingArticles
{
    public class CounsellingArticleErrors
    {
        public static Error NotFound(Guid counsellingArticleId) =>
        Error.NotFound("CounsellingArticle.NotFound", $"The counselling article with the identifier {counsellingArticleId} not found");

        public static Error IncorrectStatus(Guid counsellingArticleId, CounsellingArticleStatus status) =>
            Error.Conflict("CounsellingArticle.IncorrectStatus", $"The counselling article status " +
                           $"with the identifier {counsellingArticleId} is {status}");
    }
}