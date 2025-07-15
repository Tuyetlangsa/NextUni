using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Api;

namespace NextUni.Modules.Contents.Api.UniversityCounsellingArticles
{
    public class HideCounsellingArticleEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("university-counselling-articles/hide-unhide/{id}", async ([FromRoute] Guid id, ISender sender) =>
                {
                    Result<Guid> result = await sender.Send(
                        new Application.HideCounsellingArticle.HideCounsellingArticle.Command(id));
                    return result.MatchCreated(id => $"/university-counselling-articles/hide-unhide/{id}");
                })
                .RequireAuthorization(Permissions.HideArticle)
                .Produces<ApiResult<object>>()
                .WithTags(Tags.UniversityContent);
        }
    }
}