#!/bin/bash

current_dir=$(pwd)

cd src/Bc.CyberSec.Detection.Booster.Api
docker compose up -d --build 

cd $current_dir/syslog-ng/src
./monitor-script.sh &