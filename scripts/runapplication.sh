#!/bin/bash

cd /home/ubuntu/webapp/
dotnet build
if [ ! -f /etc/systemd/system/dotnetcore.service ]; then
    touch /etc/systemd/system/dotnetcore.service
    echo "[Unit]" > /etc/systemd/system/dotnetcore.service
    echo "Description=Dot-Net-Core service" >> /etc/systemd/system/dotnetcore.service
    echo "After=network.target" >> /etc/systemd/system/dotnetcore.service
    echo "[Service]" >> /etc/systemd/system/dotnetcore.service
    echo "Type=oneshot" >> /etc/systemd/system/dotnetcore.service
    echo "ExecStart=/usr/bin/dotnet run --project /home/ubuntu/webapp/csye6225/" >> /etc/systemd/system/dotnetcore.service
    echo "WorkingDirectory=/home/ubuntu/webapp/" >> /etc/systemd/system/dotnetcore.service
    echo "User=ubuntu" >> /etc/systemd/system/dotnetcore.service
    echo "Group=dotnetuser" >> /etc/systemd/system/dotnetcore.service
    echo "PrivateTemp=true" >> /etc/systemd/system/dotnetcore.service
    echo "[Install]" >> /etc/systemd/system/dotnetcore.service
    echo "WantedBy=multi-user.target" >> /etc/systemd/system/dotnetcore.service
    systemctl start dotnetcore
    systemctl enable dotnetcore
fi
systemctl restart dotnetcore
