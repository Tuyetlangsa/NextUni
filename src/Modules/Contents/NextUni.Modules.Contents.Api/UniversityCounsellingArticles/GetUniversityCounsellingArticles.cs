using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Api;

namespace NextUni.Modules.Contents.Api.UniversityCounsellingArticles;

public class GetUniversityCounsellingArticles : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/university-counselling-articles/{status}", async (
                [FromQuery] int pageNumber, 
                [FromQuery] int pageSize, 
                ISender sender) => 
            {
        Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus queryStatus;
        
        var result =
            await sender.Send(
                new Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.Query(
                    pageNumber, 
                    pageSize, null, null, null));
                return result.MatchOk();
            })
            .AllowAnonymous()
            .Produces<Page<Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.Response>>()
            .WithTags(Tags.UniversityContent);
        
        
        app.MapGet("/admin/university-counselling-articles/{status}", async (
                [FromQuery] int pageNumber, 
                [FromQuery] int pageSize, 
                [FromRoute] string status, 
                ISender sender) => 
            {
                var queryStatus = status switch
                {
                    "Published" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus
                        .Published,
                    "Pending" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus
                        .Pending,
                    _ => throw new NextUniException("invalid status provided"),
                };
                var result =
                    await sender.Send(
                        new Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.Query(
                            pageNumber, 
                            pageSize, 
                            queryStatus, 
                            true, false));
                return result.MatchOk();
            })
            .RequireAuthorization()
            .Produces<Page<Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.Response>>()
            .WithTags(Tags.UniversityContent);
        
        app.MapGet("/staff/university-counselling-articles/{status}", async (
                [FromQuery] int pageNumber, 
                [FromQuery] int pageSize, 
                [FromRoute] string status, 
                ISender sender) => 
            {
                var queryStatus = status switch
                {
                    "Published" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus
                        .Published,
                    "Draft" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus.Draft,
                    "Pending" => Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.QueryStatus
                        .Pending,
                    _ => throw new NextUniException("Invalid status provided"),
                };
                var result =
                    await sender.Send(
                        new Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.Query(
                            pageNumber, 
                            pageSize, 
                            queryStatus, 
                            false, true));
                return result.MatchOk();
            })
            .RequireAuthorization()
            .Produces<Page<Application.GetUniversityCounsellingArticles.GetUniversityCounsellingArticles.Response>>()
            .WithTags(Tags.UniversityContent);
        
    }
}