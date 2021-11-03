from rgbmatrix import RGBMatrix, RGBMatrixOptions
from PIL import Image

def test():
    print("Hello")

def imageviewer(image_file):

    #Get the image number that is passed through to this function as a parameter and decode it to be a number. Then load that file
    #from the path below.
    #!/usr/bin/env python
    image = Image.open("/home/pi/Desktop/rpi-rgb-led-matrix/bindings/python/samples/MediaPool/"+  image_file.decode("utf-8") +".png")

    #Create a new RGB matrix with any options that have been updated
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
