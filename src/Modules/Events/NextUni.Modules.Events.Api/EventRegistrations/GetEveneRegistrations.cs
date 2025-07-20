using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Events.Api.EventRegistrations;

public class GetEventRegistrations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}/registrations", async ([FromRoute] Guid id, ISender sender) =>
            {
                var result = await sender
                    .Send(new Application.EventRegistrations.GetEventRegistrations.GetEventRegistrations.Query(id));
                return result.MatchOk();
            })
            .Produces<ApiResult<Application.EventRegistrations.GetEventRegistrations.GetEventRegistrations.Response>>()
            .WithTags(Tags.Events);
    }
}