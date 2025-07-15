using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Api.SubjectGroups
{
    internal sealed class UpdateSubjectGroupEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("subject-groups/{id}", async ([FromRoute] Guid id, [FromBody] Request request, ISender sender) =>
            {
                Result result = await sender.Send(
                    new Application.SubjectGroups.UpdateSubjectGroup.UpdateSubjectGroup.Command(
                        id,
                        request.Code,
                        request.SubjectIds));

                return result.MatchOk();
            })
                .RequireAuthorization(Permissions.ModifySubjectGroup)
                .Produces<ApiResult<bool>>()
                .WithTags(Tags.SubjectGroup);
        }

        internal sealed class Request
        {
            public string Code { get; set; }
            public List<Guid> SubjectIds { get; set; }
        }
    }
}
