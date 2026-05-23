# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install required packages in the base/final stage (where the app runs)


USER root
 
RUN apt-get update && apt-get install -y \
    mdbtools \
    odbc-mdbtools \
    unixodbc \
    unixodbc-dev \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get clean
USER app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# These packages are only needed for building (if you have any build-time dependencies)
RUN apt-get update && apt-get install -y \
    mdbtools \
    odbc-mdbtools \
    unixodbc \
    unixodbc-dev \
    && rm -rf /var/lib/apt/lists/*

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["readAccess/readAccess.csproj", "readAccess/"]
RUN dotnet restore "./readAccess/readAccess.csproj"
COPY . .
WORKDIR "/src/readAccess"
RUN dotnet build "./readAccess.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./readAccess.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure the app user has access to ODBC (already handled in base stage)
ENTRYPOINT ["dotnet", "readAccess.dll"]
