using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Events.Api.EventRegistrations;

public class GetPersonalEventRegistrations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/personal-registrations", async (ISender sender) =>
            {
                var result = await sender
                    .Send(new Application.EventRegistrations.GetPersonalEventRegistrations.GetPersonalEventRegistrations.Query());
                return result.MatchOk();
            })
            .RequireAuthorization()
            .Produces<ApiResult<Application.EventRegistrations.GetPersonalEventRegistrations.GetPersonalEventRegistrations.Response>>()
            .WithTags(Tags.Events);
    }
}