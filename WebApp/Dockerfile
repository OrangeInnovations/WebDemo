#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApp/WebApp.csproj", "WebApp/"]
COPY ["Demo.Domain.Infrastructure/Demo.Domain.Infrastructure.csproj", "Demo.Domain.Infrastructure/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Demo.Domain/Demo.Domain.csproj", "Demo.Domain/"]
COPY ["Demo.Domain.Services/Demo.Domain.Services.csproj", "Demo.Domain.Services/"]
RUN dotnet restore "WebApp/WebApp.csproj"
COPY . .
WORKDIR "/src/WebApp"
RUN dotnet build "WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.dll"]