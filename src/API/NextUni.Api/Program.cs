using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NextUni.Api.Extensions;
using NextUni.Api.Middleware;
using NextUni.Common.Api.Endpoints;
using NextUni.Common.Application;
using NextUni.Common.Infrastructure;
using NextUni.Common.Infrastructure.Configuration;
using NextUni.Modules.Academic.Infrastructure;
using NextUni.Modules.Contents.Infrastructure;
using NextUni.Modules.Events.Infrastructure;
using NextUni.Modules.Users.Infrastructure;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

Assembly[] moduleApplicationAssemblies = [NextUni.Modules.Users.Application.AssemblyReference.Assembly,
                                          NextUni.Modules.Events.Application.AssemblyReference.Assembly,
                                          NextUni.Modules.Contents.Application.AssemblyReference.Assembly,
                                          NextUni.Modules.Academic.Application.AssemblyReference.Assembly];

builder.Services.AddApplication(moduleApplicationAssemblies);

string databaseConnectionString = builder.Configuration.GetConnectionStringOrThrow("Database");
string redisConnectionString = builder.Configuration.GetConnectionStringOrThrow("Cache");

builder.Services.AddInfrastructure(
    [
        EventModule.ConfigureConsumers], 
        databaseConnectionString,
        redisConnectionString);

Uri keyCloakHealthUrl = builder.Configuration.GetKeyCloakHealthUrl();

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString)
    .AddKeyCloak(keyCloakHealthUrl);

builder.Configuration.AddModuleConfiguration(["users", "events","contents", "academic"]);

builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddEventModule(builder.Configuration);
builder.Services.AddContentModule(builder.Configuration);
builder.Services.AddAcademicModule(builder.Configuration);
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseLogContext();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();

namespace NextUni.Api
{
    public partial class Program;
}