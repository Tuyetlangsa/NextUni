using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.Majors;

public class GetMajorById
{
    
}

internal sealed class GetMajorByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/majors/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
                var result = await sender.Send(new Application.Majors.GetMajorById.GetMajorById.Query(id,  false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .Produces<ApiResult<Application.Majors.GetMajorById.GetMajorById.MajorResponse>>()
            .WithTags(Tags.Major);
        
        app.MapGet("admin/majors/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
                var result = await sender.Send(new Application.Majors.GetMajorById.GetMajorById.Query(id,  true));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .Produces<ApiResult<Application.Majors.GetMajorById.GetMajorById.MajorResponse>>()
            .WithTags(Tags.Major);
    }
}