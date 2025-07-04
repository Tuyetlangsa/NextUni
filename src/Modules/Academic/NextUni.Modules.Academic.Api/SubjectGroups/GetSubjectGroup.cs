using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.SubjectGroups;

public class GetSubjectGroups : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("subject-groups", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
            {
                var result =
                    await sender.Send(
                        new Application.SubjectGroups.GetSubjectGroups.GetSubjectGroups.Query(
                            pageNumber, 
                            pageSize,
                            false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.SubjectGroup);

        app.MapGet("admin/subject-groups", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
            {
                var result =
                    await sender.Send(
                        new Application.SubjectGroups.GetSubjectGroups.GetSubjectGroups.Query(
                            pageNumber, 
                            pageSize,
                            true));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.GetAdministrativeSubjectGroups)
            .WithTags(Tags.SubjectGroup);
    }
}

