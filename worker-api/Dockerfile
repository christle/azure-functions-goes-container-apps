FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS installer-env

COPY api/worker-api.csproj /src/dotnet-function-app/api/worker-api.csproj
WORKDIR /src/dotnet-function-app/api
RUN dotnet restore

COPY api /src/dotnet-function-app/api

RUN cd /src/dotnet-function-app/api && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot --no-restore

FROM mcr.microsoft.com/azure-functions/dotnet:3.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

ENV AzureWebJobsDisableHomepage="true"    
COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]