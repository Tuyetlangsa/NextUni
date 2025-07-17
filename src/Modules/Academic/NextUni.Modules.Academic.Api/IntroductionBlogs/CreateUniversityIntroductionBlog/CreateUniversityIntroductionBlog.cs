using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.IntroductionBlogs.CreateUniversityIntroductionBlog;

internal sealed class CreateUniversityIntroductionBlog : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("universities/introduction-blog", async ([FromBody]Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.IntroductionBlogs.CreateUniversityIntroductionBlog.CreateUniversityIntroductionBlog.Command(
                        request.UnviersityId,
                        request.Title, 
                        request.Content));
                
                return result.MatchCreated(id => $"/universities/introduction-blog/{id}");
            })
            .RequireAuthorization(Permissions.CreateMajor)
            .Produces<ApiResult<Guid>>()
            .WithTags(Tags.Major);
    }

    internal sealed class Request
    { 
        public Guid UnviersityId  { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}