using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;

namespace NextUni.Modules.Academic.Api.Majors;

internal sealed class UpdateMajorSubjectGroupByYear : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("majors/{majorId}/subject-groups/{year}", async (
                [FromRoute] Guid majorId, 
                [FromRoute] DateOnly year, 
                [FromBody] Request request, 
                ISender sender) =>
            {
                var result =
                    await sender.Send(
                        new Application.Majors.UpdateMajorSubjectGroupsByYear.UpdateMajorSubjectGroupByYear.Command(
                            majorId, request.GroupIds, year));
            })
            // .RequireAuthorization(Permissions.ModifyMajor)
            .WithTags(Tags.Major);
    }

    internal sealed class Request
    {
        public List<Guid> GroupIds { get; set; }
    }
}