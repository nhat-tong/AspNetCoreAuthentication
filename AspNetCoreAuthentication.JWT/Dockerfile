FROM microsoft/aspnetcore:2.0 AS base

WORKDIR /app

COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet AspNetCoreAuthentication.JWT.dll
