#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

COPY ["SmartTicketing.Api/SmartTicketing.Api.csproj", "SmartTicketing.Api/"]
COPY ["SmartTicketing.Api.Models/SmartTicketing.Api.Models.csproj", "SmartTicketing.Api.Models/"]
COPY ["SmartTicketing.Application/SmartTicketing.Application.csproj", "SmartTicketing.Application/"]
COPY ["SmartTicketing.Domain/SmartTicketing.Domain.csproj", "SmartTicketing.Domain/"]
COPY ["SmartTicketing.Infrastructure/SmartTicketing.Infrastructure.csproj", "SmartTicketing.Infrastructure/"]
RUN dotnet restore "SmartTicketing.Api/SmartTicketing.Api.csproj"

COPY . .

WORKDIR "/src/SmartTicketing.Api"
RUN dotnet build "SmartTicketing.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartTicketing.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartTicketing.Api.dll"]