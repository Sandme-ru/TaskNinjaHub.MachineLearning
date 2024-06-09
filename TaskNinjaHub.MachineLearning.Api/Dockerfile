# Use an official Python runtime as a parent image
FROM python:3.9-slim AS python-base

# Set the working directory in the container for Python
WORKDIR /app/python

# Copy the current directory contents into the container at /app/python
COPY . /app/python

# Install any needed packages specified in requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# Use an official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

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

# Copy Python environment from the python-base stage
COPY --from=python-base /app/python /app/python
WORKDIR /app/python

# Expose the port the Python app runs on
EXPOSE 5000

# Command to run both applications
CMD ["sh", "-c", "dotnet /app/TaskNinjaHub.MachineLearning.Api.dll & uvicorn main:app --host 0.0.0.0 --port 5000"]
