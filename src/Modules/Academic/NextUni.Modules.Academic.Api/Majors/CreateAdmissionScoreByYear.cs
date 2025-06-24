using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.Majors;

internal sealed class CreateAdmissionScoreByYear : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("majors/admission-scores", async ([FromBody]Request request, ISender sender) =>
            {
                var command = new Application.Majors.CreateAdmissionScoreByYear.CreateAdmissionScoreByYear.Command(
                    request.Year,
                    request.AdmissionScores.ToDictionary(
                        kv => kv.Key,
                        kv => new Application.Majors.CreateAdmissionScoreByYear.CreateAdmissionScoreByYear.AdmissionScore(
                            kv.Value.GpaScore,
                            kv.Value.ExamScore
                        )
                    )
                );

                var result = await sender.Send(command);
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.Major);
    }

    internal sealed class Request
    { 
        public DateOnly Year { get; set; }
        public Dictionary<Guid, AdmissionScore> AdmissionScores { get; set; } = new Dictionary<Guid, AdmissionScore>();
    }

    public class AdmissionScore
    {
        public float GpaScore { get; set; }
        public float ExamScore { get; set; }
    };
    
}