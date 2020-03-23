#!/bin/bash

cd /var/www/webapp/
dotnet publish -c Release
mkdir -m 777 /var/www/webapp/csye6225/tmp/
touch /etc/systemd/system/dotnetcore.service
echo "[Unit]" > /etc/systemd/system/dotnetcore.service
echo "Description=Dot-Net-Core service" >> /etc/systemd/system/dotnetcore.service
echo "After=network.target" >> /etc/systemd/system/dotnetcore.service
echo "[Service]" >> /etc/systemd/system/dotnetcore.service
echo "ExecStart=/usr/bin/dotnet /var/www/webapp/csye6225/bin/Release/netcoreapp3.0/publish/csye6225.dll" >> /etc/systemd/system/dotnetcore.service
echo "WorkingDirectory=/var/www/webapp/" >> /etc/systemd/system/dotnetcore.service
echo "User=root" >> /etc/systemd/system/dotnetcore.service
echo "Group=root" >> /etc/systemd/system/dotnetcore.service
echo "EnvironmentFile=/etc/environment/" >> /etc/systemd/system/dotnetcore.service
echo "[Install]" >> /etc/systemd/system/dotnetcore.service
echo "WantedBy=multi-user.target" >> /etc/systemd/system/dotnetcore.service
systemctl start dotnetcore
systemctl enable dotnetcore
