#!/bin/bash

#Be careful to set this file without \r

WORK_DIR=/magnum
SOLUTION_DIR=$WORK_DIR/magnum-web

mkdir -p ${WORK_DIR}
cd ${WORK_DIR}
git clone https://github.com/pjamenaja/magnum-web.git
cd ${SOLUTION_DIR}
git checkout ${COMMIT_SHA} -b ${COMMIT_SHA}

dotnet sonarscanner begin \
    /key:pjamenaja_magnum_web \
    /o:pjamenaja \
    /v:${BUILT_VERSION} \
    /d:sonar.host.url=https://sonarcloud.io \
    /d:sonar.branch.name=${BRANCH_NAME} \
    /d:sonar.cs.opencover.reportsPaths=./coverage.opencover.xml \
    /d:sonar.javascript.exclusions=**/bootstrap/**,**/jquery/**,**/jquery-validation/**,**/jquery-validation-unobtrusive/** \
    /d:sonar.verbose=true \
    /d:sonar.scm.provider=git \
    /d:sonar.login=${SONAR_KEY}

dotnet build Magnum.sln

#coverlet './MagnumTest/bin/Debug/netcoreapp2.2/MagnumTest.dll' --target 'dotnet' --targetargs 'test . --no-build' --format opencover

dotnet sonarscanner end /d:sonar.login=${SONAR_KEY}
