FROM microsoft/dotnet:2.0.0-runtime
WORKDIR /dotnetapp
COPY ./bin/Docker .
ENV ASPNETCORE_URLS http://*:10010
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet Collectively.Services.SignalR.dll