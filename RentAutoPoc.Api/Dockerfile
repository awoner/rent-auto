FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 9005

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RentAutoPoc.Api/RentAutoPoc.Api.csproj", "RentAutoPoc.Api/"]
COPY ["RentAutoPoc.Api/Resources/", "RentAutoPoc.Api/Resources/"]
RUN dotnet restore "RentAutoPoc.Api/RentAutoPoc.Api.csproj"
COPY . .
WORKDIR "/src/RentAutoPoc.Api"
RUN dotnet build "RentAutoPoc.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RentAutoPoc.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/publish/Resources/Images /app/Resources/Images
ENTRYPOINT ["dotnet", "RentAutoPoc.Api.dll"]
