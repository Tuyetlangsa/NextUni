using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.Majors;

internal sealed class UpdateMajorSubjectGroupByYearEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("majors/{majorId}/subject-groups/{year}", async (
                [FromRoute] Guid majorId, 
                [FromRoute] int year, 
                [FromBody] Request request, 
                ISender sender) =>
            {
                var result =
                    await sender.Send(
                        new Application.Majors.UpdateMajorSubjectGroupsByYear.UpdateMajorSubjectGroupByYear.Command(
                            majorId, request.GroupIds, year));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.CreateMajorGroup)
            .Produces<ApiResult<bool>>()
            .WithTags(Tags.Major);
    }

    internal sealed class Request
    {
        public List<Guid> GroupIds { get; set; }
    }
}