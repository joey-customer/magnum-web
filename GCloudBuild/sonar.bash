#!/bin/bash

#Be careful to set this file without \r

dotnet sonarscanner begin \
    /key:pjamenaja_magnum_web \
    /o:pjamenaja \
    /d:sonar.host.url=https://sonarcloud.io \
    /d:sonar.login=${SONAR_KEY}

cd /workspace

dotnet build Magnum.sln

dotnet sonarscanner end /d:sonar.login=${SONAR_KEY}
