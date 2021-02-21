using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireflyGuardian.ServerResources;
using Caliburn.Micro;

namespace FireflyGuardian.ViewModels
{
    class DashboardViewModel : Screen
    {

        public DashboardViewModel()
        {
            ServerResources.LocalServer.tickComplete -= Tickupdatee;
            ServerResources.LocalServer.tickComplete += Tickupdatee;
        }

        public int globalUpdateTimeRef { get; set; }
        public int globalUpdateTimeRefReversed { get; set; }
        public int heartUpdateTimeRef { get; set; }
        public int heartUpdateTimeRefReverse { get; set; }
        public string TimeString { get; set; }
        public string DateString { get; set; }
        public string welcomeMessage { get; set; }
        public int devicesOnNetwork { get; set; }
        public string devicesOnNetworkMessage { get; set; }
        public string SystemText { get; set; }
        public string iconColour { get; set; }
        public string icon { get; set; }
        public string subText { get; set; }
        public void Tickupdatee()
        {
            Console.WriteLine("Tick" + DateTime.Now.Second);
            updateTimers();
           
        }

        public void updateTimers()
        {
            globalUpdateTimeRef = 60-ServerManagement.nextGlobalUpdate_reference;
            globalUpdateTimeRefReversed = ServerManagement.nextGlobalUpdate_reference;
            heartUpdateTimeRef = 10-ServerManagement.nextHeartBeatCheck_reference;
            heartUpdateTimeRefReverse = ServerManagement.nextHeartBeatCheck_reference;
            TimeString = DateTime.Now.Hour + ":"+ DateTime.Now.Minute + ":" + DateTime.Now.Second;
            DateString = DateTime.Now.DayOfWeek.ToString();
            welcomeMessage = "Welcome To FireFly Control Software. It is now " + TimeString + " on " + DateString + " the "+ DateTime.Now.Day.ToString();
            devicesOnNetwork = ServerManagement.devices.Count;
            devicesOnNetworkMessage = "We Found " + devicesOnNetwork + " FireFly devices connected to this network. If you are expecting more then go to the Device Management tab and click the poll devices button. If the units have just turned on, please give them 1 minute to fully boot up.";

            if (ServerManagement.shouldEvacuate)
            {
                //True
                SystemText = "SYSTEM IN EVACUATION MODE";
                iconColour = "#E43838";
                icon = "\uF13C";
                subText = "The evacuation process has been activated. All Global updates will halt. If the evacuation is over, go to Device Management and click the Evacuation button to turn it off (Grey State).";
            }
            else
            {
                //false
                SystemText = "A L L   S Y S T E M   N O M I N A L";
                iconColour = "#42DC07";
                icon = "\uF13E";
                subText = "All functions are running as expected.";


            }
            NotifyOfPropertyChange(() => heartUpdateTimeRef);
            NotifyOfPropertyChange(() => heartUpdateTimeRefReverse);
            NotifyOfPropertyChange(() => globalUpdateTimeRefReversed);
            NotifyOfPropertyChange(() => globalUpdateTimeRef);
            NotifyOfPropertyChange(() => welcomeMessage);
            NotifyOfPropertyChange(() => devicesOnNetwork);
            NotifyOfPropertyChange(() => SystemText);
            NotifyOfPropertyChange(() => iconColour);
            NotifyOfPropertyChange(() => icon);
            NotifyOfPropertyChange(() => subText);

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
