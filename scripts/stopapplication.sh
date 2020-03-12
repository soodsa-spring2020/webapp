#!/bin/bash

if (systemctl -q is-active dotnetcore.service)
    then
    systemctl stop dotnetcore
    rm -rf /home/ubuntu/webapp
fi