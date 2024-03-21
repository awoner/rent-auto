FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 9005

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RentAutoPoc.Api/RentAutoPoc.Api.csproj", "RentAutoPoc.Api/"]
RUN dotnet restore "RentAutoPoc.Api/RentAutoPoc.Api.csproj"
COPY . .
WORKDIR "/src/RentAutoPoc.Api"
RUN dotnet build "RentAutoPoc.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RentAutoPoc.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RentAutoPoc.Api.dll"]
