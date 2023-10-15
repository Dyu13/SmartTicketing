#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SmartTicketing.Ai/SmartTicketing.Ai.csproj", "SmartTicketing.Ai/"]
RUN dotnet restore "SmartTicketing.Ai/SmartTicketing.Ai.csproj"
COPY . .
WORKDIR "/src/SmartTicketing.Ai"
RUN dotnet build "SmartTicketing.Ai.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartTicketing.Ai.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/wait-for-it.sh", "rabbitmq:5672", "-t", "120", "--", "dotnet", "SmartTicketing.Ai.dll"]