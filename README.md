# BobNetEverywhere

Run template to show how to run the same .Net Core 5.0 project in 
Web: 
	nginx on Ubuntu
	docker on Centos
	IIS web on Windows

Desktop:
	Windows in Electron
	Ubuntu in Electron


https://github.com/ElectronNET/Electron.NET
https://github.com/axios/axios
https://getbootstrap.com/
https://vuejs.org/
https://www.chartjs.org/

Add to nginx
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

