#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IFermerAnalyticsService/IFermerAnalyticsService.csproj", "IFermerAnalyticsService/"]
RUN dotnet restore "IFermerAnalyticsService/IFermerAnalyticsService.csproj"
COPY . .
WORKDIR "/src/IFermerAnalyticsService"
RUN dotnet build "IFermerAnalyticsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IFermerAnalyticsService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
expose 8089
ENTRYPOINT ["dotnet", "IFermerAnalyticsService.dll", "--server.urls", "htpp://0.0.0.0:5000"]
