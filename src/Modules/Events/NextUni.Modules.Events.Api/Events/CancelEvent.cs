using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Events.Api.Events
{
    public class CancelEvent : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("events/cancel/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.Events.CancelEvent.CancelEvent.Command(id));
                return result.MatchCreated(id => $"/events/cancel/{id}");
            })
                .WithTags(Tags.Events);
        }
    }
}
