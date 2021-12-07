FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS installer-env

COPY hub/position-handler.csproj /src/dotnet-function-app/hub/position-handler.csproj
WORKDIR /src/dotnet-function-app/hub
RUN dotnet restore

COPY hub /src/dotnet-function-app/hub

RUN cd /src/dotnet-function-app/hub && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot --no-restore

FROM mcr.microsoft.com/azure-functions/dotnet:3.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

ENV AzureWebJobsDisableHomepage="true"    
COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]