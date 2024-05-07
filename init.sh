#!/bin/bash

cd /src/Bc.CyberSec.Detection.Booster.Api
docker compose up -d --build 

cd /syslog-ng/src
./monitor-script.sh &

