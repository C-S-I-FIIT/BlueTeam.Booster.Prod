## Initialization REST API Server application
First ensure that software package `inotifywait` and `syslog-ng` are installed on host server.
After that clone source code from Github. You can do it with provided command:

``` Shell
git clone git@github.com:MatusMakay/BlueTeam.Booster.Prod.git
```

### Configuring Syslog-ng
Navigate to directory `BlueTeam.Booster.Prod/syslog-ng`. First you have to configure file `filter-cisco-devices.conf` to match you specific network parameters.

``` shell
BlueTeam.Booster.Prod/syslog-ng
vi filter-cisco-devices.conf

source s_dev { 
    tcp(
        ip(IPAddressOfInterfaceOnWhichSyslogNgServiceWillListen)
	    port(514)
        flags(no-parse,store-raw-message,no-hostname)
    );
};

destination d_filebeat {
   tcp("IpAddressWhereFileBeatIsRunning" port(PortOnWhichFileBeatIsListenning));
};

```


After you configure your network parameters you can run `init.sh` script with sudo privileges:
``` shell
sudo chmod +x init.sh
sudo ./init.sh
```

### Create environment variables for REST API Server
After that create `.env` variables in directory  `BlueTeam.Booster.Prod/src/Bc.CyberSec.Detection.Booster.Api`. Example environment variable file can look like this:

``` 
API_ACCESS_KEY=ApiAccessKeyUseToAuthorizationOfApiCalls
SYSLOG_NG_CONFIG_DIR=/etc/syslog-ng/conf.d/uc-filters
SYSLOG_NG_CONFIG_FILE=uc-filters-definition.conf
KIBANA_URL=https://DomainWhereKibanaIsHosted/api/alerting/rule
KIBANA_API_ACCESS_KEY=ApiAccessKeyGeneratedInKibana
ENVIRONMENT=production
MONGO_CONNECTION_STRING=mongodb://username:Password@mongodb
CISCO_DEVICE_IPS=SubnetForCiscoDevicesInForm=192.168.0.0/MASK
FORTIGATE_IP=SubnetWhereFortigateIsLocatedInForm=192.168.0.0/MASK
MONGO_USERNAME=username
MONGO_PASSWORD=Password
```

After you create `.env` you can run docker containers. Docker compose file is located in the same directory as should be environment variables. Navigate to that directory after that run command:

``` shell
docker compose up -d --build
```

Also you should ensure that your firewall allows sendings logs from devices to our application. Also you should have allowed that our application.



## Initialization of the REST API Server Application
First, ensure that the software packages inotifywait and syslog-ng are installed on the host server. After that, clone the source code from GitHub using the following command:

```shell
git clone git@github.com:MatusMakay/BlueTeam.Booster.Prod.git
```
### Configuring Syslog-ng
Navigate to the BlueTeam.Booster.Prod/syslog-ng/conf directory. You need to configure the filter-cisco-devices.conf file to match your specific network parameters:

``` shell
cd BlueTeam.Booster.Prod/syslog-ng/conf
vi filter-cisco-devices.conf

source s_dev { 
    tcp(
        ip(IPAddressOfInterfaceOnWhichSyslogNgServiceWillListen)
        port(514)
        flags(no-parse,store-raw-message,no-hostname)
    );
};

destination d_filebeat {
   tcp("IpAddressWhereFileBeatIsRunning" port(PortOnWhichFileBeatIsListening));
};
```

After configuring your network parameters, you can run the init.sh script with sudo privileges:

``` shell
sudo chmod +x init.sh
sudo ./init.sh
```
### Creating Environment Variables for the REST API Server

Next, create a .env file in the BlueTeam.Booster.Prod/src/Bc.CyberSec.Detection.Booster.Api directory. An example .env file might look like this:

``` plaintext
API_ACCESS_KEY=ApiAccessKeyUsedForAuthorizationOfApiCalls
SYSLOG_NG_CONFIG_DIR=/etc/syslog-ng/conf.d/uc-filters
SYSLOG_NG_CONFIG_FILE=uc-filters-definition.conf
KIBANA_URL=https://DomainWhereKibanaIsHosted/api/alerting/rule
KIBANA_API_ACCESS_KEY=ApiAccessKeyGeneratedInKibana
ENVIRONMENT=production
MONGO_CONNECTION_STRING=mongodb://username:Password@mongodb
CISCO_DEVICE_IPS=SubnetForCiscoDevicesInFormat=192.168.0.0/MASK
FORTIGATE_IP=SubnetWhereFortigateIsLocatedInFormat=192.168.0.0/MASK
MONGO_USERNAME=username
MONGO_PASSWORD=Password
``` 

After creating the .env file, you can run application with `init.sh` script which is located in `BlueTeam.Booster.Prod`
``` shell
sudo ./init.sh
```
Ensure that your firewall allows logs to be sent from devices to our application. You should also ensure that our application is allowed to communicate as needed.