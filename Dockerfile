

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MessageService.Api/MessageService.Api.csproj", "src/MessageService.Api/"]
COPY ["src/MessageService.Application/MessageService.Application.csproj", "src/MessageService.Application/"]
COPY ["src/MessageService.Domain/MessageService.Domain.csproj", "src/MessageService.Domain/"]
COPY ["src/MessageService.Infrastructure/MessageService.Infrastructure.csproj", "src/MessageService.Infrastructure/"]
RUN dotnet restore "src/MessageService.Api/MessageService.Api.csproj"
COPY . .
WORKDIR "/src/src/MessageService.Api"
RUN dotnet build "MessageService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessageService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageService.Api.dll"]