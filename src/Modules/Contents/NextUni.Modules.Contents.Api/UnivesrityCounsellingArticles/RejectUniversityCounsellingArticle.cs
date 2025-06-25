using NextUni.Common.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Modules.Academic.Api;
using Microsoft.AspNetCore.Mvc;
using NextUni.Common.Api.Results;
using Microsoft.AspNetCore.Http;

namespace NextUni.Modules.Contents.Api.UnivesrityCounsellingArticles
{
    public class RejectUniversityCounsellingArticle : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("university-counselling-articles/reject/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.ApproveUniversityCounsellingArticle.ApproveUniversityCounsellingArticle.Command(id));
                return result.MatchCreated(id => $"/university-counselling-articles/reject/{id}");
            })
            .WithTags(Tags.UniversityContent);
        }
    }
}
