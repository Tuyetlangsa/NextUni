using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.Subjects
{
    internal sealed class UpdateSubject : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("subjects/{id}", async ([FromRoute] Guid id, [FromBody] Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.Subjects.UpdateSubject.UpdateSubject.Command(
                        id,
                        request.Name));

                return result.MatchCreated(id => $"/subjects/update/{id}");
            })
                .AllowAnonymous()
                .WithTags(Tags.Major);
        }

        internal sealed class Request
        {
            public string Name { get; set; }
        }
    }
}
