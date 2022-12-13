#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG APPVERSION=0.0.0.1
WORKDIR /src
COPY ["Inventory/Nwd.Inventory.Api/Nwd.Inventory.Api.csproj", "Inventory/Nwd.Inventory.Api/"]
COPY ["Inventory/Nwd.Inventory.Application/Nwd.Inventory.Application.csproj", "Inventory/Nwd.Inventory.Application/"]
COPY ["BuildingBlocks/Nws.BuildingBlocks/Nws.BuildingBlocks.csproj", "BuildingBlocks/Nws.BuildingBlocks/"]
COPY ["Inventory/Nwd.Inventory.Domain/Nwd.Inventory.Domain.csproj", "Inventory/Nwd.Inventory.Domain/"]
COPY ["Inventory/Nwd.Inventory.Infrastructure/Nwd.Inventory.Infrastructure.csproj", "Inventory/Nwd.Inventory.Infrastructure/"]
RUN dotnet restore "Inventory/Nwd.Inventory.Api/Nwd.Inventory.Api.csproj"
COPY . .
WORKDIR "/src/Inventory/Nwd.Inventory.Api"
RUN dotnet build "Nwd.Inventory.Api.csproj" -c Release -o /app/build -p:Version=$APPVERSION

FROM build AS publish
RUN dotnet publish "Nwd.Inventory.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nwd.Inventory.Api.dll"]