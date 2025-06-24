using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.Majors;

internal sealed class GetAdmissionScoreByYear : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/majors/admission-scores/{year}", async ([FromRoute] DateOnly year, ISender sender) =>
            {
                var result =
                    await sender.Send(
                        new Application.Majors.GetAdmissionScoresByYear.GetAdmissionScoreByYear.Query(year));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.Major);
    }
}
