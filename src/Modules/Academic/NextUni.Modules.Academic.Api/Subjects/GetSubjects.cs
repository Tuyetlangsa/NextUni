using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.Subjects;

internal sealed class GetSubjects : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/subjects", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
            {
                var result = await sender.Send(new Application.Subjects.GetSubjects.GetSubjects.Query(pageNumber,  pageSize, false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.Major);
        
        app.MapGet("admin/subjects", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
            {
                var result = await sender.Send(new Application.Subjects.GetSubjects.GetSubjects.Query(pageNumber,  pageSize, true));
                return result.MatchOk();
            })
            // .RequireAuthorization(Permissions.GetAdministrativeSubjects)
            .Produces<Page<Application.Subjects.GetSubjects.GetSubjects.ResponseItem>>()
            .WithTags(Tags.Major);
    }
}