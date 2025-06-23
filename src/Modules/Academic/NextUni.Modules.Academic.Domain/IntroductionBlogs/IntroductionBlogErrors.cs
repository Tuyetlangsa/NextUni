using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.IntroductionBlogs
{
    public class IntroductionBlogErrors
    {
        public static Error NotFound(Guid majorId) =>
        Error.NotFound("IntroductionBlog.NotFound", $"The introduction of this major with the identifier {majorId} not found");
    }
}
