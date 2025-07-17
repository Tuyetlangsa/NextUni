using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Contents.Api.MasterCounsellingArticles;

public class GetMasterCounsellingArticlesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("master-counselling-articles", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
            {
                var result = await sender.Send(
                        new Application.GetMasterCounsellingArticles.GetMasterCounsellingArticles.Query(
                            pageNumber,
                            pageSize, false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .Produces<ApiResult<Page<Application.GetMasterCounsellingArticles.GetMasterCounsellingArticles.Response>>>()
            .WithTags(Tags.SystemContent);
        
        app.MapGet("admin/master-counselling-articles", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
            {
                var result = await sender.Send(
                        new Application.GetMasterCounsellingArticles.GetMasterCounsellingArticles.Query(
                            pageNumber,
                            pageSize, true));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.GetAdministrativeMasterArticles)
            .Produces<ApiResult<Page<Application.GetMasterCounsellingArticles.GetMasterCounsellingArticles.Response>>>()
            .WithTags(Tags.SystemContent);
    }
}