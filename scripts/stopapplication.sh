#!/bin/bash

rm -rf /var/www/webapp/
if (systemctl -q is-active dotnetcore.service)
    then
    systemctl stop dotnetcore
fi