# BobNetEverywhere

Run template to show how to run the same .Net Core 5.0 project in 
* Web: 
	* nginx on Ubuntu
	* docker on Centos (see https://bobcalypso.com/index.html)
	* IIS web on Windows

* Desktop:
	* Windows in Electron
	* Ubuntu in Electron

Using
* https://github.com/ElectronNET/Electron.NET
* https://github.com/axios/axios
* https://getbootstrap.com/
* https://vuejs.org/
* https://www.chartjs.org/


dotnet publish --configuration Release
BobNetEverywhere\bobweb\bin\Release\net5.0\publish
sudo chown www-data:www-data /var/www/bobelectron

Add to nginx /etc/nginx/sites-available/default
```
    location / {
        proxy_pass         http://localhost:6000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}
```

/etc/systemd/system/bob-calypso.service
```
[Unit]
Description=Bob Calypsos .Net Everywhere

[Service]
WorkingDirectory=/home/inctrakmanga/dotnet/bobweb
ExecStart=/usr/bin/dotnet /home/inctrakmanga/dotnet/bobweb/bobweb.dll --urls=http://localhost:6000
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=bob-calypso
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```
Enable/start service
```
sudo systemctl enable bob-calypso.service
sudo systemctl start bob-calypso.service
sudo systemctl status bob-calypso.service
```

Build/run a Docker w/ nginx and letsencrypt
```
docker build -t bobcalypsoweb .
docker rmi $(docker images -f "dangling=true" -q)
docker save bobcalypsoweb > /tmp/bobcalypsoweb.tar
docker load < bobcalypsoweb.tar
docker-compose up -d
```
