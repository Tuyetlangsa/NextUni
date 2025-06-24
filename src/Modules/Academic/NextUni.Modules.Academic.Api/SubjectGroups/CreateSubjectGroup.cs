using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.SubjectGroups;

internal sealed class CreateSubjectGroup : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("subject-groups", async ([FromBody]Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new Application.SubjectGroups.CreateSubjectGroup.CreateSubjectGroup.Command(request.Code, request.SubjectIds));
                return result.MatchCreated(id => $"/subject-groups/{id}");
            })
            .AllowAnonymous()
            .WithTags(Tags.SubjectGroup);
    }

    internal sealed class Request
    {
        public string Code { get; set; }
        public List<Guid> SubjectIds { get; set; } = new();
    }
}