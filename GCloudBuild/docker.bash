#!/bin/bash

cd /workspace

PACAKGE=pjamenaja/magnum-web:${DOCKER_VERSION}

docker build --build-arg VERSION_NUMBER=${BUILT_VERSION} -f ./GCloudBuild/Dockerfile -t ${PACAKGE} ../..
#docker login -u ${DOCKER_HUB_USERNAME} -p ${DOCKER_HUB_PASSWORD} docker.io
#docker push ${PACAKGE}
