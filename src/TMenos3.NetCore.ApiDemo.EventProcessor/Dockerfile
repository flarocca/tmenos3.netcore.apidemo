FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/TMenos3.NetCore.ApiDemo.EventProcessor/TMenos3.NetCore.ApiDemo.EventProcessor.csproj", "src/TMenos3.NetCore.ApiDemo.EventProcessor/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Contracts/TMenos3.NetCore.ApiDemo.Contracts.csproj", "src/TMenos3.NetCore.ApiDemo.Contracts/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.csproj", "src/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Services/TMenos3.NetCore.ApiDemo.Services.csproj", "src/TMenos3.NetCore.ApiDemo.Services/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Models/TMenos3.NetCore.ApiDemo.Models.csproj", "src/TMenos3.NetCore.ApiDemo.Models/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Database/TMenos3.NetCore.ApiDemo.Database.csproj", "src/TMenos3.NetCore.ApiDemo.Database/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ.csproj", "src/TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ/"]
COPY ["src/TMenos3.NetCore.ApiDemo.Infrastructure/TMenos3.NetCore.ApiDemo.Infrastructure.csproj", "src/TMenos3.NetCore.ApiDemo.Infrastructure/"]
RUN dotnet restore "src/TMenos3.NetCore.ApiDemo.EventProcessor/TMenos3.NetCore.ApiDemo.EventProcessor.csproj"
COPY . .
WORKDIR "/src/src/TMenos3.NetCore.ApiDemo.EventProcessor"
RUN dotnet build "TMenos3.NetCore.ApiDemo.EventProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TMenos3.NetCore.ApiDemo.EventProcessor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TMenos3.NetCore.ApiDemo.EventProcessor.dll"]
