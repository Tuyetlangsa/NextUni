using NextUni.Common.Domain;

namespace NextUni.Modules.Contents.Domain.CounsellingArticles;

public class CounsellingArticle : Entity
{
    public Guid Id { get; set; }
    
    public Guid? UniversityId { get; set; } 

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime PublishAt { get; set; }

    public CounsellingArticleType Type { get; set; }
    
    public CounsellingArticleStatus Status { get; set; }
}