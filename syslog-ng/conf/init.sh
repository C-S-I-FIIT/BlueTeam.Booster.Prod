#!/bin/bash
$CUR_DIR = $(pwd)
cd /etc/syslog-ng
rm syslog-ng.conf 
cp $CUR_DIR/syslog-ng.conf .
cp $CUR_DIR/filter-cisco-devices.conf conf.d
mkdir conf.d/uc-filters
touch conf.d/uc-filters/uc-filters.conf