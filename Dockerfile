FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CMMS_System.sln ./
COPY CMMS.WebAPI/CMMS.WebAPI.csproj CMMS.WebAPI/
COPY CMMS.BLL/CMMS.BLL.csproj CMMS.BLL/
COPY CMMS.DAL/CMMS.DAL.csproj CMMS.DAL/

RUN dotnet restore CMMS.WebAPI/CMMS.WebAPI.csproj

COPY CMMS.WebAPI/ CMMS.WebAPI/
COPY CMMS.BLL/ CMMS.BLL/
COPY CMMS.DAL/ CMMS.DAL/

RUN dotnet publish CMMS.WebAPI/CMMS.WebAPI.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN adduser --disabled-password --gecos "" --uid 1000 appuser \
    && chown -R appuser:appuser /app
USER appuser

COPY --from=build --chown=appuser:appuser /app/publish .

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_EnableDiagnostics=0

EXPOSE 8080

ENTRYPOINT ["dotnet", "CMMS.WebAPI.dll"]
