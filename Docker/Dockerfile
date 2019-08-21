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
RUN dotnet publish -c Release -o out -p:Version=$VERSION_NUMBER 

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app

RUN  apt-get update \
  && apt-get install -y wget \
  && rm -rf /var/lib/apt/lists/*

RUN wget https://artifacts.elastic.co/downloads/beats/filebeat/filebeat-7.3.0-amd64.deb
RUN dpkg -i filebeat-7.3.0-amd64.deb
RUN rm filebeat-7.3.0-amd64.deb

COPY --from=build /app/publish/MagnumWeb/out ./
COPY Docker/filebeat.yml /etc/filebeat/filebeat.yml

RUN mkdir -p /logs

ENTRYPOINT ["service filebeat start;", "dotnet", "MagnumWeb.dll"]
