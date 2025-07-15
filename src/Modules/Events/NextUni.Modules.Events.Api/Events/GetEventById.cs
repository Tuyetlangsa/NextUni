using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Events.Api.Events;

public class GetEventByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("event-by-id/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
                var result = await sender.Send(new Application.Events.GetEventById.GetEventById.Query(id));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .Produces<ApiResult<Application.Events.GetEventById.GetEventById.Response>>()
            .WithTags(Tags.Events);
    }
}