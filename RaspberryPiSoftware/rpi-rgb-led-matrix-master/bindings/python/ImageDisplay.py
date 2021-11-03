## This script relates to everything that is used for handling the images on the display

import time
import sys
import threading
from rgbmatrix import RGBMatrix, RGBMatrixOptions
from PIL import Image, ImageEnhance 
ThreadAlive = False;


exit_event = threading.Event()

#Called when a new image needs to be displayed
def imageviewer(image_file, matrix, brightness, rotation):

    print("Brightness Setting: "+str(brightness))
    #Go through all threads running and if there is a display thread already running then kill it and create a new one.
    #This happens to try and reduce the flicker that can occur on the screen due to the PI processing. This is one
    #of the limiations of the RGB matrix lib we are using.
    for thread in threading.enumerate():
        print(thread.getName() +",")
        if(thread.getName() == "DisplayImageThread"):
            
            while(thread.is_alive()):
                if(thread.is_alive()):
                    print("Deleteing thread")
                    #Sets the exit_event event to be true which will be triggered in any active display threads and
                    #will ask them to close correctly
                    exit_event.set()
                time.sleep(0.1)
                print("thread alive")
    #Resets the event so that it doesn't keep triggering
    exit_event.clear()
    
    
    print("Create New Thread")
    DisplayImage = True
    #Load the file that the user has requested from the images folder
    image = Image.open("Images/"+  str(image_file) +".png")
    
    # Configuration for the matrix
    matrix.brightness = brightness
    # Make image fit our screen.
    image.thumbnail((matrix.width, matrix.height), Image.ANTIALIAS) 
    #Creates a new display thread with all of the needed parameters
    displayThread = threading.Thread(target=ThreadDisplay, args=(1,image,matrix,brightness,rotation,))
    displayThread.setName("DisplayImageThread")
    displayThread.start()

    
        
def ThreadDisplay(name, image, matrix, brightnessLevel, rotation):
    
    ThreadAlive = True;
    
    DisplayImage = True
    #Adjust the images rotation if needed and converts it to RGB values to be used on
    #the matrix.
    image = image.rotate(rotation)
    matrix.SetImage(image.convert('RGB'),0,0, False)
    matrix.brightness = brightnessLevel
    #Runs a while loop to constantly display the image. This is in a different thread from the 
    #main program so it doesn't block any of the UDP listening. We also use a while loop to keep
    #this thread running, otherwise it would display an image for a second and then stop.
    while(DisplayImage):
        #If the exit_event is set then force the program to stop.
        if exit_event.is_set():
            DisplayImage = False
            image = None
    print("Display Thread End")
    
def closeImageDisplay():
    exit_event.set()       
        

