#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Plibmon/Server/Plibmon.Server.csproj", "Plibmon/Server/"]
COPY ["src/Plibmon/Client/Plibmon.Client.csproj", "Plibmon/Client/"]
COPY ["src/Plibmon/Shared/Plibmon.Shared.csproj", "Plibmon/Shared/"]
COPY ["src/Plibmon/Domain/Plibmon.Domain.csproj", "Plibmon/Domain/"]
RUN dotnet restore "Plibmon/Server/Plibmon.Server.csproj"
COPY src/. .
WORKDIR "/src/Plibmon/Server"
RUN dotnet build "Plibmon.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Plibmon.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Plibmon.Server.dll"]