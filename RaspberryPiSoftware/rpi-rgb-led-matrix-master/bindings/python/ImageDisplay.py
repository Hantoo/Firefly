
import time
import sys
import threading
from rgbmatrix import RGBMatrix, RGBMatrixOptions
from PIL import Image, ImageEnhance 
ThreadAlive = False;


exit_event = threading.Event()

def imageviewer(image_file, matrix, brightness, rotation):
    print("Brightness Setting: "+str(brightness))
    for thread in threading.enumerate():
        print(thread.getName() +",")
        if(thread.getName() == "DisplayImageThread"):
            
            while(thread.is_alive()):
                if(thread.is_alive()):
                    print("Deleteing thread")
                    exit_event.set()
                time.sleep(0.1)
                print("thread alive")
    exit_event.clear()
    
    
    print("Create New Thread")
    DisplayImage = True
    image = Image.open("Images/"+  str(image_file) +".png")
    
    # Configuration for the matrix
    matrix.brightness = brightness
    # Make image fit our screen.
    image.thumbnail((matrix.width, matrix.height), Image.ANTIALIAS) 
    displayThread = threading.Thread(target=ThreadDisplay, args=(1,image,matrix,brightness,rotation,))
    displayThread.setName("DisplayImageThread")
    displayThread.start()

    
        
def ThreadDisplay(name, image, matrix, brightnessLevel, rotation):
    
    ThreadAlive = True;
    
    DisplayImage = True
    image = image.rotate(rotation)
    matrix.SetImage(image.convert('RGB'),0,0, False)
    matrix.brightness = brightnessLevel
    while(DisplayImage):
        #matrix.brightness = brightnessLevel
        if exit_event.is_set():
            DisplayImage = False
            image = None
    print("Display Thread End")
    
def closeImageDisplay():
    exit_event.set()       
        

