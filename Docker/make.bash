#!/bin/bash

PACAKGE=pjamenaja/magnum-web:latest

#docker rm $(docker ps -qa --no-trunc --filter "status=exited")
#docker rmi $(docker images | grep "none" | awk '/ / { print $3 }')

docker build --build-arg VERSION_NUMBER=1.0.0.1 -f ./Dockerfile -t ${PACAKGE} ..

