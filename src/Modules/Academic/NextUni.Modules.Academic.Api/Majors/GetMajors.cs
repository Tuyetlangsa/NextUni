using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.SubjectGroups.GetSubjectGroups;

namespace NextUni.Modules.Academic.Api.Majors;

internal sealed class GetMajors : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("universities/{universityId}/majors", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, Guid universityId, ISender sender) =>
            {
                var result = await sender.Send(new Application.Majors.GetMajors.GetMajors.Query(pageNumber,  pageSize, universityId, false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .Produces<ApiResult<Page<Application.Majors.GetMajors.GetMajors.MajorResponse>>>()
            .WithTags(Tags.Major);
        
        app.MapGet("admin/universities/{universityId}/majors", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, Guid universityId, ISender sender) =>
            {
                var result = await sender.Send(new Application.Majors.GetMajors.GetMajors.Query(pageNumber,  pageSize, universityId, true));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.GetAdministrativeMajors)
            .Produces<ApiResult<Page<Application.Majors.GetMajors.GetMajors.MajorResponse>>>()
            .WithTags(Tags.Major);
    }
}