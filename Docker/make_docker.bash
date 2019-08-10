#!/bin/bash

docker build -f ./Dockerfile -t pjamenaja/magnum-web ..

docker login -u ${DOCKER_HUB_USERNAME} -p ${DOCKER_HUB_PASSWORD} docker.io

docker push pjamenaja/magnum-web:${DOCKER_VERSION}

docker rm $(docker ps -qa --no-trunc --filter "status=exited")
docker rmi $(docker images | grep "none" | awk '/ / { print $3 }')
