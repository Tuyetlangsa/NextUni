using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Academic.Api.Subjects;

internal sealed class GetSubjects : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/subjects", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
            {
                var result = await sender.Send(new Application.Subjects.GetSubjects.CreateSubject.Query(pageNumber,  pageSize));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.Academic);
    }
}