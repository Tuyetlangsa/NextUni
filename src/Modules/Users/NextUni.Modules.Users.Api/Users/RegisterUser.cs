using FluentValidation;
using NextUni.Common.Domain;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace NextUni.Modules.Users.Api.Users;

internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (Request request, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new NextUni.Modules.Users.Application.Users.RegisterUser.RegisterUser.Command(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.PhoneNumber));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags(Tags.Users);
    }

    internal sealed class Request
    {
        public string Email { get; init; }

        public string Password { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
    }
}
