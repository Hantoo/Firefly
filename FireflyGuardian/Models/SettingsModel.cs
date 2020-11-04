using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.Models
{
    class SettingsModel
    {
        public string ftpURL { get; set; }
        public string ftpUsername { get; set; }
        public string ftpPassword { get; set; }
        public string absoluteLocationOfAppData { get; set; }
        public string absoluteLocationOfLocalisedMedia { get; set; }
        
    }
}
