using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Api;

namespace NextUni.Modules.Contents.Api.UniversityCounsellingArticles;

public class CreateUniversityCounsellingArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("university-counselling-articles", async (Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.CreateUniversityCounsellingArticle.CreateUniversityCounsellingArticle.Command(
                        request.UniversityId,
                        request.Title,
                        request.Content));

                return result.MatchCreated(id => $"/university-counselling-articles/{id}");
            })
            .RequireAuthorization()
            .WithTags(Tags.UniversityContent);
    }
    
    
    public class Request
    {
        public Guid UniversityId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}