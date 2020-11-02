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

            // CreateDirectory will check if folder exists and, if not, create it.
            // If folder exists then CreateDirectory will do nothing.
            Directory.CreateDirectory(appdataFolder);
        }

        public bool checkIfJSONExists()
        {

            // Check if file already exists. If yes, delete it.     
            if (File.Exists(appdataFolder+"/settings.json"))
            {
                string json = File.ReadAllText(appdataFolder + "/settings.json");
                Models.SettingsModel settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.SettingsModel>(json);
                FireflyGuardian.ServerResources.ServerManagement.settings = settings;
                return true;
            }
            else { return false;
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
