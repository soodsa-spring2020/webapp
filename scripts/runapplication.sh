#!/bin/bash

cd /home/ubuntu/webapp/
if [ ! -f /etc/systemd/system/dotnetcore.service ]; then
    touch /etc/systemd/system/dotnetcore.service
    echo "[Unit]" > /etc/systemd/system/dotnetcore.service
    echo "Description=Dot-Net-Core service" >> /etc/systemd/system/dotnetcore.service
    echo "After=network.target" >> /etc/systemd/system/dotnetcore.service
    echo "[Service]" >> /etc/systemd/system/dotnetcore.service
    echo "ExecStart=/usr/bin/dotnet run --project /home/ubuntu/webapp/csye6225" >> /etc/systemd/system/dotnetcore.service
    echo "Restart=on-failure" >> /etc/systemd/system/dotnetcore.service
    echo "WorkingDirectory=/home/ubuntu/webapp/" >> /etc/systemd/system/dotnetcore.service
    echo "User=ubuntu" >> /etc/systemd/system/dotnetcore.service
    echo "Group=ubuntu" >> /etc/systemd/system/dotnetcore.service
    echo "[Install]" >> /etc/systemd/system/dotnetcore.service
    echo "WantedBy=multi-user.target" >> /etc/systemd/system/dotnetcore.service
fi
systemctl start dotnetcore.service
systemctl enable dotnetcore.service





