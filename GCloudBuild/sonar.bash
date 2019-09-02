#!/bin/bash

#Be careful to set this file without \r

dotnet sonarscanner begin \
    /key:pjamenaja_magnum_web \
    /o:pjamenaja \
    /d:sonar.host.url=https://sonarcloud.io \
    /d:sonar.login=6b3024e6cc1576028a5bba761b1009ffb850b6f5

cd /workspace

dotnet build Magnum.sln

dotnet sonarscanner end /d:sonar.login=6b3024e6cc1576028a5bba761b1009ffb850b6f5
