FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/NextUni.Api/NextUni.Api.csproj", "API/NextUni.Api/"]
COPY ["src/Common/NextUni.Common.Application/NextUni.Common.Application.csproj", "Common/NextUni.Common.Application/"]
COPY ["src/Common/NextUni.Common.Domain/NextUni.Common.Domain.csproj", "Common/NextUni.Common.Domain/"]
COPY ["src/Common/NextUni.Common.Infrastructure/NextUni.Common.Infrastructure.csproj", "Common/NextUni.Common.Infrastructure/"]
COPY ["src/Common/NextUni.Common.Api/NextUni.Common.Api.csproj", "Common/NextUni.Common.Api/"]
COPY ["src/Modules/Users/NextUni.Modules.Users.Application/NextUni.Modules.Users.Application.csproj", "Modules/Users/NextUni.Modules.Users.Application/"]
COPY ["src/Modules/Users/NextUni.Modules.Users.Domain/NextUni.Modules.Users.Domain.csproj", "Modules/Users/NextUni.Modules.Users.Domain/"]
COPY ["src/Modules/Users/NextUni.Modules.Users.Infrastructure/NextUni.Modules.Users.Infrastructure.csproj", "Modules/Users/NextUni.Modules.Users.Infrastructure/"]
COPY ["src/Modules/Users/NextUni.Modules.Users.Api/NextUni.Modules.Users.Api.csproj", "Modules/Users/NextUni.Modules.Users.Api/"]
RUN dotnet restore "API/NextUni.Api/NextUni.Api.csproj"
COPY src/ .
WORKDIR "/src/API/NextUni.Api"
RUN dotnet build "./NextUni.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NextUni.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NextUni.Api.dll"]
