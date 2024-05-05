#!/bin/bash

FILE_TO_MONITOR="/etc/syslog-ng/conf.d/uc-filters/uc-filters-definition.conf"

SERVICE_NAME="syslog-ng"

restart_service() {
    systemctl restart "$SERVICE_NAME"
    echo "Restarted $SERVICE_NAME service."
}

inotifywait -m -e modify "$FILE_TO_MONITOR" |
    while read -r directory events filename; do
        echo "Change detected in $FILE_TO_MONITOR"
        restart_service
    done