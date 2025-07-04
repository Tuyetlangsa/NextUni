using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.Majors;

internal sealed class CreateMajor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("majors", async ([FromBody]Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.Majors.CreateMajor.CreateMajor.Command(
                        request.Code, 
                        request.Name, 
                        request.UniversityId, 
                        request.Title, 
                        request.Content));
                
                return result.MatchCreated(id => $"/majors/{id}");
            })
            .RequireAuthorization(Permissions.CreateMajor)
            .WithTags(Tags.Major);
    }

    internal sealed class Request
    { 
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid UniversityId  { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}