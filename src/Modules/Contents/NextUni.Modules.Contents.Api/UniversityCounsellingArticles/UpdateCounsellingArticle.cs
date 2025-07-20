using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Contents.Api.UniversityCounsellingArticles;

public class UpdateCounsellingArticleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("university-counselling-articles", async ([FromBody] Request request, ISender sender) =>
            {
                Result result = await sender.Send(
                    new Application.UpdateCounsellingArticle.UpdateCounsellingArticle.Command(
                        request.ArticleId,
                        request.Title,
                        request.Content));

                return result.MatchOk();
                
            })
            .RequireAuthorization(Permissions.ProcessUniversityArticle)
            .Produces<ApiResult<object>>()
            .WithTags(Tags.UniversityContent);
    }

    public class Request
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    };

}