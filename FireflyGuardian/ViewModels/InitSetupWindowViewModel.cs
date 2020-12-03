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
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using FireflyGuardian.ServerResources;
using System.ComponentModel;
using FireflyGuardian.Views;

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

        public void LoadFromFile()
        {
             OpenFileDialog openShowFile = new OpenFileDialog();
            openShowFile.Filter = "FireFly Project File |*.fly| Zip Files | *.zip";
            bool hasFile;
            hasFile = (bool)openShowFile.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (hasFile == true)
            {
                hasFile = false;
                ServerResources.DataAccess.json.unZipProjectFile(openShowFile.FileName);
                _shell.generatePages();
                _shell.ExitSetupView();
            }
        }

        public void submitSettings()
        {
            if (FireflyGuardian.ServerResources.ServerManagement.settings == null)
            {
                SettingsModel settings = new SettingsModel();
                settings.ftpPassword = ftp_Password;
                settings.ftpURL = ftp_IP;
                settings.ftpUsername = ftp_User;
                FireflyGuardian.ServerResources.ServerManagement.settings = settings;
            }
            else
            {
                FireflyGuardian.ServerResources.ServerManagement.settings.ftpPassword = ftp_Password;
                FireflyGuardian.ServerResources.ServerManagement.settings.ftpURL = ftp_IP;
                FireflyGuardian.ServerResources.ServerManagement.settings.ftpUsername = ftp_User;
            }
            
            if (testSettings())
            {
                string JSONresult = JsonConvert.SerializeObject(FireflyGuardian.ServerResources.ServerManagement.settings);
                _Init.generateSettings(JSONresult);
                ServerResources.DataAccess.Init.createBaseLocalisedMediaPool();
                
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
