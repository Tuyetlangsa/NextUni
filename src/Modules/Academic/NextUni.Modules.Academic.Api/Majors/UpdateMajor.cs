using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.Majors
{
    internal sealed class UpdateMajor : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("majors/{id}", async ([FromRoute] Guid id, [FromBody] Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(
                    new Application.Majors.UpdateMajor.UpdateMajor.Command(
                        id,
                        request.Code,
                        request.Name,
                        request.UniversityId,
                        request.Title,
                        request.Content));

                return result.MatchOk();
            })
                // .RequireAuthorization(Permissions.ModifyMajor)
                .WithTags(Tags.Major);
        }

        internal sealed class Request
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public Guid UniversityId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }
    }
}