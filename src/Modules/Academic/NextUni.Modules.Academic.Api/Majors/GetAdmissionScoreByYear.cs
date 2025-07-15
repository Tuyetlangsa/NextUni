using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.Majors;

internal sealed class GetAdmissionScoreByYearEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/universities/{universityId}/majors/admission-scores/{year}", async ([FromRoute] int year,[FromRoute] Guid universityId, ISender sender) =>
            {
                var result =
                    await sender.Send(
                        new Application.Majors.GetAdmissionScoresByYear.GetAdmissionScoreByYear.Query(year, universityId));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.Major);
    }
}
