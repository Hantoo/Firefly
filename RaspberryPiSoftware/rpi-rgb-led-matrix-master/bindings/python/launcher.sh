#!/bin/sh
# launcher.sh
# navigate to home directory, then to this directory, then execute python script, then back home

cd /
cd /home/pi/Desktop/rpi-rgb-led-matrix-master/bindings/python
sudo python3 Main.py
cd /
