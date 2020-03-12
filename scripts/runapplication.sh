#!/bin/bash

cd /home/ubuntu/webapp/
dotnet publish -c Release
touch /etc/systemd/system/dotnetcore.service
echo "[Unit]Description=Dot-Net-Core service" > /etc/systemd/system/dotnetcore.service
echo "After=network.target[Service]ExecStart=/usr/bin/dontu/webapp/csye6225/bin/Release/netcore3.0/csye6225.dll" >> /etc/systemd/system/dotnetcore.service
echo "Restart=on-failure" >> /etc/systemd/system/dotnetcore.service
echo "WorkingDirectory=/home/ubuntu/webapp/" >> /etc/systemd/system/dotnetcore.service
echo "User=dotnetuser" >> /etc/systemd/system/dotnetcore.service
echo "Group=dotnetuser[Install]WantedBy=multi-user.target" >> /etc/systemd/system/dotnetcore.service
systemctl start dotnetcore
systemctl enable dotnetcore
systemctl status dotnetcore




