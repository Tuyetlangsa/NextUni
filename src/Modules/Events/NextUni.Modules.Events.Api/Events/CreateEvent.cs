using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Events.Api.Events;

public class CreateEvent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.Events.CreateEvent.CreateEvent.Command(
                        request.Name,
                       request.StartDate,
                        request.Address,
                        request.IsOnline,
                        request.UniversityId,
                        request.Title,
                        request.Content));
                return result.MatchCreated(id => $"/events/{id}");
            })
            // .RequireAuthorization(Permissions.CreateEvent)
            .WithName("CreateEvent")
            .WithTags(Tags.Events);
    }

    public class Request
    {
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public string Address { get; set; } = null!;
        public bool IsOnline { get; set; }
        public Guid UniversityId { get; set; }
        
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}