FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Choco.Backend.Api/Choco.Backend.Api.csproj", "Choco.Backend.Api/"]
RUN dotnet restore "Choco.Backend.Api/Choco.Backend.Api.csproj"
COPY . .
WORKDIR "/src/Choco.Backend.Api"
RUN dotnet build "Choco.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Choco.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Choco.Backend.Api.dll"]
