
import time
import sys
import commands
import socket

from rgbmatrix import RGBMatrix, RGBMatrixOptions
from PIL import Image

def imageviewer(image_file):
    #!/usr/bin/env python
    image = Image.open("/home/pi/Desktop/rpi-rgb-led-matrix/bindings/python/samples/MediaPool/"+  image_file.decode("utf-8") +".png")

    # Configuration for the matrix
    options = RGBMatrixOptions()
    options.rows = 64
    options.cols = 64
    options.chain_length = 1
    options.parallel = 1


    options.hardware_mapping = 'adafruit-hat-pwm'  # If you have an Adafruit HAT: 'adafruit-hat'

    matrix = RGBMatrix(options = options)

    # Make image fit our screen.
    image.thumbnail((matrix.width, matrix.height), Image.ANTIALIAS)
    while True:
        matrix.SetImage(image.convert('RGB'))


sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
print(commands.getoutput('hostname -I'))
server_address = '192.168.0.32'
server_port = 42891

server = (server_address, server_port)
sock.bind(server)
print("Listening on " + server_address + ":" + str(server_port))

while True:
    payload, client_address = sock.recvfrom(4096)
    print(client_address)
    print(payload)
    if len(payload)>0:
        imageviewer(payload)
    sent = sock.sendto(payload, client_address)


def init_server:



