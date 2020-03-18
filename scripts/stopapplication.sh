#!/bin/bash

rm -rf /home/ubuntu/webapp/
if (systemctl -q is-active dotnetcore.service)
    then
    systemctl stop dotnetcore

fi