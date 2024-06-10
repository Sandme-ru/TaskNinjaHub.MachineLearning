# Используем официальный образ .NET runtime как базовый
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Установка Python 2.7 в базовый образ
RUN apt-get update -y && apt-get install python2.7 -y

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaskNinjaHub.MachineLearning.Api/TaskNinjaHub.MachineLearning.Api.csproj", "TaskNinjaHub.MachineLearning.Api/"]
COPY ["TaskNinjaHub.MachineLearning.Application/TaskNinjaHub.MachineLearning.Application.csproj", "TaskNinjaHub.MachineLearning.Application/"]
RUN dotnet restore "./TaskNinjaHub.MachineLearning.Api/TaskNinjaHub.MachineLearning.Api.csproj"
COPY . .
WORKDIR "/src/TaskNinjaHub.MachineLearning.Api"
RUN dotnet build "./TaskNinjaHub.MachineLearning.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskNinjaHub.MachineLearning.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskNinjaHub.MachineLearning.Api.dll"]