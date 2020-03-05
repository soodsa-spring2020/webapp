#!/bin/bash

unzip webapp_144.zip 
cd release
dotnet tool restore
dotnet ef database update --project csye6225
dotnet run --project csye6225