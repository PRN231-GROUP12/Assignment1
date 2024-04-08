FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PRN231_Group12.Assignment1.API/PRN231_Group12.Assignment1.API.csproj", "PRN231_Group12.Assignment1.API/"]
RUN dotnet restore "PRN231_Group12.Assignment1.API/PRN231_Group12.Assignment1.API.csproj"
COPY . .
WORKDIR "/src/PRN231_Group12.Assignment1.API"
RUN dotnet build "PRN231_Group12.Assignment1.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PRN231_Group12.Assignment1.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRN231_Group12.Assignment1.API.dll"]
