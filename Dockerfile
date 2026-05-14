FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["nuget.config", "./"]
COPY ["Bajol.GovFlow.sln", "./"]
COPY ["Bajol.GovFlow.API/Bajol.GovFlow.API.csproj", "Bajol.GovFlow.API/"]
COPY ["Bajol.GovFlow.Application/Bajol.GovFlow.Application.csproj", "Bajol.GovFlow.Application/"]
COPY ["Bajol.GovFlow.Domain/Bajol.GovFlow.Domain.csproj", "Bajol.GovFlow.Domain/"]
COPY ["Bajol.GovFlow.Infrastructure/Bajol.GovFlow.Infrastructure.csproj", "Bajol.GovFlow.Infrastructure/"]

RUN dotnet restore "Bajol.GovFlow.API/Bajol.GovFlow.API.csproj"

COPY . .
WORKDIR "/src/Bajol.GovFlow.API"
RUN dotnet publish "Bajol.GovFlow.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bajol.GovFlow.API.dll"]
