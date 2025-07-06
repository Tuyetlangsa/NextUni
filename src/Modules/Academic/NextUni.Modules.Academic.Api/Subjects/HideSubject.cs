using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.Subjects;

internal sealed class HideSubject : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/subjects/{subjectId:guid}", async ([FromRoute]Guid subjectId, ISender sender) =>
            {
                Result result = await sender.Send(new Application.Subjects.HideSubject.HideSubject.Command(subjectId));
                return result.MatchOk();
            })
            // .RequireAuthorization(Permissions.ModifySubject)
            .WithTags(Tags.Major);
    }

    internal sealed class Request
    {
        public Guid Id { get; set; }
    }
}