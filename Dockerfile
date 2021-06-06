#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY ["./src/Evento.Api/Evento.Api.csproj", "Evento.Api/"]
RUN dotnet restore "Evento.Api/Evento.Api.csproj"
COPY . .
RUN dotnet build "./src/Evento.Api/Evento.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./src/Evento.Api/Evento.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# To override default ports
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_HTTPS_PORT=https://+:5001
ENTRYPOINT ["dotnet", "Evento.Api.dll"]