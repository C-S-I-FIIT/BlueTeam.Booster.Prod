# Bc.Syslog-ng.API.Configurator

## Install dotnet Ubuntu 20.04
1. `wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb`
2. `sudo dpkg -i packages-microsoft-prod.deb`
3. `sudo apt-get update`
4. `sudo apt-get install -y dotnet-sdk-7.0`
5. `dotnet --version`
6. `sudo apt-get install -y aspnetcore-runtime-5.0`

## Deployment
1. Install nginx `sudo apt-get install -y nginx`
2. ensure that application has needed privileges to change the file /etc/syslog-ng/conf.d/uc-filter/uc-filters-definition.conf
