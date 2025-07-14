using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Events.Api.Events;


public class GetEvents : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{status}", async (
                [FromQuery] int pageNumber, 
                [FromQuery] int pageSize, 
                [FromRoute] string status, 
                ISender sender) =>
            {
                var queryStatus = status switch
                {
                    "Published" => Application.Events.GetEvents.GetEvents.QueryStatus.Published,
                    "Completed" => Application.Events.GetEvents.GetEvents.QueryStatus.Completed,
                    "Ongoing" => Application.Events.GetEvents.GetEvents.QueryStatus.Ongoing,
                    "Cancelled" => Application.Events.GetEvents.GetEvents.QueryStatus.Canceled,
                    _ => throw new NextUni.Common.Application.Exceptions.NextUniException("invalid status provided"),
                };
                var result = await sender.Send(new Application.Events.GetEvents.GetEvents.Query(pageNumber, pageSize, queryStatus , false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .Produces<Page<Application.Events.GetEvents.GetEvents.Response>>()
            .WithTags(Tags.Events);
    
    
        app.MapGet("/admin/events/{status}", async (
                [FromQuery] int pageNumber, 
                [FromQuery] int pageSize, 
                [FromRoute] string status, 
                ISender sender) =>
            {
                var queryStatus = status switch
                {
                    "Published" => Application.Events.GetEvents.GetEvents.QueryStatus.Published,
                    "Completed" => Application.Events.GetEvents.GetEvents.QueryStatus.Completed,
                    "Ongoing" => Application.Events.GetEvents.GetEvents.QueryStatus.Ongoing,
                    "Cancelled" => Application.Events.GetEvents.GetEvents.QueryStatus.Canceled,
                    "Pending" => Application.Events.GetEvents.GetEvents.QueryStatus.Pending,
                    "Rejected" => Application.Events.GetEvents.GetEvents.QueryStatus.Rejected,
                    _ => throw new NextUni.Common.Application.Exceptions.NextUniException("invalid status provided"),
                };
                var result = await sender.Send(new Application.Events.GetEvents.GetEvents.Query(pageNumber, pageSize, queryStatus , true));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.GetAdministrativeEvents)
            .Produces<Page<Application.Events.GetEvents.GetEvents.Response>>()
            .WithTags(Tags.Events);
    }

}