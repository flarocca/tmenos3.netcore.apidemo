#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/TMenos3.NetCore.ApiDemo.API/TMenos3.NetCore.ApiDemo.API.csproj", "src/TMenos3.NetCore.ApiDemo.API/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Contracts/TMenos3.NetCore.ApiDemo.Contracts.csproj", "src/TMenos3.NetCore.ApiDemo.Contracts/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.csproj", "src/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Services/TMenos3.NetCore.ApiDemo.Services.csproj", "src/TMenos3.NetCore.ApiDemo.Services/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Models/TMenos3.NetCore.ApiDemo.Models.csproj", "src/TMenos3.NetCore.ApiDemo.Models/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Database/TMenos3.NetCore.ApiDemo.Database.csproj", "src/TMenos3.NetCore.ApiDemo.Database/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ.csproj", "src/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Infrastructure/TMenos3.NetCore.ApiDemo.Infrastructure.csproj", "src/TMenos3.NetCore.ApiDemo.Infrastructure/"]
RUN dotnet restore "src/TMenos3.NetCore.ApiDemo.API/TMenos3.NetCore.ApiDemo.API.csproj"
COPY . .
WORKDIR "/src/src/TMenos3.NetCore.ApiDemo.API"
RUN dotnet build "TMenos3.NetCore.ApiDemo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TMenos3.NetCore.ApiDemo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TMenos3.NetCore.ApiDemo.API.dll"]
