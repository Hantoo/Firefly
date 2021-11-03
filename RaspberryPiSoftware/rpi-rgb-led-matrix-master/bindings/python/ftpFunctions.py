# This script is used for downloading images from the FTP server
import ftplib
import json
import os

#Download files from FTP server
def FTPRetrieveImages():

	#This function runs when the devie is requested to download files from the FTP server

	hardware_infomation = None;
	#Load the settings.json file which contains the login and IP for the FTP server
	with open('settings.json', 'r') as f:
		hardware_infomation = json.load(f)
  	
  	# connect to the FTP server using the details the user has provided
	ftp = ftplib.FTP(hardware_infomation["FTPIP"], hardware_infomation["FTPUser"], hardware_infomation["FTPPass"])
	# force UTF-8 encoding
	ftp.encoding = "utf-8"
	filenames = ftp.nlst() # get filenames within the directory
	#Go through every file and download it to the file location as specified in the JSON file 
	for filename in filenames:
		local_filename = os.path.join(hardware_infomation["DownloadImageLocation"], filename)
		#with open(local_filename, 'wb') as fil:
		ftp.retrbinary('RETR '+ filename, open(local_filename, 'wb+').write)
	ftp.quit() 

