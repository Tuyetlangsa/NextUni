using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Infrastructure.Outbox;
using NextUni.Modules.Academic.IntegrationEvents;
using NextUni.Modules.Chatbot.Application.Abstractions.Data;
using NextUni.Modules.Chatbot.Infrastructure.Database;
using NextUni.Modules.Chatbot.Infrastructure.Inbox;
using NextUni.Modules.Chatbot.Infrastructure.OllamaEmbeddingGenerator;
using NextUni.Modules.Chatbot.Infrastructure.Outbox;
using OllamaSharp;
using Qdrant.Client;
using IEmbeddingGenerator = NextUni.Modules.Chatbot.Application.Abstractions.EmbeddingGenerator.IEmbeddingGenerator;

namespace NextUni.Modules.Chatbot.Infrastructure;

public static class ChatbotModule
{
    public static IServiceCollection AddChatbotModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDomainEventHandlers();
        services.AddIntegrationEventHandlers();
        services.AddInfrastructure(configuration);
        
        services.AddEndpoints(Api.AssemblyReference.Assembly);
        
        IChatClient client = new OllamaApiClient(
            new Uri("http://localhost:11434/"),
            "qwen3");
    
        services.AddChatClient(client);

        services.AddEmbeddingGenerator<string, Embedding<float>>(sp =>
        {
            return new OllamaApiClient(
                new Uri("http://localhost:11434/"),
                "nomic-embed-text");
        });
        
        services.AddSingleton(new QdrantClient("localhost"));
        
        services.AddScoped<IEmbeddingGenerator, MyEmbeddingGenerator>();
        return services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UniversityCreatedIntegrationEvent>>();
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IVectorDbContext, VectorDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Chatbot))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));;

        
        services.Configure<OutboxOptions>(configuration.GetSection("Chatbot:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Chatbot:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();
    }
    
    
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
    
    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlers = Api.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlers)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler =
                typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, closedIdempotentHandler);
        }
    }
}