#!/bin/bash

cd /home/ubuntu/webapp
dotnet publish -c Release
if [ ! -f /etc/systemd/system/dotnetcore.service ]; then
    touch /etc/systemd/system/dotnetcore.service
    echo "[Unit]" > /etc/systemd/system/dotnetcore.service
    echo "Description=Dot-Net-Core service" >> /etc/systemd/system/dotnetcore.service
    echo "After=network.target" >> /etc/systemd/system/dotnetcore.service
    echo "[Service]" >> /etc/systemd/system/dotnetcore.service
    echo "ExecStart=/usr/bin/dotnet /home/ubuntu/webapp/csye6225/bin/Release/netcoreapp3.0/csye6225.dll" >> /etc/systemd/system/dotnetcore.service
    echo "WorkingDirectory=/home/ubuntu/webapp/" >> /etc/systemd/system/dotnetcore.service
    echo "User=ubuntu" >> /etc/systemd/system/dotnetcore.service
    echo "Group=ubuntu" >> /etc/systemd/system/dotnetcore.service
    echo "[Install]" >> /etc/systemd/system/dotnetcore.service
    echo "WantedBy=multi-user.target" >> /etc/systemd/system/dotnetcore.service
    systemctl enable dotnetcore
    systemctl start dotnetcore
fi
systemctl restart dotnetcore






