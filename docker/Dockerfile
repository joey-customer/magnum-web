FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

ARG VERSION_NUMBER

EXPOSE 80
EXPOSE 443

COPY Magnum.sln ./publish/
COPY MagnumWeb/. ./publish/MagnumWeb
COPY MagnumConsole/. ./publish/MagnumConsole
COPY MagnumTest/. ./publish/MagnumTest

WORKDIR /app/publish

RUN dotnet restore
RUN dotnet publish -c Release -o out -p:Version=$VERSION_NUMBER 

FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS runtime
WORKDIR /app

RUN  apt-get update \
  && apt-get install -y wget \
  && apt-get install -y supervisor \
  && mkdir -p /var/log/supervisor \
  && mkdir -p /etc/supervisor/conf.d \
  && rm -rf /var/lib/apt/lists/*

RUN wget https://artifacts.elastic.co/downloads/beats/filebeat/filebeat-7.3.0-amd64.deb
RUN dpkg -i filebeat-7.3.0-amd64.deb
RUN rm filebeat-7.3.0-amd64.deb

COPY --from=build /app/publish/out ./
COPY docker/filebeat.yml /etc/filebeat/filebeat.yml
COPY docker/supervisor.conf /etc/supervisor.conf
COPY configs/. ./configs/

RUN mkdir -p /logs

CMD ["supervisord", "-c", "/etc/supervisor.conf"]
