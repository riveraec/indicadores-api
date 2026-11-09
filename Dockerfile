FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/Indicadores.Api/Indicadores.Api.csproj Indicadores.Api/
RUN dotnet restore Indicadores.Api/Indicadores.Api.csproj
COPY src/Indicadores.Api/ Indicadores.Api/
RUN dotnet publish Indicadores.Api/Indicadores.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Indicadores.Api.dll"]
