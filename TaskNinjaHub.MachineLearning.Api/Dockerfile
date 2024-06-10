# Используем официальный образ Python как базовый
FROM python:3.9-slim AS python-base

# Устанавливаем рабочую директорию для Python
WORKDIR /app/python

# Копируем содержимое текущей директории в контейнер в /app/python
COPY . /app/python

# Проверяем наличие requirements.txt и устанавливаем зависимости, если файл существует
RUN if [ -f requirements.txt ]; then pip install --no-cache-dir -r requirements.txt; fi

# Используем официальный образ .NET runtime как базовый
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем файлы проекта для .NET API и приложения
COPY TaskNinjaHub.MachineLearning.Api/TaskNinjaHub.MachineLearning.Api.csproj TaskNinjaHub.MachineLearning.Api/
COPY TaskNinjaHub.MachineLearning.Application/TaskNinjaHub.MachineLearning.Application.csproj TaskNinjaHub.MachineLearning.Application/

# Восстанавливаем зависимости для .NET проекта
RUN dotnet restore TaskNinjaHub.MachineLearning.Api/TaskNinjaHub.MachineLearning.Api.csproj

# Копируем остальные файлы и директории
COPY . .

# Сборка .NET проекта
WORKDIR /src/TaskNinjaHub.MachineLearning.Api
RUN dotnet build TaskNinjaHub.MachineLearning.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish TaskNinjaHub.MachineLearning.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Копируем окружение Python из стадии python-base
COPY --from=python-base /app/python /app/python
WORKDIR /app/python

# Устанавливаем Uvicorn и другие зависимости для Python сервера
RUN pip install uvicorn

# Открываем порт, на котором работает Python приложение
EXPOSE 5000

# Команда для запуска обоих приложений
CMD ["sh", "-c", "dotnet /app/TaskNinjaHub.MachineLearning.Api.dll & uvicorn main:app --host 0.0.0.0 --port 5000"]
