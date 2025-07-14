using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Users.Api.Staffs;

internal sealed class DeleteStaffAccount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/universities/{universityId}/staff-account", async ([FromRoute] Guid universityId, ISender sender) =>
            {
                var result = await sender.Send(new Application.Staffs.DeleteStaffAccount.DeleteStaffAccount.Command(universityId));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.DeleteStaffAccount)
            .WithTags(Tags.Users);
    }
}
