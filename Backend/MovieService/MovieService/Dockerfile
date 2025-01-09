
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MovieService/MovieService.csproj", "MovieService/"]
COPY ["MovieService.Application/MovieService.Application.csproj", "MovieService.Application/"]
COPY ["MovieService.Core/MovieService.Core.csproj", "MovieService.Core/"]
COPY ["MovieService.DataAccess/MovieService.DataAccess.csproj", "MovieService.DataAccess/"]
RUN dotnet restore "./MovieService/MovieService.csproj"
COPY . .
WORKDIR "/src/MovieService"
RUN dotnet build "./MovieService.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MovieService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieService.dll"]