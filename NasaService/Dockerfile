#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NasaService/NasaService.csproj", "NasaService/"]
RUN dotnet restore "NasaService/NasaService.csproj"
COPY . .
WORKDIR "/src/NasaService"
RUN dotnet build "NasaService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NasaService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NasaService.dll"]