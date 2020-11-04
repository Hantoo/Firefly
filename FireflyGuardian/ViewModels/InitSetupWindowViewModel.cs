using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FireflyGuardian.Models;
using FireflyGuardian.ServerResources.DataAccess;

namespace FireflyGuardian.ViewModels
{
    class InitSetupWindowViewModel : Screen
    {

        public string ftp_IP { get; set; }
        public string ftp_User { get; set; }
        public string ftp_Password { get; set; }
        public string errorMessage { get; set; }
        private FireflyGuardian.ServerResources.DataAccess.Init _Init;
        private ShellViewModel _shell;
        public InitSetupWindowViewModel(FireflyGuardian.ServerResources.DataAccess.Init init, ShellViewModel shell)
        {
            _Init = init;
            _shell = shell;
        }

        public void submitSettings()
        {

            SettingsModel settings = new SettingsModel();
            settings.ftpPassword = ftp_Password;
            settings.ftpURL = ftp_IP;
            settings.ftpUsername = ftp_User;
            FireflyGuardian.ServerResources.ServerManagement.settings = settings;
            
            if (testSettings())
            {
                string JSONresult = JsonConvert.SerializeObject(settings);
                _Init.generateSettings(JSONresult);
                _shell.ExitSetupView();
            }
            else
            {
                errorMessage = "["+DateTime.Now.ToString()+"] Connection Can Not Be Established. Please Verify Details / Connection";
                NotifyOfPropertyChange(() => errorMessage);
            }
        }

        public bool testSettings()
        {
            return FTPAccess.VerifyConnection(FireflyGuardian.ServerResources.ServerManagement.settings.ftpURL, FireflyGuardian.ServerResources.ServerManagement.settings.ftpUsername, FireflyGuardian.ServerResources.ServerManagement.settings.ftpPassword);
        }
    }
}
