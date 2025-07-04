using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NextUni.Common.Api.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Domain;

namespace NextUni.Modules.Events.Api.Events
{
    public class RejectEvent : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("events/reject/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.Events.RejectEvent.RejectEvent.Command(id));
                return result.MatchCreated(id => $"/events/reject/{id}");
            })
            // .RequireAuthorization(Permissions.ProcessEvents)
                .WithTags(Tags.Events);
        }
    }
}
