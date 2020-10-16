using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireflyGuardian.Models
{
    class PageModel
    {
        public string name { get; set; }
        public string icon { get; set; }
        public object View { get; set; }
        public bool reloadOnActive { get; set; }
    
    }
}
