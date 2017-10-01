#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd src/Collectively.Services.SignalR
dotnet run --no-restore --urls "http://*:10010"