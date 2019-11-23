#!/bin/bash
dotnet build
coverlet ./MagnumTest/bin/Debug/netcoreapp3.0/MagnumTest.dll --target "dotnet" --targetargs "test . --no-build" --format lcov
