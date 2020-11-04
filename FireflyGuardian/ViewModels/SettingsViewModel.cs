using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.ViewModels
{
    class SettingsViewModel : Screen
    {
        public string FTP_IP { get; set; }
        public string FTP_User { get; set; }
        public string FTP_Pass { get; set; }

        

        public SettingsViewModel()
        {
            if (ServerResources.ServerManagement.settings != null)
            {
                FTP_IP = ServerResources.ServerManagement.settings.ftpURL;
                FTP_User = ServerResources.ServerManagement.settings.ftpUsername;
                FTP_Pass = ServerResources.ServerManagement.settings.ftpPassword;
                NotifyOfPropertyChange(() => FTP_IP);
                NotifyOfPropertyChange(() => FTP_User);
                NotifyOfPropertyChange(() => FTP_Pass);
            }
        }

        public void SaveFTP()
        {
            ServerResources.ServerManagement.settings.ftpURL = FTP_IP;
            ServerResources.ServerManagement.settings.ftpUsername = FTP_User;
            ServerResources.ServerManagement.settings.ftpPassword = FTP_Pass;
        }
    }
}
