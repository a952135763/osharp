FROM microsoft/dotnet:3.0-aspnetcore-runtime AS base
MAINTAINER KAPAI kapai@kapai.me

WORKDIR /app
EXPOSE 80

COPY ./bin/Release/netcoreapp3.0/publish /app
ENTRYPOINT ["dotnet", "KaPai.Pay.Web.dll"]
