#!/bin/bash

unzip webapp.zip 
dotnet tool restore
dotnet ef database update csye6225.dll
dotnet run csye6225.dll