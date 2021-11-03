## This file is used to look at each byte of the command that the device recieved and work
## out what needs to be done with it 

#from FireFly_Matrix import *
from ftpFunctions import *
from ImageDisplay import *
import json
#Return 0 - Error
#Return 1 - Completed

def sortRecievedCommands(byteArray, matrix):
	#Check that the first byte is formated as expected
	if(byteArray[0] != 255):
		print("Byte Array Not Formated Properly. Expected 'OxFF', Recieved: '"+byteArray[0]+"'")
		return 0;

	#Look at second Byte and determine what it is doing
	#Poll
	if(byteArray[1] == 1):
		print("Poll")
		return ReturnPollData()
	#HeartBeat
	elif(byteArray[1] == 2):
		print("HeartBeat")
		returnArray = bytearray(b'\xff\xa1\xa1')
		return returnArray
	#Force Set Data
	elif(byteArray[1] == 3):
		print("Force Set Data")
		#Check second byte for what data should do
		#Set Image
		if(byteArray[2] == 1 or byteArray[2] == 20):
			print("Set Image")
			with open('settings.json', 'r') as f:
				hardware_infomation = json.load(f)
			imageviewer(byteArray[3], matrix, int(hardware_infomation['Brightness']), int(hardware_infomation['Rotation']))
			return 1;
		#Set IP
		elif(byteArray[2] == 2):
			print("Set IP")
			return 1
		#Set Name
		elif(byteArray[2] == 17):
			print("Set Name")
			strbyteArray = []
			strName = ""
			for i in range(byteArray[3]):
				print("i: "+str(i))
				strbyteArray.append((byteArray[4+i]).to_bytes(1, byteorder='big'))
				strName = strName + (str(strbyteArray[i], 'UTF-8'))
			setName(strName)
			return 1
		#Set Device ID
		elif(byteArray[2] == 18):
			print("Set Device ID")
			iDValue = int(byteArray[3]) + int(byteArray[4]*255) 
			setId(iDValue)
		#Set Brightness
		elif(byteArray[2] == 19):
			print("Set Brightness")
			setBrightness(round((byteArray[3] / 255) * 100))
			return 1
		#Set IP and Port
		elif(byteArray[2] == 21):
			print("Set FTP IP")
			IP0 = byteArray[3]
			IP1 = byteArray[4]
			IP2 = byteArray[5]
			IP3 = byteArray[6] 
			ipStr = str(IP0) + "." + str(IP1) + "."+ str(IP2) + "." + str(IP3)
			setFTPIp(ipStr)
			return 1;
		#Set IP and Port
		elif(byteArray[2] == 22):
			print("Set FTP Username and Password")
			usernamebyteArray = []
			strUsername = ""
			passwordbyteArray = []
			strPassword = ""
			usernameLength = int(byteArray[3]) #Max 255 Chars
			for i in range(usernameLength):
				print("i: "+str(i))
				usernamebyteArray.append((byteArray[4+i]).to_bytes(1, byteorder='big'))
				strUsername = strUsername + (str(usernamebyteArray[i], 'UTF-8'))
			passwordLengthStart = 4+usernameLength
			passwordLength = byteArray[passwordLengthStart]
			for i in range(passwordLength):
				print("i: "+str(i))
				passwordbyteArray.append((byteArray[passwordLengthStart+1+i]).to_bytes(1, byteorder='big'))
				strPassword = strPassword + (str(passwordbyteArray[i], 'UTF-8'))
			setUsernameAndPassword(strUsername, strPassword)
			return 1
		elif(byteArray[2] == 23):
			print("Set Rotation")
			setRotation(byteArray[3])
			return 1;
		elif(byteArray[2] == 24):
			print("Set Emergency Image")
			setEmergencyImage(byteArray[3])
			return 1;
		else:
			print("Byte Array Not Formated Properly. Byte[2] Recieved: '"+byteArray[2]+"'")
			return 0;
	#Return Server IP And Port Number
	elif(byteArray[1] == 4):
		print("Return Server IP And Port Number")
		return 1
	#Download Images From FTP
	elif(byteArray[1] == 5):
		print("Download Images From FTP")
		returnArray = None
		try:
			FTPRetrieveImages();
			returnArray = bytearray(b'\xff\xa3\xa2')
		except Exception as e:
			print(e)
			returnArray = bytearray(b'\xff\xa3\xfe')
		return returnArray
	#Reserved
	elif(byteArray[1] == 6):
		print("Reserved")
		return 1
	#Reserved
	elif(byteArray[1] == 7):
		print("Reserved")
		return 1
	else:
		print("Byte Array Not Formated Properly. Byte[1] Recieved: '"+byteArray[1]+"'")
		return 0;
		

def ReturnPollData():
	print("Fetching Device Infomation Data")
	hardware_infomation = None
	with open('settings.json', 'r') as f:
	    hardware_infomation = json.load(f)
	print(hardware_infomation['Name'])
	hardware_infomation['Name']
	IdLow = hardware_infomation['ID'] - (255*(hardware_infomation['ID']//255))
	IdHigh = hardware_infomation['ID']//255;
	print("ID: "+ str(hardware_infomation['ID']) + ", Hi: " + str(IdHigh) + ", Low: " + str(IdLow) )
	ipArray = hardware_infomation['IP'].split('.')
	print(ipArray)
	IP0 = int(ipArray[0]);
	IP1 = int(ipArray[1]);
	IP2 = int(ipArray[2]);
	IP3 = int(ipArray[3]);
	NameLenLow = len(hardware_infomation['Name'])
	print("Name Len: "+str(NameLenLow))
	Name = split(str(hardware_infomation['Name']).encode())
	#4 = FF A0 LenLo LenHi, #2 = IDLow IDHigh, #4 = IP, #1 NameLen, ## Name
	fullLength = 4 + 2 + 4 + 1 + len(Name)
	lenHi = fullLength//255;
	lenLow = fullLength - (255*(fullLength//255));
	returnPollData = [255, 160, lenLow, lenHi, IdLow, IdHigh, IP0, IP1, IP2, IP3, NameLenLow]
	returnPollData = addArrayToEnd(returnPollData, Name)
	print(returnPollData)
	print("len: ",len(returnPollData))
	return bytearray(returnPollData)

def setUsernameAndPassword(user,passw):
	updateJson("FTPUser", user)
	updateJson("FTPPass", passw)

def setEmergencyImage(imageNum):
	updateJson("EmergencyImage", imageNum)

def setFTPIp(IPstr):
	updateJson("FTPIP", IPstr)

def setName(name):
	updateJson("Name", name)
	
def setId(idVal):
	updateJson("ID", int(idVal))

def setBrightness(val):
	updateJson("Brightness", val)

def setRotation(val):
	value = 0
	if(val == 0):
		value = 0
	if(val == 1):
		value = 90
	if(val == 2):
		value = 180
	if(val == 3):
		value = 270	
	updateJson("Rotation", value)

def split(word): 
    return [char for char in word] 
    
def addArrayToEnd(SourceArray, ArrayToAdd):
	amendedArray = SourceArray
	for char in ArrayToAdd:
		amendedArray.append(char)
	return amendedArray

def closeDisplayThread():
	closeImageDisplay()

def updateJson(feild, value):
	hardware_infomation = None
	
	with open('settings.json', 'r') as f:
		hardware_infomation = json.load(f)

		print("Updating "+feild+" from: "+ str(hardware_infomation[feild]) + " to: "+ str(value))
		hardware_infomation[feild] = value
	with open('settings.json', 'w') as f:
		json.dump(hardware_infomation, f)
		

		
