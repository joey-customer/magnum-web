#!/bin/bash
dotnet build
coverlet ./MagnumTest/bin/Debug/netcoreapp2.2/MagnumTest.dll --target "dotnet" --targetargs "test . --no-build" --format lcov
