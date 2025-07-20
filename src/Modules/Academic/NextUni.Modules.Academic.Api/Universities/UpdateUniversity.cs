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

internal sealed class UpdateUniversityEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("universities", async ([FromBody]Request request, ISender sender) =>
            {
                Result result = await sender.Send(
                    new Application.Universities.UpdateUniversity.UpdateUniversity.Command(
                        request.Id,
                        request.Code,
                        request.Name,
                        request.Region,
                        request.UniversityType,
                        request.Address,
                        request.Email,
                        request.WebsiteUrl,
                        request.FacebookUrl));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.ModifyUniversity)
            .Produces<ApiResult<bool>>()
            .WithName("UpdateUniversity")
            .WithTags(Tags.University);
    }

    internal sealed class Request
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Region Region { get; set; }
        public UniversityType UniversityType { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string FacebookUrl { get; set; }
    }
}