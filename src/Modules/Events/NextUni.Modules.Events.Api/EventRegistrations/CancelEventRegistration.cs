using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Modules.Events.Application.Users;

namespace NextUni.Modules.Events.Api.EventRegistrations;

public class CancelEventRegistration : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("event-registrations/{id}/cancel", async ([FromRoute] Guid id, ISender sender) =>
            {
                var result = await sender.Send(new Application.CancelEventRegistration.CancelEventRegistration.Command(id));
                return result.MatchOk();
            })
            .RequireAuthorization()
            .WithName("CancelEventRegistration")
            .WithTags(Tags.Events);
    }
}