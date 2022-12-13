#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Basket/Nwd.Basket.Api/Nwd.Basket.Api.csproj", "Basket/Nwd.Basket.Api/"]
RUN dotnet restore "Basket/Nwd.Basket.Api/Nwd.Basket.Api.csproj"
COPY . .
WORKDIR "/src/Basket/Nwd.Basket.Api"
RUN dotnet build "Nwd.Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nwd.Basket.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nwd.Basket.Api.dll"]