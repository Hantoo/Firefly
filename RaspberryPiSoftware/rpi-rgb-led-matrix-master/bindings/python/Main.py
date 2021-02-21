
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
	global HeartBeat
	HeartBeat = True
	hardware_infomation = None
	with open('settings.json', 'r') as f:
		hardware_infomation = json.load(f)
	#hardware_infomation["Name"], hardware_infomation["IP"] ... "EmergencyImage", "Rotation", "Brightness", "DownloadImageLocation", "RunTime"
	
	#Network Setups
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
	
	#sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
	#sock.setblocking(False)
	#print("Local Ip Addresses:")
	#ipAddr =  (subprocess.getoutput('hostname -I')).split(' ')
	#print(ipAddr)
	#server_address = ipAddr[0]
	#server_port = 42891
	#server = (server_address, server_port)
	#sock.bind(server)
	
	import ipaddress

	#IP = '192.168.32.16'
	#MASK = '255.255.255.0'

	#host = ipaddress.IPv4Address(ipAddr[0])
	#net = ipaddress.IPv4Network(ipAddr[0] + '/' + MASK, False)
	#print('IP:', ipAddr[0])
	#print('Mask:', MASK)
	#print('Subnet:', ipaddress.IPv4Address(int(host) & int(net.netmask)))
	#print('Host:', ipaddress.IPv4Address(int(host) & int(net.hostmask)))
	#print('Broadcast:', net.broadcast_address)
	#broadcastIP = 'b\xFF\xFF\xFF\xFF'
	#strBroadcastaddr = str(net.broadcast_address)
	#sock.bind((strBroadcastaddr,12345))
	#print("Listening on " + server_address + ":" + str(server_port))
	



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
		
		while HeartBeat:
			
			#payload, client_address = sock.recvfrom(4096)
			#dataInSocket, _, _ = select.select([sock], [], [])
			payload = None
			try:
				newLost = True
				payload, client_address = sock.recvfrom(4096)
				newLost = True
			except socket.error:
				newLost = True
				pass ## handle error
			if payload != None:
				print("I HAZ DATA :D")
				#payload, client_address = sock.recvfrom(4096)
				
				print("Command From "+client_address[0]+" On Port "+str(client_address[1]))
				print(payload)
				#Returned 0 - Error in completing task | Returned 1 - Completed Task Successfully.
				result = sortRecievedCommands(payload, matrix) 
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
			ImageCommandByteArray = [255,3,1,int(hardware_infomation["EmergencyImage"])]
			sortRecievedCommands(ImageCommandByteArray, matrix) 
			newLost = False;
		payload = None
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
