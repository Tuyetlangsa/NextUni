using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.Majors;

internal sealed class CreateAdmissionScoreByYearEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("majors/admission-scores/{year}", async ([FromBody]Request request,[FromRoute] int year, ISender sender) =>
            {
                var command = new Application.Majors.CreateAdmissionScoreByYear.CreateAdmissionScoreByYear.Command(
                    year,
                    request.AdmissionScores.Select(a => new Application.Majors.CreateAdmissionScoreByYear.CreateAdmissionScoreByYear.AdmissionScore(
                            a.MajorId,
                            a.GpaScore,
                            a.ExamScore
                        )
                    ).ToList()
                );

                var result = await sender.Send(command);
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.CreateAdmissionScoreByYear)
            .Produces<ApiResult<bool>>()
            .WithTags(Tags.Major);
    }

    internal sealed class Request
    { 
        public List<AdmissionScore> AdmissionScores { get; set; } = new List<AdmissionScore>();
    }

    public class AdmissionScore
    {
        public Guid MajorId { get; set; }
        public float GpaScore { get; set; }
        public float ExamScore { get; set; }
    };
    
}