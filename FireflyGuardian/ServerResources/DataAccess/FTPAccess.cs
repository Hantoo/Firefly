
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
                    Console.WriteLine("File Uploaded.");
                    MessageBox.Show("File Uploaded");
                }
            }
        }


    }
}
