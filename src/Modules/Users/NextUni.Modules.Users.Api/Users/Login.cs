using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Users.Api.Users;

public class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", async (Request request, ISender sender) =>
            {
                var result =
                    await sender.Send(new Application.Users.LoginUser.Login.Command(request.Email, request.Password));
                return result.MatchOk();
            })
        .AllowAnonymous()
        .Produces<Application.Users.LoginUser.Login.TokenResponse>()
        .WithTags(Tags.Users);
    }


    public class Request
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}