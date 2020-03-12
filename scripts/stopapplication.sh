#!/bin/bash

if (systemctl -q is-active dotnetcore.service)
    then
    systemctl stop dotnetcore.service
fi