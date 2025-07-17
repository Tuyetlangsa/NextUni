using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.IntroductionBlogs.CreateMajorIntroductionBlog;

internal sealed class CreateMajorIntroductionBlog : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("majors/introduction-blog", async ([FromBody]Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.IntroductionBlogs.CreateMajorIntroductionBlog.CreateMajorIntroductionBlog.Command(
                        request.MajorId,
                        request.Title, 
                        request.Content));
                
                return result.MatchCreated(id => $"/majors/introduction-blog/{id}");
            })
            .RequireAuthorization(Permissions.CreateMajor)
            .Produces<ApiResult<Guid>>()
            .WithTags(Tags.Major);
    }

    internal sealed class Request
    { 
        public Guid MajorId  { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}