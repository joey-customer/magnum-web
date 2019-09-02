#!/bin/bash

#Be careful to set this file without \r

cd /workspace

dotnet sonarscanner begin \
    /key:pjamenaja_magnum_web \
    /o:pjamenaja \
    /v:${BUILT_VERSION} \
    /d:sonar.host.url=https://sonarcloud.io \
    /d:sonar.branch.name=${BRANCH_NAME} \
    /d:sonar.cs.opencover.reportsPaths=./coverage.opencover.xml \
    /d:sonar.javascript.exclusions=**/bootstrap/**,**/jquery/**,**/jquery-validation/**,**/jquery-validation-unobtrusive/** \
    /d:sonar.login=${SONAR_KEY}

dotnet build Magnum.sln

dotnet sonarscanner end /d:sonar.login=${SONAR_KEY}
