using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Modules.Academic.Api;

namespace NextUni.Modules.Contents.Api.UniversityCounsellingArticles;

public class GetUniversityCounsellingArticles : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/university-counselling-articles/{status}", async (
                [FromQuery] int pageNumber, 
                [FromQuery] int pageSize, 
                [FromRoute] string status, 
                ISender sender) => 
            {
        Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus queryStatus;
        queryStatus = status switch
        {
            "All" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus.All,
            "Published" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus
                .Published,
            "Draft" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus.Draft,
            "Pending" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus
                .Pending,
        };

        var result =
            await sender.Send(
                new Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.Query(
                    pageNumber, 
                    pageSize, 
                    queryStatus, 
                    false));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.UniversityContent);
        
        
        app.MapGet("/admin/university-counselling-articles/{status}", async (
                [FromQuery] int pageNumber, 
                [FromQuery] int pageSize, 
                [FromRoute] string status, 
                ISender sender) => 
            {
                var queryStatus = status switch
                {
                    "All" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus.All,
                    "Published" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus
                        .Published,
                    "Draft" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus.Draft,
                    "Pending" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus
                        .Pending,
                };

                var result =
                    await sender.Send(
                        new Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.Query(
                            pageNumber, 
                            pageSize, 
                            queryStatus, 
                            true));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .WithTags(Tags.UniversityContent);
    }
}