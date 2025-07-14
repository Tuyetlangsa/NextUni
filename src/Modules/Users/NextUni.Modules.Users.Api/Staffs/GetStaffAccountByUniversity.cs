using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Users.Api.Staffs;

public class GetStaffAccountByUniversity : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/universities/{universityId}/staff", async ([FromRoute] Guid universityId, ISender sender) =>
            {
                var result = await sender.Send(new Application.Staffs.GetStaffAccountByUniversity.GetStaffAccountByUniversity.Query(universityId));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.GetStaffAccountByUniversity)
            .Produces<Application.Staffs.GetStaffAccountByUniversity.GetStaffAccountByUniversity.Response>()
            .WithTags(Tags.Users);
    }
}