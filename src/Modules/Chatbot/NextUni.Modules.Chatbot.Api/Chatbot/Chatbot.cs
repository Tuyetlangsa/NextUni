using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Api.Results;

namespace NextUni.Modules.Chatbot.Api.Chatbot;

public class Chatbot : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/chatbot", async ([FromBody] Request request, ISender sender) =>
            {
                var result = await sender.Send(new Application.Chatbot.Chatbot.Query(request.Prompt));
                return result.MatchOk();
            })
            .WithTags("Chatbot");
    }

    public class Request
    {
        public string Prompt { get; set; }

    }
}