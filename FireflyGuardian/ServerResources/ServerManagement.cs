using FireflyGuardian.Models;
using FireflyGuardian.ServerResources.DataAccess;
using FireflyGuardian.ServerResources.UDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.ServerResources
{

    class ServerManagement
    {
        /*public struct serverProjectVariables{
            public string FTPIP;
            public string FTPPort;
            public string FTPUser;
            public string FTPPassword;
        }*/

        public static SettingsModel settings;
        public static UDPServer udpServer;
        public static List<DeviceModel> devices = new List<DeviceModel>();
        //Global variable to indicate any changes to device array which compromises the intergrity of path algorithm, etc.
        public static bool deviceStructureValid = true;
        //public static List<DeviceModel> prevdevices = new List<DeviceModel>();
        //public static serverProjectVariables serverVariables;
        public ServerManagement()
        {
            //serverVariables = new serverProjectVariables();
           
        }

        public static void Init()
        {
            //Settings Generated From ShellView / DataAccess.Init() - Reads from JSON file.
            //Create UDP Server
            udpServer = new UDPServer();
            //Poll Devices
            UDPPreformattedMessages.PollBoards();
            /*serverVariables.FTPIP = "192.168.0.11"; // Don't Hard Code
            serverVariables.FTPPort = "21"; // Don't Hard Code //ToDo: Don't HARD CODE THE VALUES HAHA 
            serverVariables.FTPUser = "FTP-User"; // Don't Hard Code
            serverVariables.FTPPassword = "root"; // Don't Hard Code*/
            //RefreshDeviceList(); 
            //
        }

        public static void saveAllJsonFiles()
        {

        }

       /* private static async Task RefreshDeviceList()
        {
            // In the Real World, we would actually do something...
            // For this example, we're just going to (asynchronously) wait 100ms.
            await Task.Delay(40000);
            

        }*/

        public static void addDevice(DeviceModel device)
        {
            devices.Add(device);

        }
    }
}
