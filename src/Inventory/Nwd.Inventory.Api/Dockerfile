#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Nwd.Inventory.Api/Nwd.Inventory.Api.csproj", "Nwd.Inventory.Api/"]
COPY ["Nwd.Inventory.Domain/Nwd.Inventory.Domain.csproj", "Nwd.Inventory.Domain/"]
COPY ["Nwd.Inventory.Infrastructure/Nwd.Inventory.Infrastructure.csproj", "Nwd.Inventory.Infrastructure/"]
RUN dotnet restore "Nwd.Inventory.Api/Nwd.Inventory.Api.csproj"
COPY . .
WORKDIR "/src/Nwd.Inventory.Api"
RUN dotnet build "Nwd.Inventory.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nwd.Inventory.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nwd.Inventory.Api.dll"]