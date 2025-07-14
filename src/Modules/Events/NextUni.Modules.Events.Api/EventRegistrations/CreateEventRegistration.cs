using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Events.Api.EventRegistrations;

public class CreateEventRegistration : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("event-registrations/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
                var result = await sender.Send(new Application.EventRegistrations.CreateEventRegistration.CreateEventRegistration.Command(id));
                return result.MatchCreated(id => $"/event-registrations/{id}");
            })
            .RequireAuthorization(Permissions.RegisterEvent)
            .WithName("CreateEventRegistration")
            .WithTags(Tags.Events);
    }
}