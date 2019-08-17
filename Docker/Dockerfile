FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

ARG VERSION_NUMBER

EXPOSE 80
EXPOSE 443

COPY Magnum.sln ./publish/
COPY MagnumWeb/. ./publish/MagnumWeb
COPY MagnumCore/. ./publish/MagnumCore
COPY MagnumConsole/. ./publish/MagnumConsole
COPY MagnumTest/. ./publish/MagnumTest

WORKDIR /app/publish

RUN dotnet restore
RUN dotnet publish -c Release -o out --version-suffix $VERSION_NUMBER

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/publish/MagnumWeb/out ./

ENTRYPOINT ["dotnet", "MagnumWeb.dll"]
