using FireflyGuardian.Models;
using FireflyGuardian.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FireflyGuardian.ServerResources.DataAccess
{
    public class Init
    {

        private string appdataFolder;
        public bool firstTimeStartUp;
        public Init()
        {
            checkIfAppDataFolderExists();
            if (checkIfJSONExists())
            {
                //read
                firstTimeStartUp = false;
            }
            else
            {
                firstTimeStartUp = true;
                
            }
        }

        public void checkIfAppDataFolderExists()
        {
            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            appdataFolder = Path.Combine(folder, "FireFly");
            Directory.CreateDirectory(appdataFolder);
           
            if (File.Exists(appdataFolder + "/temp/Settings.json"))
            {
                string json = File.ReadAllText(appdataFolder + "/temp/Settings.json");
                   SettingsModel settings = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsModel>(json);
                FireflyGuardian.ServerResources.ServerManagement.settings = settings;
            }
            else
            {
                FireflyGuardian.ServerResources.ServerManagement.settings = new SettingsModel();
                Directory.CreateDirectory(appdataFolder + "/LocalisedMediaPool");

            }
            FireflyGuardian.ServerResources.ServerManagement.settings.absoluteLocationOfAppData = appdataFolder;
            // CreateDirectory will check if folder exists and, if not, create it.
            // If folder exists then CreateDirectory will do nothing.
            FireflyGuardian.ServerResources.ServerManagement.settings.absoluteLocationOfLocalisedMedia = appdataFolder + "\\LocalisedMediaPool";

            

            getRoutineJson();
        }

        public static void getRoutineJson()
        {
            if (FireflyGuardian.ServerResources.ServerManagement.routines == null)
            {
                FireflyGuardian.ServerResources.ServerManagement.routines = new List<RoutineModel>();
            }
            if (File.Exists(FireflyGuardian.ServerResources.ServerManagement.settings.absoluteLocationOfAppData + "/temp/Routines.json"))
            {
                string routineJSON = File.ReadAllText(FireflyGuardian.ServerResources.ServerManagement.settings.absoluteLocationOfAppData + "/temp/Routines.json");
                FireflyGuardian.ServerResources.ServerManagement.routines = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RoutineModel>>(routineJSON); ;
            }
            
        }

        public static void saveRoutineJson()
        {
            for(int i =0; i < ServerManagement.routines.Count; i++){
                ServerManagement.routines[i].suggestDelete = false;
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(ServerManagement.routines);

            using (FileStream fs = File.Create(FireflyGuardian.ServerResources.ServerManagement.settings.absoluteLocationOfAppData + "/temp/Routines.json"))
            {
                // Add some text to file    
                Byte[] file = new UTF8Encoding(true).GetBytes(json);
                fs.Write(file, 0, file.Length);

            }

           
        }

        public static void createBaseLocalisedMediaPool()
        {
            Console.WriteLine("AppData Folder: " + ServerManagement.settings.absoluteLocationOfAppData);
            Properties.Resources._0.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/0.png");
            Properties.Resources._1.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/1.png");
            Properties.Resources._2.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/2.png");
            Properties.Resources._3.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/3.png");
            Properties.Resources._4.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/4.png");
            Properties.Resources._5.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/5.png");
            Properties.Resources._6.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/6.png");
            Properties.Resources._7.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/7.png");
            Properties.Resources._8.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/8.png");
            Properties.Resources._9.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/9.png");
            Properties.Resources._10.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/10.png");

            Properties.Resources._11.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/11.png");
            Properties.Resources._12.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/12.png");
            Properties.Resources._13.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/13.png");
            Properties.Resources._14.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/14.png");
            Properties.Resources._15.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/15.png");
            Properties.Resources._16.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/16.png");
            Properties.Resources._17.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/17.png");
            Properties.Resources._18.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/18.png");
            Properties.Resources._19.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/19.png");
            Properties.Resources._20.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/20.png");
            Properties.Resources._21.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/21.png");
            Properties.Resources._22.Save(ServerManagement.settings.absoluteLocationOfAppData + "/LocalisedMediaPool/22.png");

            ServerManagement.settings.slotNames[0] = "Blank";
            ServerManagement.settings.slotNames[1] = "BS ISO Left";
            ServerManagement.settings.slotNames[2] = "BS ISO Left / Up";
            ServerManagement.settings.slotNames[3] = "BS ISO Up";
            ServerManagement.settings.slotNames[4] = "BS ISO Up / Right";
            ServerManagement.settings.slotNames[5] = "BS ISO Right";
            ServerManagement.settings.slotNames[6] = "BS ISO Right / Down";
            ServerManagement.settings.slotNames[7] = "BS ISO Down";
            ServerManagement.settings.slotNames[8] = "BS ISO Down / Left";
            ServerManagement.settings.slotNames[9] = "BS ISO No Entry";
            ServerManagement.settings.slotNames[10] = "BS ISO Exit";
            ServerManagement.settings.slotNames[11] = "Left";
            ServerManagement.settings.slotNames[12] = "Left / Up";
            ServerManagement.settings.slotNames[13] = "Up";
            ServerManagement.settings.slotNames[14] = "Up / Right";
            ServerManagement.settings.slotNames[15] = "Right";
            ServerManagement.settings.slotNames[16] = "Right / Down";
            ServerManagement.settings.slotNames[17] = "Down";
            ServerManagement.settings.slotNames[18] = "Down / Left";
            ServerManagement.settings.slotNames[19] = "No Entry";
            ServerManagement.settings.slotNames[20] = "Exit";
            ServerManagement.settings.slotNames[21] = "Orientation";
            ServerManagement.settings.slotNames[22] = "Colour Bars";

        }

        public bool checkIfJSONExists()
        {

            // Check if file already exists. If yes, delete it.     
            if (File.Exists(appdataFolder+ "/temp/Settings.json"))
            {
                return true;
            }
            else { 
                return false;
            }
        }

        public void generateSettings(string json)
        {
            // Create a new file    
            Directory.CreateDirectory(appdataFolder + "/temp");
            using (FileStream fs = File.Create(appdataFolder + "/temp/Settings.json"))
            {
                // Add some text to file    
                Byte[] file = new UTF8Encoding(true).GetBytes(json);
                fs.Write(file, 0, file.Length);
                
            }
        }

        public static void updateJson(string json)
        {

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string appdataFolder = Path.Combine(folder, "FireFly");
            using (FileStream fs = File.Create(appdataFolder + "/temp/Settings.json"))
            {
                // Add some text to file    
                Byte[] file = new UTF8Encoding(true).GetBytes(json);
                fs.Write(file, 0, file.Length);

            }


        }
    }
}
