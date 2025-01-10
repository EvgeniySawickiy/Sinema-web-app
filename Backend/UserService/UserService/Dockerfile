
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081



FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserService/UserService.API.csproj", "UserService/"]
COPY ["UserService.BLL/UserService.BLL.csproj", "UserService.BLL/"]
COPY ["UserService.DAL/UserService.DAL.csproj", "UserService.DAL/"]
RUN dotnet restore "./UserService/UserService.API.csproj"
COPY . .
WORKDIR "/src/UserService"
RUN dotnet build "./UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]