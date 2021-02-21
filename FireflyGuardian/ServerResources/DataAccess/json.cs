using FireflyGuardian.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.ServerResources.DataAccess
{
    public static class json
    {

        private static string appdataFolder;
        private static void checkIfAppDataFolderExists()
        {
            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            appdataFolder = Path.Combine(folder, "FireFly");

            // CreateDirectory will check if folder exists and, if not, create it.
            // If folder exists then CreateDirectory will do nothing.
            Directory.CreateDirectory(appdataFolder);
        }

        public static void unZipProjectFile(string fileName)
        {
            Directory.CreateDirectory(ServerManagement.settings.absoluteLocationOfAppData + "/temp");
            if (File.Exists(fileName))
            {
                //Read the .fly folder and unzip it
                ZipArchive zip = ZipFile.OpenRead(fileName);
                //ToDo: fails when loading project from dashboard, settings file already exists.
                ZipFile.ExtractToDirectory(fileName, ServerManagement.settings.absoluteLocationOfAppData + "/temp");

                //Load from Settings file
                string jsonSettings = File.ReadAllText(ServerManagement.settings.absoluteLocationOfAppData + "/temp/Settings.json");
                SettingsModel settingsListFromFile = new SettingsModel();
                settingsListFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsModel>(jsonSettings);
                ServerResources.ServerManagement.settings = settingsListFromFile;

                //Load from Devices file
                string json = File.ReadAllText(ServerManagement.settings.absoluteLocationOfAppData + "/temp/Devices.json");
                List<DeviceModel> deviceListFromFile = new List<DeviceModel>();
                deviceListFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceModel>>(json);
                ServerResources.ServerManagement.devices = deviceListFromFile;

                //Load from Routine file
                string jsonRoutine = File.ReadAllText(ServerManagement.settings.absoluteLocationOfAppData + "/temp/Routines.json");
                List<RoutineModel> routineListFromFile = new List<RoutineModel>();
                routineListFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RoutineModel>>(jsonRoutine);
                ServerResources.ServerManagement.routines = routineListFromFile;

                if (Directory.Exists(ServerManagement.settings.absoluteLocationOfAppData + "/temp/LocalisedMediaPool"))
                {
                    if (Directory.Exists(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool"))
                    {

                        WithRetry(() => Directory.Delete(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool", true));
                        
                    }
                    Directory.Move(ServerManagement.settings.absoluteLocationOfAppData + "/temp/LocalisedMediaPool", ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool");
                }
            }
            else
            {
                ServerResources.DataAccess.Init.createBaseLocalisedMediaPool();
            }
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

        public static void updateJson(string json, string name, string location = null)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....

            
            string appdataFolder = Path.Combine(folder, "FireFly");
            if (location == null)
            {
                using (FileStream fs = File.Create(appdataFolder + "/" + name + ".json"))
                {
                    // Add some text to file    
                    Byte[] file = new UTF8Encoding(true).GetBytes(json);
                    fs.Write(file, 0, file.Length);

                }
            }
            else
            { //ToDo: when person quits when in the midst of saving it throws error
                using (FileStream fs = File.Create(location))
                {
                    // Add some text to file    
                    Byte[] file = new UTF8Encoding(true).GetBytes(json);
                    fs.Write(file, 0, file.Length);

                }
            }
        }

        public static void saveDevices()
        {
            /*// serialize JSON to a string and then write string to a file
            File.WriteAllText(@"c:\movie.json", JsonConvert.SerializeObject(ServerResources.ServerManagement.devices));
*/
            // serialize JSON directly to a file
            checkIfAppDataFolderExists();
           
            updateJson(JsonConvert.SerializeObject(ServerResources.ServerManagement.devices), "Devices", ServerManagement.settings.absoluteLocationOfAppData+"/temp/Devices.json");
            
            /*using (StreamWriter file = File.CreateText(@"c:\movie.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, ServerResources.ServerManagement.devices);
            }*/
        }



    }
}
