using NextUni.Common.Domain;

namespace NextUni.Modules.Contents.Domain.IntroductionBlogs;

public class IntroductionBlog : Entity
{
    public Guid Id { get; set; }
    public IntroductionType IntroductionType { get; set; }
    public Guid? UniversityId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime PublishedAt { get; set; }
}