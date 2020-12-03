using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.ViewModels
{
    class DashboardViewModel
    {
        public DashboardViewModel()
        {

        }

        public void LoadFromPreviousFile()
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
                //string json = File.ReadAllText(openShowFile.FileName);
                //List<DeviceModel> deviceListFromFile = new List<DeviceModel>();
                //deviceListFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceModel>>(json);
                //ServerResources.ServerManagement.devices = deviceListFromFile;

                //updateDeviceListWindow();

            }
        }

    }
}
