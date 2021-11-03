## This is the main script that is run to trigger the main device functionality

from ServerCommandsSorting import *
import time
import sys
import subprocess
import socket
import json
import threading
import select
import ipaddress
import fcntl
import struct

global softReload
global sock
global matrix
global timeCountdown

heartBeattimeout = 16
timeCountdown = heartBeattimeout # 16Seconds wait for heartbeat
softReload = False
HeartBeat = True



def ThreadHeartTimer(name):
  	#This thread runs to count down from the timer set. 
  	#If the timer reaches 0 then the heart beat is set to false, indicating that
  	#the heartbeat has been lost. 
	global timeCountdown
	global HeartBeat
	while(timeCountdown != 0):
		print("Timer: "+str(timeCountdown))
		time.sleep(1)
		timeCountdown -= 1;
	#Countdown Reached # Seconds without a heartbeat
	print("Heartbeat Lost")
	
	HeartBeat = False;
	

def Main():
	#This is the function that is run to start the program off.
	global HeartBeat
	HeartBeat = True
	hardware_infomation = None

	with open('settings.json', 'r') as f:
		hardware_infomation = json.load(f)
	#hardware_infomation["Name"], hardware_infomation["IP"] ... "EmergencyImage", "Rotation", "Brightness", "DownloadImageLocation", "RunTime"
	
	#Network Setups
	#Create a socket for listening to UDP for listening to both UDP IP unicast and broadcast infomation
	global sock
	sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM, socket.IPPROTO_UDP) # UDP

	# Enable port reusage so we will be able to run multiple clients and servers on single (host, port). 
	# Do not use socket.SO_REUSEADDR except you using linux(kernel<3.9): goto https://stackoverflow.com/questions/14388706/how-do-so-reuseaddr-and-so-reuseport-differ for more information.
	# For linux hosts all sockets that want to share the same address and port combination must belong to processes that share the same effective user ID!
	# So, on linux(kernel>=3.9) you have to run multiple servers and clients under one user to share the same (host, port).
	# Thanks to @stevenreddie
	sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEPORT, 1)

	# Enable broadcasting mode
	sock.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
	
	sock.bind(("", 42891))
	
	print(sock.getsockname()[0]) # '192.168.0.110'

	#Sets a RGB matrix using all the correct paramters for use with the custom PCB we have created.
	options = RGBMatrixOptions()
	options.rows = 64
	options.cols = 64
	options.chain_length = 1
	options.parallel = 1
	options.hardware_mapping = 'adafruit-hat-pwm'  # If you have an Adafruit HAT: 'adafruit-hat'
	options.gpio_slowdown = 4
	options.panel_type = 'FM6126A'
	options.brightness = int(hardware_infomation["Brightness"]) 
	#options.daemon = 1 #Makes the entire script run in the background - final final step of the pi
	global matrix
	matrix = RGBMatrix(options = options)
	while True:
		#While loop runs indefinately for the program to constantly function and not close.
		#When a heartbeat has been detected within the given time, then the system will run normally.
		while HeartBeat:
			payload = None
			#Listen out for a UDP packet without blocking
			try:
				newLost = True
				payload, client_address = sock.recvfrom(4096)
				newLost = True
			except socket.error:
				newLost = True
				pass ## handle error
			if payload != None:			
				print("Command From "+client_address[0]+" On Port "+str(client_address[1]))
				print(payload)
				#Returned 0 - Error in completing task | Returned 1 - Completed Task Successfully.
				#if a packet has been detected then route the package and activate what needs to be run.
				result = sortRecievedCommands(payload, matrix) 
				#If a result has been returned and we need to send something back to the server,
				#then so it here
				print(result)
				if(type(result) == type(bytearray())):
					print("Sending To Client: "+str(result)) 
					sent = sock.sendto(result, client_address)
					if(result == b'\xff\xa1\xa1'):
						#Heartbeat Response Detected. Reset Counter
						global timeCountdown
						timeCountdown = heartBeattimeout
				print("------- Command Complete -------")
		#Show Assigned Emergency Image If Heartbeat lost
		if newLost:
			#Set a byte array for the correct byte values to display the emerancy image
			ImageCommandByteArray = [255,3,1,int(hardware_infomation["EmergencyImage"])]
			sortRecievedCommands(ImageCommandByteArray, matrix) 
			newLost = False;
		payload = None
		# Listen out for a heartbeat, and if a start beat has been detected then return to the code above,
		#and start the heart beat monitor / timer again
		try:
			payload, client_address = sock.recvfrom(4096)
		except socket.error:
			pass ## handle error
		if payload != None:
			if(payload[0] == 255 and payload[1] == 2):
				timeCountdown = heartBeattimeout
				print("Recieved Heart Beat - Returning To Full Functionality")
				timerHeartbeatThread = threading.Thread(target=ThreadHeartTimer, args=(1,))
				timerHeartbeatThread.setName("HeartbeatTimerThread")
				timerHeartbeatThread.start()
				HeartBeat = True
			
		

	


while True:
	timerHeartbeatThread = threading.Thread(target=ThreadHeartTimer, args=(1,))
	timerHeartbeatThread.setName("HeartbeatTimerThread")
	timerHeartbeatThread.start()
	Main()
