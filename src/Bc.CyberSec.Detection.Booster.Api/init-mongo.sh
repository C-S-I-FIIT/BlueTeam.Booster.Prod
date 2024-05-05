#!/bin/bash
set -e
KEYFILE="/etc/mongo/mongodb-keyfile"

if [ ! -f "$KEYFILE" ]; then
    mkdir /etc/mongo
    openssl rand -base64 756 > "$KEYFILE"
    chmod 400 "$KEYFILE"
    chown mongodb:mongodb "$KEYFILE"
fi

echo "Waiting for MongoDB to start..."
until mongosh --eval "db.adminCommand({ ping: 1 })" &>/dev/null; do
    echo "Waiting for MongoDB to be ready..."
    sleep 1
done
echo "MongoDB is up and running."

IS_INITIATED=$(mongosh --quiet --eval "db.isMaster().ismaster")
if [ "$IS_INITIATED" != "true" ]; then
    echo "Initiating replica set..."
    mongosh --eval "rs.initiate()"
else
    echo "Replica set already initiated."
fi
