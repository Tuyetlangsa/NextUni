using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.Universities;

public class GetUniversityById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/universities/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
               var result = await sender.Send(new Application.Universities.GetUniversityById.GetUniversityId.Query(id, false));
               return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.University);
        
        app.MapGet("/admin/universities/{id}", async ([FromRoute] Guid id, ISender sender) =>
            {
                var result = await sender.Send(new Application.Universities.GetUniversityById.GetUniversityId.Query(id, true));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.University);
    }
}