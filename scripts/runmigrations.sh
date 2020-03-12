#!/bin/bash

cd ./deployment-root/$DEPLOYMENT_GROUP_ID/$DEPLOYMENT_ID/deployment-archive/
dotnet ef database update --project csye6225