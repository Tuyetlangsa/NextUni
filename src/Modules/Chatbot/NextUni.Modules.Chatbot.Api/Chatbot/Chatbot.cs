using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Chatbot.Api.Chatbot;

public class Chatbot : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/chatbot", async (ISender sender, string prompt) =>
            {
                var result = await sender.Send(new Application.Chatbot.Chatbot.Query(prompt));
                return result.MatchOk();
            })
            .WithTags("Chatbot");
    }
}