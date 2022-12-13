#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Orders/Nwd.Orders.Api/Nwd.Orders.Api.csproj", "Orders/Nwd.Orders.Api/"]
COPY ["Orders/Nwd.Orders.Application/Nwd.Orders.Application.csproj", "Orders/Nwd.Orders.Application/"]
COPY ["BuildingBlocks/Nws.BuildingBlocks/Nws.BuildingBlocks.csproj", "BuildingBlocks/Nws.BuildingBlocks/"]
COPY ["Orders/Nwd.Orders.Domain/Nwd.Orders.Domain.csproj", "Orders/Nwd.Orders.Domain/"]
COPY ["Orders/Nwd.Orders.Infrastructure/Nwd.Orders.Infrastructure.csproj", "Orders/Nwd.Orders.Infrastructure/"]
RUN dotnet restore "Orders/Nwd.Orders.Api/Nwd.Orders.Api.csproj"
COPY . .
WORKDIR "/src/Orders/Nwd.Orders.Api"
RUN dotnet build "Nwd.Orders.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nwd.Orders.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nwd.Orders.Api.dll"]