#!/bin/bash

PACAKGE=pjamenaja/magnum-web:${DOCKER_VERSION}

docker build --build-arg VERSION_NUMBER=${DOCKER_VERSION} -f ./Dockerfile -t ${PACAKGE} ..
docker login -u ${DOCKER_HUB_USERNAME} -p ${DOCKER_HUB_PASSWORD} docker.io
docker push ${PACAKGE}

docker rm $(docker ps -qa --no-trunc --filter "status=exited")
docker rmi $(docker images | grep "none" | awk '/ / { print $3 }')
