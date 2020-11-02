using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static void saveDevices(string path)
        {
            /*// serialize JSON to a string and then write string to a file
            File.WriteAllText(@"c:\movie.json", JsonConvert.SerializeObject(ServerResources.ServerManagement.devices));
*/
            // serialize JSON directly to a file
            checkIfAppDataFolderExists();
           
            updateJson(JsonConvert.SerializeObject(ServerResources.ServerManagement.devices), "Devices", path);
            
            /*using (StreamWriter file = File.CreateText(@"c:\movie.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, ServerResources.ServerManagement.devices);
            }*/
        }



    }
}
