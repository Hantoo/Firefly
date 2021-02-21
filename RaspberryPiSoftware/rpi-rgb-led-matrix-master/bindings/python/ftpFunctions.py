import ftplib
import json
import os

#Download files from FTP server
def FTPRetrieveImages():
	hardware_infomation = None;
	with open('settings.json', 'r') as f:
		hardware_infomation = json.load(f)
  		#hardware_infomation["FTPIP"], hardware_infomation["FTPPort"], hardware_infomation["FTPUser"], hardware_infomation["FTPPass"]
  	# connect to the FTP server
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

