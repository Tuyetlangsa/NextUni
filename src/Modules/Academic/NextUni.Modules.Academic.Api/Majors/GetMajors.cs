using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.Majors;

internal sealed class GetMajors : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/majors", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
            {
                var result = await sender.Send(new Application.Majors.GetMajors.GetMajors.Query(pageNumber,  pageSize, false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.Academic);
        
        app.MapGet("admin/majors", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
            {
                var result = await sender.Send(new Application.Majors.GetMajors.GetMajors.Query(pageNumber,  pageSize, true));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.Academic);
    }
}