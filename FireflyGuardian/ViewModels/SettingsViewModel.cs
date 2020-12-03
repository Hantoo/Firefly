using Caliburn.Micro;
using FireflyGuardian.ServerResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.ViewModels
{
    class SettingsViewModel : Screen
    {
        public string FTP_IP { get; set; }
        public string FTP_User { get; set; }
        public string FTP_Pass { get; set; }
        public string NodeDiagramLayoutView { get; set; }
        public double VenueDiagramScaleMultipler { get; set; }
        public int NodeGlobalBrightness { get { return ServerManagement.settings.globalbrightness; } set { ServerManagement.settings.globalbrightness = value; } }

        #region statusBanner
        public bool showStatus { get; set; }
        public string statusMessage { get; set; }
        public FireflyGuardian.ViewModels.statusState statusStateIndicator { get; set; }
        public void changeStatusBackGround(statusState state)
        {
            statusStateIndicator = state;
            NotifyOfPropertyChange(() => statusStateIndicator);

        }

        public async void ShowStatus(string message, int timeShown, statusState state)
        {
            changeStatusBackGround(state);
            await PutTaskDelay(message, timeShown);
        }

        async Task PutTaskDelay(string msg, int timeout)
        {
            showStatus = true;
            NotifyOfPropertyChange(() => showStatus);
            statusMessage = msg;
            NotifyOfPropertyChange(() => statusMessage);
            await Task.Delay(timeout);
            showStatus = false;
            NotifyOfPropertyChange(() => showStatus);
            await Task.Delay(500);
            statusMessage = "";
            NotifyOfPropertyChange(() => statusMessage);
        }

        #endregion

        public void updateNodeGlobalBrightness()
        {
            byte[] msg = { 0xFF, 0x03, 0x13, (byte)NodeGlobalBrightness };
            for (int i = 0; i < ServerManagement.devices.Count; i++) {
                ServerManagement.udpServer.UDPSend(msg,ServerManagement.devices[i].deviceIP);
            }
        }

        public SettingsViewModel()
        {
            if (ServerResources.ServerManagement.settings != null)
            {
                FTP_IP = ServerResources.ServerManagement.settings.ftpURL;
                FTP_User = ServerResources.ServerManagement.settings.ftpUsername;
                FTP_Pass = ServerResources.ServerManagement.settings.ftpPassword;
                NodeDiagramLayoutView = ServerManagement.settings.locationOfVenueDiagram;
        VenueDiagramScaleMultipler = ServerResources.ServerManagement.settings.VenueDiagramScaleMultipler;
                NotifyOfPropertyChange(() => FTP_IP);
                NotifyOfPropertyChange(() => FTP_User);
                NotifyOfPropertyChange(() => FTP_Pass);
                NotifyOfPropertyChange(() => VenueDiagramScaleMultipler);
                NotifyOfPropertyChange(() => NodeDiagramLayoutView);
            }
        }

        public void chooseNewNodeDiagramFile()
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = "PNG Files | *.png|JPEG Files | *.jpg";
            fileDialog.ShowDialog();
            if(fileDialog.FileName != null)
            {
                try
                {
                    NodeDiagramLayoutView = fileDialog.FileName;
                    File.Copy(fileDialog.FileName, ServerResources.ServerManagement.settings.absoluteLocationOfAppData +"/temp/VenueMap" + fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.')), true);
                    NodeDiagramLayoutView = ServerResources.ServerManagement.settings.absoluteLocationOfAppData + "/temp/VenueMap" + fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.'));
                    ServerManagement.settings.locationOfVenueDiagram = ServerResources.ServerManagement.settings.absoluteLocationOfAppData + "/temp/VenueMap" + fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.')); 
  
                    NotifyOfPropertyChange(() => NodeDiagramLayoutView);
                }
                catch (Exception e)
                {
                    ShowStatus("File Already Exists Within App Data Folder - Please Delete And Try Re-Uploading", 5000, statusState.Error);
                }
            }
            

        }

        public void SaveFTP()
        {
            ServerResources.ServerManagement.settings.ftpURL = FTP_IP;
            ServerResources.ServerManagement.settings.ftpUsername = FTP_User;
            ServerResources.ServerManagement.settings.ftpPassword = FTP_Pass;
            ServerResources.ServerManagement.settings.locationOfVenueDiagram = NodeDiagramLayoutView;
            ServerResources.ServerManagement.settings.VenueDiagramScaleMultipler = VenueDiagramScaleMultipler;
            ShowStatus("Settings Saved", 2000, statusState.Good);
        }
    }
}
