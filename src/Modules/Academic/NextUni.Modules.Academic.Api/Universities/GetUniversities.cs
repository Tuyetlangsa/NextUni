using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Modules.Academic.Application.Universities.GetUniversities;

namespace NextUni.Modules.Academic.Api.Universities;

internal sealed class GetUniversities : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/universities", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] GetUniversity.QueryFilter queryFilter, ISender sender) =>
            {
                var result = await sender.Send(new GetUniversity.Query(pageNumber,  pageSize, queryFilter, false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.University);
        
        app.MapGet("admin/universities", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] GetUniversity.QueryFilter queryFilter, ISender sender) =>
            {
                var result = await sender.Send(new GetUniversity.Query(pageNumber,  pageSize, queryFilter, true));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.University);
    }
}