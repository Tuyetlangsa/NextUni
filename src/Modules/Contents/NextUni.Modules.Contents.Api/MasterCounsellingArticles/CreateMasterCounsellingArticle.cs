using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Api;

namespace NextUni.Modules.Contents.Api.MasterCounsellingArticles;

public class CreateMasterCounsellingArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("master-counselling-articles", async (Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.CreateMasterCounsellingArticle.CreateMasterCounsellingArticle.Command(
                        request.Title,
                        request.Content));

                return result.MatchCreated(id => $"/master-counselling-articles/{id}");
            })
            .RequireAuthorization(Permissions.CreateMasterArticle)
            .Produces<Request>()
            .WithTags(Tags.SystemContent);
    }

    public class Request
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}