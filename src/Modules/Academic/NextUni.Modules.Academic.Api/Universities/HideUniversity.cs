using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.Universities
{
    internal sealed class HideUniversityEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("universities/status/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.Universities.HideUniversity.HideUniversity.Command(id));

                return result.MatchCreated(id => $"/universities/status/{id}");
            })
                .RequireAuthorization(Permissions.ModifyUniversity)
                .Produces<ApiResult<bool>>()
                .WithTags(Tags.University);
        }
    }
}
