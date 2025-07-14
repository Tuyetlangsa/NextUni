using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Events.GetEventsByUniversity;

namespace NextUni.Modules.Events.Api.Events;

public class GetEventsByUniversity : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/staff/universities/{universityId}/events/{status}", async (
                [FromQuery] int pageNumber,
                [FromQuery] int pageSize,
                [FromRoute] string status,
                [FromRoute] Guid universityId,
                ISender sender) =>
            {
                var queryStatus = status switch
                {
                    "Pending" => Application.Events.GetEventsByUniversity.GetEventByUniversity.QueryStatus.Pending,
                    "Published" => Application.Events.GetEventsByUniversity.GetEventByUniversity.QueryStatus.Published,
                    "Completed" => Application.Events.GetEventsByUniversity.GetEventByUniversity.QueryStatus.Completed,
                    "Ongoing" => Application.Events.GetEventsByUniversity.GetEventByUniversity.QueryStatus.Ongoing,
                    "Cancelled" => Application.Events.GetEventsByUniversity.GetEventByUniversity.QueryStatus.Canceled,
                    "Rejected" => Application.Events.GetEventsByUniversity.GetEventByUniversity.QueryStatus.Rejected,
                    _ => throw new NextUni.Common.Application.Exceptions.NextUniException("invalid status provided"),
                };
                var result =
                    await sender.Send(
                        new Application.Events.GetEventsByUniversity.GetEventByUniversity.Query(pageNumber, pageSize, universityId, queryStatus));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.GetStaffEvent)
            .Produces<Page<GetEventByUniversity.Response>>()
            .WithTags(Tags.Events);
    }
}