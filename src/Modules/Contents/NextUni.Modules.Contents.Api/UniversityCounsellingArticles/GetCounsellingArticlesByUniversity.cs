using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Domain;

namespace NextUni.Modules.Contents.Api.UniversityCounsellingArticles;

public class GetCounsellingArticlesByUniversityEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("universities/{universityId}/university-counselling-articles/{status}", async (
                [FromRoute] Guid universityId,
                [FromQuery] int pageNumber, 
                [FromQuery] int pageSize, 
                [FromRoute] string status, 
                ISender sender) => 
            {
                Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.QueryStatus queryStatus;
                queryStatus = status switch
                {
                    "Published" => Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.QueryStatus
                        .Published,
                    "Draft" => Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.QueryStatus.Draft,
                    "Pending" => Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.QueryStatus
                        .Pending,
                };

                var result =
                    await sender.Send(
                        new Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.Query(
                            universityId,
                            pageNumber, 
                            pageSize, 
                            queryStatus, 
                            false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .Produces<ApiResult<Page<Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.Response>>>()
            .WithTags(Tags.UniversityContent);
        
        app.MapGet("admin/universities/{universityId}/university-counselling-articles/{status}", async (
                [FromRoute] Guid universityId,
                [FromQuery] int pageNumber, 
                [FromQuery] int pageSize, 
                [FromRoute] string status, 
                ISender sender) => 
            {
                Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.QueryStatus queryStatus;
                queryStatus = status switch
                {
                    "Published" => Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.QueryStatus
                        .Published,
                    "Draft" => Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.QueryStatus.Draft,
                    "Pending" => Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.QueryStatus
                        .Pending,
                };

                var result =
                    await sender.Send(
                        new Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.Query(
                            universityId,
                            pageNumber, 
                            pageSize, 
                            queryStatus, 
                            true));
                return result.MatchOk();
            })
            .RequireAuthorization(Permissions.GetAdministrativeUniversityArticles)
            .Produces<ApiResult<Page<Application.GetCounsellingArticlesByUniversity.GetCounsellingArticlesByUniversity.Response>>>()
            .WithTags(Tags.UniversityContent);
    }
}