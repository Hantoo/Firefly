using FireflyGuardian.Models;
using FireflyGuardian.ServerResources.DataAccess;
using FireflyGuardian.ServerResources.UDP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        public static List<MediaSlotModel> mediaSlots;
        public static List<DeviceModel> devices = new List<DeviceModel>();
        public static List<RoutineModel> routines = new List<RoutineModel>();
        private static LocalServer localServer;
        public static int nextHeartBeatCheck_reference;
        public static int nextGlobalUpdate_reference;
        //Global variable to indicate any changes to device array which compromises the intergrity of path algorithm, etc.
        public static bool deviceStructureValid = true;
        public static bool shouldEvacuate = false;
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
            updateServermangementMediapool();
            localServer = new LocalServer();
            localServer.start();
            /*serverVariables.FTPIP = "192.168.0.11"; // Don't Hard Code
            serverVariables.FTPPort = "21"; // Don't Hard Code //ToDo: Don't HARD CODE THE VALUES HAHA 
            serverVariables.FTPUser = "FTP-User"; // Don't Hard Code
            serverVariables.FTPPassword = "root"; // Don't Hard Code*/
            //RefreshDeviceList(); 
            //
        }

        public static void ShouldEvacuate()
        {
            if (!shouldEvacuate)
            {
                for (int i = 0; i < ServerManagement.routines.Count; i++)
                {
                    ServerManagement.routines[i].isRunning = false;
                    ServerManagement.routines[i].routineThread.Abort();
                    Console.WriteLine("[SERVER] Setting Routine " + i + " isRunning To False");
                }
                shouldEvacuate = true;
            }
            else
            {
                shouldEvacuate = false;
            }
            
        }

        public static void stopAll()
        {
            ServerResources.UDP.UDPServer.shutdownUDP();
            string JSONresult = JsonConvert.SerializeObject(ServerManagement.settings);
            ServerResources.DataAccess.Init.updateJson(JSONresult);
            ServerResources.DataAccess.Init.saveRoutineJson();
            DataAccess.json.saveDevices();
            ViewModels.DeviceNetworkViewModel.GlobalDestroyNodeCanvas();
            WithRetry(() => Directory.Move(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool", ServerManagement.settings.absoluteLocationOfAppData + "/temp/LocalisedMediaPool"));
            

            ZipFile.CreateFromDirectory(ServerManagement.settings.absoluteLocationOfAppData+"/temp", ServerManagement.settings.absoluteLocationOfAppData +"/"+DateTime.Now.Year.ToString()+DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()+"_"+DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second+".fly");
            
            for (int i = 0; i < ServerManagement.routines.Count; i++)
            {
                ServerManagement.routines[i].isRunning = false;
                ServerManagement.routines[i].routineThread.Abort();
                Console.WriteLine("[SERVER] Setting Routine " + i + " isRunning To False");
            }
            localServer.stop();
            Directory.Delete(ServerManagement.settings.absoluteLocationOfAppData + "/temp", true);
            Environment.Exit(0);
        }

        private static void WithRetry(Action action, int timeoutMs = 1000)
        {
            var time = Stopwatch.StartNew();
            while (time.ElapsedMilliseconds < timeoutMs)
            {
                try
                {
                    action();
                    return;
                }
                catch (IOException e)
                {
                    // access error
                    if (e.HResult != -2147024864)
                        throw;
                }
            }
            throw new Exception("Failed perform action within allotted time.");
        }

        public static void addDevice(DeviceModel device)
        {
            devices.Add(device);
        }

        public static void clearAllDevice()
        {
            devices.Clear();
        }

        public static void deleteDeviceFromSystem(DeviceModel model)
        {
            //Look at and remove device from list
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].deviceID == model.deviceID)
                {
                    devices.RemoveAt(i);
                    break;
                }
            }
           
            for (int i = 0; i < devices.Count; i++)
            {
                //Remove removed device from connections
                for (int j = 0; j < devices[i].deviceConnectionOutputIds.Count; j++)
                {
                    if(devices[i].deviceConnectionOutputIds[j] == model.deviceID)
                    {
                        devices[i].deviceConnectionOutputIds.RemoveAt(j);
                    }
                }
            }

            //Remove removed device from routines
            for (int i = 0; i < routines.Count; i++)
            {
                
                
                for (int j = 0; j < routines[i].deviceIDsToRun.Count; j++)
                {
                    if (routines[i].deviceIDsToRun[j] == model.deviceID)
                    {
                        routines[i].deviceIDsToRun.RemoveAt(j);
                    }
                }
            }


        }

        public static bool checkForDuplicateDeviceIDs(int deviceIDToMatch)
        {
            for(int i =0; i < devices.Count; i++)
            {
                if(devices[i].deviceID == deviceIDToMatch)
                {
                    return true;
                }
            }
            return false;
        }

        public static void updateServermangementMediapool()
        {
            mediaSlots = new List<MediaSlotModel>();

            for (int i = 0; i < 255; i++)
            {
                MediaSlotModel slot = new MediaSlotModel();
                slot.slotID = i;
                if (File.Exists(ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + i + ".png"))
                {
                    if (i < 23)
                    {
                        slot.image_symbol = "\uE72E";
                    }
                    if (ServerManagement.settings.slotNames[i] != null)
                    {
                        slot.image_name = ServerManagement.settings.slotNames[i];
                    }
                    else
                    {

                        slot.image_name = ServerManagement.settings.slotNames[i];
                    }
                    slot.image = utils.BitmapToImageSource(ServerManagement.settings.absoluteLocationOfLocalisedMedia + "/" + i + ".png");
                    //slot.image_source = ;
                }
                else
                {
                    ServerManagement.settings.slotNames[i] = "";
                    slot.image_name = "No Media";
                }
                mediaSlots.Add(slot);
            }
            ServerManagement.mediaSlots = mediaSlots;

        }

        
    }
}
