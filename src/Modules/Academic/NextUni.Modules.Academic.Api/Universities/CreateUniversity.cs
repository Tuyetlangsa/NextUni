using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Api.Universities;

internal sealed class CreateUniversity : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("universities", async ([FromBody]Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.Universities.CreateUniveristy.CreateUniversity.Command(
                    request.Code,
                    request.Name,
                    request.Region,
                    request.UniversityType,
                    request.Address,
                    request.Email,
                    request.WebsiteUrl,
                    request.FacebookUrl,
                    request.Title,
                    request.Content));
                
                return result.MatchCreated(id => $"/universities/{id}");
            })
            // .RequireAuthorization(Permissions.CreateUniversity)
            .WithName("CreateUniversity")
            .WithTags(Tags.University);
    }

    internal sealed class Request
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Region Region { get; set; }
        public UniversityType UniversityType { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}