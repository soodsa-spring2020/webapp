#!/bin/bash

cd /var/www/webapp/
dotnet build
mkdir -m 777 /var/www/webapp/csye6225/tmp/
touch /etc/systemd/system/dotnetcore.service
echo "[Unit]" > /etc/systemd/system/dotnetcore.service
echo "Description=Dot-Net-Core service" >> /etc/systemd/system/dotnetcore.service
echo "After=network.target" >> /etc/systemd/system/dotnetcore.service
echo "[Service]" >> /etc/systemd/system/dotnetcore.service
echo "ExecStart=/usr/bin/dotnet run --project /var/www/webapp/csye6225" >> /etc/systemd/system/dotnetcore.service
echo "WorkingDirectory=/var/www/webapp/" >> /etc/systemd/system/dotnetcore.service
echo "User=root" >> /etc/systemd/system/dotnetcore.service
echo "Group=root" >> /etc/systemd/system/dotnetcore.service
echo "EnvironmentFile=/etc/environment/" >> /etc/systemd/system/dotnetcore.service
echo "[Install]" >> /etc/systemd/system/dotnetcore.service
echo "WantedBy=multi-user.target" >> /etc/systemd/system/dotnetcore.service
systemctl daemon-reload
systemctl start dotnetcore
systemctl enable dotnetcore
