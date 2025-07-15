using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Modules.Users.Application.Users.GetCurrentUser;

namespace NextUni.Modules.Users.Api.Users;

public class CurrentUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/current-user", async (ISender sender) =>
            {
                var result = await sender.Send(new GetCurrentUser.Query());
                return result.MatchOk();
            })
            .RequireAuthorization()
            .Produces <ApiResult<Application.Users.GetCurrentUser.GetCurrentUser.Response>>()
            .WithTags(Tags.Users);
    }
}