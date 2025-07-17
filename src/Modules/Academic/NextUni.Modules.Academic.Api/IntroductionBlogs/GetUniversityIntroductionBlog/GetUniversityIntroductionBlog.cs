using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Modules.Academic.Application.IntroductionBlogs.GetIntroductionBlogByUniverity;

namespace NextUni.Modules.Academic.Api.IntroductionBlogs.GetUniversityIntroductionBlog;

internal sealed class GetUniversityIntroductionBlog : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/universities/{id}/introduction-blog", async ([FromRoute] Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetIntroductionBlogByUniversity.Query(id));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .Produces<ApiResult<GetIntroductionBlogByUniversity.ResponseItem>>()
            .WithTags(Tags.Major);
        
        // app.MapGet("admin/majors/{id}", async ([FromRoute] Guid id, ISender sender) =>
        //     {
        //         var result = await sender.Send(new Application.Majors.GetMajorById.GetMajorById.Query(id,  true));
        //         return result.MatchOk();
        //     })
        //     .AllowAnonymous()
        //     .Produces<ApiResult<Application.Majors.GetMajorById.GetMajorById.MajorResponse>>()
        //     .WithTags(Tags.Major);
    }
}