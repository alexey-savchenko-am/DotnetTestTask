# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DotnetTestTask.Api/DotnetTestTask.Api.csproj", "src/DotnetTestTask.Api/"]
COPY ["src/DotnetTestTask.Application/DotnetTestTask.Application.csproj", "src/DotnetTestTask.Application/"]
COPY ["src/SharedKernel.Application/SharedKernel.Application.csproj", "src/SharedKernel.Application/"]
COPY ["src/SharedKernel.Core/SharedKernel.Core.csproj", "src/SharedKernel.Core/"]
COPY ["src/DotnetTestTask.Infrastructure/DotnetTestTask.Infrastructure.csproj", "src/DotnetTestTask.Infrastructure/"]
COPY ["src/DotnetTestTask.Core/DotnetTestTask.Core.csproj", "src/DotnetTestTask.Core/"]
COPY ["src/SharedKernel.Infrastructure/SharedKernel.Infrastructure.csproj", "src/SharedKernel.Infrastructure/"]
RUN dotnet restore "./src/DotnetTestTask.Api/DotnetTestTask.Api.csproj"
COPY . .
WORKDIR "/src/src/DotnetTestTask.Api"
RUN dotnet build "./DotnetTestTask.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DotnetTestTask.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotnetTestTask.Api.dll"]