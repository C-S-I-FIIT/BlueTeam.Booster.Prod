#!/bin/bash

nohup /home/mmakay/blueteam-boobster/BlueTeam.Booster.Prod/syslog-ng/src/monitor-script.sh > /dev/null 2>&1 &

cd /home/mmakay/blueteam-boobster/BlueTeam.Booster.Prod/src/Bc.CyberSec.Detection.Booster.Api
docker compose up -d --build

