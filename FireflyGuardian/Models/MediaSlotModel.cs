using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FireflyGuardian.Models
{
    class MediaSlotModel
    {
        public string image_source { get; set; }
        public string image_name { get; set; }
        public string image_symbol { get; set; }
        public int slotID { get; set; }
        public BitmapImage image { get; set; }
    }
}
