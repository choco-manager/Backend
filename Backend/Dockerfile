FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ARG ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=$ENVIRONMENT

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Backend.csproj", "Backend/"]
RUN dotnet restore "Backend/Backend.csproj"
WORKDIR "/src/Backend"
COPY . .
RUN dotnet build "Backend.csproj" -o /app/build

FROM build AS publish
RUN apt-get update && apt-get install -y libfreetype6 libfontconfig1
RUN dotnet publish "Backend.csproj" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]