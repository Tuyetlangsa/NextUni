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
    public class SubmitUniversityCounsellingArticleToPublish : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("university-counselling-articles/submit/{id}", async ([FromRoute] Guid id, ISender sender) =>
                {
                    Result<Guid> result = await sender.Send(
                        new Application.SubmitUniversityCounsellingArticle.SubmitUniversityCounsellingArticle.Command(id));
                    return result.MatchCreated(id => $"/university-counselling-articles/submit/{id}");
                })
                .WithTags(Tags.UniversityContent);
        }
    }
}