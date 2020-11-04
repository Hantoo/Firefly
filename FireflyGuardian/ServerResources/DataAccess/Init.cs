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
            if (File.Exists(appdataFolder + "/settings.json"))
            {
                string json = File.ReadAllText(appdataFolder + "/settings.json");
                   SettingsModel settings = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsModel>(json);
                FireflyGuardian.ServerResources.ServerManagement.settings = settings;
            }
            else
            {
                FireflyGuardian.ServerResources.ServerManagement.settings = new SettingsModel();
            }
            FireflyGuardian.ServerResources.ServerManagement.settings.absoluteLocationOfAppData = appdataFolder;
            // CreateDirectory will check if folder exists and, if not, create it.
            // If folder exists then CreateDirectory will do nothing.
            Directory.CreateDirectory(appdataFolder);
            Directory.CreateDirectory(appdataFolder+"/LocalisedMediaPool");
            FireflyGuardian.ServerResources.ServerManagement.settings.absoluteLocationOfLocalisedMedia = appdataFolder + "\\LocalisedMediaPool";
            createBaseLocalisedMediaPool();
        }

        public void createBaseLocalisedMediaPool()
        {
            Properties.Resources._0.Save(appdataFolder + "/LocalisedMediaPool/0.png");
            Properties.Resources._1.Save(appdataFolder + "/LocalisedMediaPool/1.png");
            Properties.Resources._2.Save(appdataFolder + "/LocalisedMediaPool/2.png");
            Properties.Resources._3.Save(appdataFolder + "/LocalisedMediaPool/3.png");
            Properties.Resources._4.Save(appdataFolder + "/LocalisedMediaPool/4.png");
            Properties.Resources._5.Save(appdataFolder + "/LocalisedMediaPool/5.png");
            Properties.Resources._6.Save(appdataFolder + "/LocalisedMediaPool/6.png");
            Properties.Resources._7.Save(appdataFolder + "/LocalisedMediaPool/7.png");
            Properties.Resources._8.Save(appdataFolder + "/LocalisedMediaPool/8.png");
            Properties.Resources._9.Save(appdataFolder + "/LocalisedMediaPool/9.png");
            Properties.Resources._10.Save(appdataFolder + "/LocalisedMediaPool/10.png");
            
        }

        public bool checkIfJSONExists()
        {

            // Check if file already exists. If yes, delete it.     
            if (File.Exists(appdataFolder+"/settings.json"))
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
            using (FileStream fs = File.Create(appdataFolder + "/settings.json"))
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
            using (FileStream fs = File.Create(appdataFolder + "/settings.json"))
            {
                // Add some text to file    
                Byte[] file = new UTF8Encoding(true).GetBytes(json);
                fs.Write(file, 0, file.Length);

            }
        }
    }
}
