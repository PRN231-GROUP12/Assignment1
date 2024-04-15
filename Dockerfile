FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /app

# Copy only the solution file and project files necessary for the build
COPY *.sln .
COPY PRN231_Group12.Assignment1.API/*.csproj ./src/API/
COPY PRN231_Group12.Assignment1.Repo/*.csproj ./src/Repository/
RUN dotnet restore ./src/API/*.csproj

COPY . .
RUN dotnet build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "PRN231_Group12.Assignment1.API.dll" ]