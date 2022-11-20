#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Nwd.Orders.Api/Nwd.Orders.Api.csproj", "Nwd.Orders.Api/"]
COPY ["Nwd.Orders.Domain/Nwd.Orders.Domain.csproj", "Nwd.Orders.Domain/"]
COPY ["Nwd.Orders.Infrastructure/Nwd.Orders.Infrastructure.csproj", "Nwd.Orders.Infrastructure/"]
RUN dotnet restore "Nwd.Orders.Api/Nwd.Orders.Api.csproj"
COPY . .
WORKDIR "/src/Nwd.Orders.Api"
RUN dotnet build "Nwd.Orders.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nwd.Orders.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nwd.Orders.Api.dll"]