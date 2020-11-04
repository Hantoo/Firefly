
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FireflyGuardian.ServerResources.DataAccess
{
    public class ftpThreadInfomation
    {
        public string username { get; set; }
        public string password { get; set; }
        public string serverAddress { get; set; }
        public string fileNameWithExtenstion { get; set; }
        public string locationOfFileOnPCWithExtenstion { get; set; }

    }
    public static class FTPAccess
    {
        private static Thread fileUploadToFTPThread;

        public static void UploadFileToFTP(string locationOfFileOnPCWithExtenstion, string serverAddress, string fileNameWithExtenstion, string username, string pass)
        {
            fileUploadToFTPThread = new Thread(ThreadedUploadToFTP);
            ftpThreadInfomation threadInfomation = new ftpThreadInfomation();
            threadInfomation.username = username;
            threadInfomation.password = pass;
            threadInfomation.locationOfFileOnPCWithExtenstion = locationOfFileOnPCWithExtenstion;
            threadInfomation.serverAddress = serverAddress;
            threadInfomation.fileNameWithExtenstion = fileNameWithExtenstion;
            fileUploadToFTPThread.Start(threadInfomation);
           
        }


        public static bool VerifyConnection(string serverAddress, string username, string pass)
        {
            try
            {
                Console.WriteLine("URI: " + serverAddress);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://"+serverAddress+"/");
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username, pass);
                request.GetResponse();
            }
            catch (WebException ex)
            {
                return false;
            }
            return true;
        }

        public static bool VerifyFileInFTP(string serverAddress, string username, string pass, string filename)
        {
            
            var request = (FtpWebRequest)WebRequest.Create("ftp://" + serverAddress + filename);
            request.Credentials = new NetworkCredential(username, pass);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
            }
            return false;
            

        }

        public static void syncLocalisedMediaPoolToFTPServer()
        {
            DirectoryInfo mediapoolInfo = new DirectoryInfo(ServerManagement.settings.absoluteLocationOfLocalisedMedia);
            for(int i=0; i< mediapoolInfo.GetFiles().Length; i++)
            {
                UploadFileToFTP(ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + i + ".png", ServerManagement.settings.ftpURL, i+".png", ServerManagement.settings.ftpUsername, ServerManagement.settings.ftpPassword);
            }
            Console.WriteLine("Items In Pool: "+mediapoolInfo.GetFiles().Length);
          
        }

        private static void ThreadedUploadToFTP(object ftpThreadInfomation)
        {
            ftpThreadInfomation infomation = (ftpThreadInfomation)ftpThreadInfomation;
            FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create("ftp://" + infomation.serverAddress + "/" + infomation.fileNameWithExtenstion); 
            
            ftpReq.UseBinary = true;
            ftpReq.Method = WebRequestMethods.Ftp.UploadFile;
            ftpReq.Credentials = new NetworkCredential(infomation.username, infomation.password);

            byte[] b = File.ReadAllBytes(@"" + infomation.locationOfFileOnPCWithExtenstion);
            ftpReq.ContentLength = b.Length;
            using (Stream s = ftpReq.GetRequestStream())
            {
                s.Write(b, 0, b.Length);
            }

            FtpWebResponse ftpResp = (FtpWebResponse)ftpReq.GetResponse();

            if (ftpResp != null)
            {
                if (ftpResp.StatusDescription.StartsWith("226"))
                {
                    Console.WriteLine("File Uploaded: " + infomation.fileNameWithExtenstion);
                   
                }
            }
        }


    }
}
