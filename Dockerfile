# builds our image using dotnet's sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source
COPY ./src/PlexLibraryMonitor ./PlexLibraryMonitor/
WORKDIR /source/PlexLibraryMonitor
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

# runs it using aspnet runtime
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "PlexLibraryMonitor.dll"]