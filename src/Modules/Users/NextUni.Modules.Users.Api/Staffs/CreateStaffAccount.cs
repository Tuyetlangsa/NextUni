using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Users.Api.Staffs;


internal sealed class CreateStaffAccountEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("staffs/create-account", async (Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new Application.Staffs.CreateStaffAccount.CreateStaffAccount.Command(
                    request.Email,
                    request.Password,
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber,
                    request.UniversityId));

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization(Permissions.CreateStaffAccount)
            .Produces<ApiResult<Guid>>()
            .WithTags(Tags.Users);
    }

    internal sealed class Request
    {
        public string Email { get; init; }

        public string Password { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        
        public Guid UniversityId { get; init; }
    }
}
