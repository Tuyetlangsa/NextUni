using Microsoft.AspNetCore.Routing;

namespace NextUni.Common.Api.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}